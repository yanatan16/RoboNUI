using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

using log4net;

using RoboNui.Core;
using RoboNui.Management;

using Utilities.Messaging;

using Microsoft.Research.Kinect.Audio;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace RoboNui.KinectAdapter
{
    /**
     * <summary>
     * This class provides extended voice control functionality for the RoboNUI system. 
     * It communicates solely with the State Manager for the purpose of commanding an action of the system.
     * </summary>
     * <remarks>Author: Jon Eisen (jon.m.eisen@gmail.com)</remarks>
     */
    class VoiceControlInterpreter : Provider<StateCommand>
    {
        /**
         * <summary>Log for logging events in this class</summary>
         */
        private ILog log;

        KinectAudioSource source;
        SpeechRecognitionEngine sre;
        Stream s;

        /**
         * <summary>
         * Constructor
         * </summary>
         */
        public VoiceControlInterpreter() :
            base()
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");

            source = new KinectAudioSource();
            source.FeatureMode = true;
            source.AutomaticGainControl = false;
            source.SystemMode = SystemMode.OptibeamArrayOnly;
            
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            RecognizerInfo ri = SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();

            if (ri == null)
            {
                log.Error("Could not find Kinect speech recognizer. Please refer to the sample requirements.");
                return;
            }

            log.DebugFormat("Using Kinect speech recognizer: {0}", ri.Name);

            sre = new SpeechRecognitionEngine(ri.Id);

            // Add commands here
            
            sre.LoadGrammar(BuildGrammar(ri.Culture));

            // Set up the callbacks
            sre.SpeechRecognized += sre_SpeechRecognized;
            sre.SpeechHypothesized += sre_SpeechHypothesized;

            s = source.Start();

            sre.SetInputToAudioStream(s,
                    new SpeechAudioFormatInfo(
                        EncodingFormat.Pcm, 16000, 16, 1,
                        32000, 2, null));

            sre.RecognizeAsync(RecognizeMode.Multiple);

            log.Info("Speech Recognition enabled.");
        }

        private Grammar BuildGrammar(CultureInfo ci)
        {
            // Prefix
            var prefix = new GrammarBuilder(new Choices(new string[] { "robo noo ee", "system", "control" }));
            prefix.Culture = ci;

            // On/Off Command
            var on = new Choices(new string[] { "on", "enable", "activate" });
            var on_val = new SemanticResultValue(on, true);

            var off = new Choices(new string[] { "off", "disable", "deactivate" });
            var off_val = new SemanticResultValue(off, false);

            var onoff = new Choices();
            onoff.Add(on_val);
            onoff.Add(off_val);
            var onoff_key = new SemanticResultKey("onoff", onoff);
            var onoff_key_val = new SemanticResultValue(onoff_key, "onoff");

            // Robot Selection Command
            var robot = new GrammarBuilder();
            var select = new GrammarBuilder(new Choices(new string[] { "select", "use", "" }));
            robot.Append(select);
            robot.Append(new Choices(new string[] { "", "robot" }));

            var arm_val = new SemanticResultValue("arm", RoboticServoControllerType.Arm);
            var mar_val = new SemanticResultValue("marionette", RoboticServoControllerType.Marionette);
            var rob_cho = new Choices();
            rob_cho.Add(arm_val);
            rob_cho.Add(mar_val);
            var rob_key = new SemanticResultKey("robot", rob_cho);
            robot.Append(rob_key);
            var rob_val = new SemanticResultValue(robot, "robot");

            // Side Selection Command
            var side = new GrammarBuilder();
            side.Append(select);
            var postfix = new Choices(new string[] {"", "side", "arm", "hand"} );

            var right = new SemanticResultValue("right", SideSelectionType.Right);
            var left = new SemanticResultValue("left", SideSelectionType.Left);
            var rightleft = new Choices();
            rightleft.Add(right);
            rightleft.Add(left);
            var rl_key = new SemanticResultKey("side", rightleft);

            side.Append(rl_key);
            side.Append(postfix);
            var side_val = new SemanticResultValue(side, "side");
            
            // Controller (User) Selection Command
            var controller = new GrammarBuilder();
            controller.Append(select);
            controller.Append(new Choices(new string[] { "", "controller", "user" }));
            controller.Append(new Choices(new string[] { "as me", "me" }));

            var con_val = new SemanticResultValue(controller, "controller");

            // Construct the commands grammar
            var commands = new Choices();
            commands.Add(onoff_key_val);
            commands.Add(rob_val);
            commands.Add(con_val);
            commands.Add(side_val);
            var com_key = new SemanticResultKey("command", commands);

            prefix.Append(com_key);

            return new Grammar(prefix);
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            log.DebugFormat("\nSpeech Recognized: \t{0}", e.Result.Text);
            var words = e.Result.Words;
            string com = (string) e.Result.Semantics["command"].Value;
            switch (com)
            {
                case "onoff":
                    var sc = new StateCommand();
                    sc.ComType = CommandType.Activation;
                    sc.Argument = (bool)e.Result.Semantics["onoff"].Value;
                    log.DebugFormat("Interpreted on/off command {0} from voice!", (bool) sc.Argument);
                    base.Send(sc);
                    break;
                case "robot":
                    sc = new StateCommand();
                    sc.ComType = CommandType.RoboticServoControllerSelect;
                    sc.Argument = (RoboticServoControllerType)e.Result.Semantics["robot"].Value;
                    log.DebugFormat("Interpreted robot selection {0} from voice!", ((RoboticServoControllerType) sc.Argument).ToString());
                    base.Send(sc);
                    break;
                case "controller":
                    sc = new StateCommand();
                    sc.ComType = CommandType.ControllerIDSelect;
                    if (source.SoundSourcePositionConfidence > 0.9)
                        sc.Argument = source.SoundSourcePosition;
                    log.DebugFormat("Interpreted controller selection at angle {0} from voice!", sc.Argument);
                    base.Send(sc);
                    break;
                default:
                    log.Error("Semantic 'command' unrecognized!");
                    break;
            }
        }

        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            log.DebugFormat("\nSpeech Hypothesized: \t{0}", e.Result.Text);
        }
    }
}
