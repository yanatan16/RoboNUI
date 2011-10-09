using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Newtonsoft.Json;
using System.ComponentModel;
using Newtonsoft.Json.Converters;

using RoboNui.Core;

namespace RoboNui.Management
{
    /**
     * <summary>
     * Configuration structure for RoboNui.
     * 
     * Contains all necessary paramters to be read on Startup for the operation of the StateManager's control of the system.
     * </summary>
     *
     * <remarks>Author: Jon Eisen (jon.m.eisen@gmail.com) </remarks>
     */
    public class StateConfiguration
    {

        /**
         * <summary>
         * Configuration sub-struct for the RobotAdapter Assembly.
         * </summary>
         */
        public struct _RobotAdapterConfig
        {

            /**
             * <summary>
             * Configuration sub-struct for the Robotic Arm component(s).
             * </summary>
             */
            public struct _Arm
            {
                /// <summary>Serial Port name of the Robotic Arm's Servo Controller</summary>
                public string Port;

                /// <summary>Robotic Angle to Channel Number list</summary>
                public Dictionary<RoboticAngle, uint> Channels;

                /// <summary>Speed of motions for the Robotic Arm (in microseconds per second)</summary>
                public ulong Speed;
            }

            /**
             * <summary>
             * Configuration sub-struct for the Robotic Marionette component(s).
             * </summary>
             */
            public struct _Marionette
            {
            }

            /**
             * <summary>Accessible Field for the Robotic Arm configuration sub-struct</summary>
             */
            public _Arm Arm;

            /**
             * <summary>Accessible Field for the Robotic Marionette configuration sub-struct</summary>
             */
            public _Marionette Marionette;
        }

        /**
         * <summary>
         * Configuration sub-struct for the Kinect Adapter Assembly.
         * </summary>
         */
        public struct _KinectAdapter
        {
            /// <summary>Period to update the system</summary>
            /// <remarks>In milliseconds</remarks>
            public long Period;

        }

        /**
         * <summary>
         * Configuration sub-struct for the Core RoboNui Assembly.
         * </summary>
         */
        public struct _Core
        {

        }

        /**
         * <summary>Accessible Field for the Robot Adapter Assembly configuration sub-struct</summary>
         */
        public _RobotAdapterConfig RobotAdapter;

        /**
         * <summary>Accessible Field for the Kinect Adapter Assembly configuration sub-struct</summary>
         */
        public _KinectAdapter KinectAdapter;

        /**
         * <summary>Accessible Field for the Core RoboNui Assembly configuration sub-struct</summary>
         */
        public _Core Core;

        /**
         * <summary>Construct the Configuration with default values.</summary>
         */
        public StateConfiguration()
        {
            RobotAdapter.Arm.Port = "";
            RobotAdapter.Arm.Channels = new Dictionary<RoboticAngle, uint>();
            RobotAdapter.Arm.Speed = 0;

            RobotAdapter.Arm.Channels[RoboticAngle.ArmBaseRotate] = 0;
            RobotAdapter.Arm.Channels[RoboticAngle.ArmElbowBend] = 1;
            RobotAdapter.Arm.Channels[RoboticAngle.ArmHandGrasp] = 2;
            RobotAdapter.Arm.Channels[RoboticAngle.ArmShoulderLift] = 3;
            RobotAdapter.Arm.Channels[RoboticAngle.ArmWristRotate] = 4;
            RobotAdapter.Arm.Channels[RoboticAngle.ArmWristTilt] = 5;
        }

        /**
         * <summary>Construct the Configuration with a configuration file to load in custom configuration.</summary>
         */
        public static StateConfiguration ReadConfigFile(string fn)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.MissingMemberHandling = MissingMemberHandling.Error;

            JsonSerializer serializer = JsonSerializer.Create(settings);
            JsonReader reader = new JsonTextReader(new StreamReader(fn));
            StateConfiguration sc = serializer.Deserialize<StateConfiguration>(reader);

            StateConfiguration realSc = new StateConfiguration();
            sc.RobotAdapter.Arm.Channels = realSc.RobotAdapter.Arm.Channels;
            return sc;
        }
    }
}
