using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
            var activation = new Choices();
            activation.Add("on");
            activation.Add("off");

            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;
            gb.Append(activation);

            //Load the grammar into the speech recognizer
            var g = new Grammar(gb);
            sre.LoadGrammar(g);
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

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            log.DebugFormat("\nSpeech Recognized: \t{0}", e.Result.Text);
        }

        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            log.DebugFormat("\nSpeech Hypothesized: \t{0}", e.Result.Text);
        }
    }
}
