using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Audio;
//using Microsoft.Speech.AudioFormat;
//using Microsoft.Speech.Recognition;

namespace RoboNui.KinectAdapter
{
    class CommandRecognitionAdapter
    {
        Dictionary<string, Action> callbacks;

        public CommandRecognitionAdapter(Dictionary<string, Action> callbacks)
        {
            this.callbacks = callbacks;

            var source = new KinectAudioSource();
            source.FeatureMode = true;
            source.AutomaticGainControl = false;
            source.SystemMode = SystemMode.OptibeamArrayOnly;

            

        }
    }
}
