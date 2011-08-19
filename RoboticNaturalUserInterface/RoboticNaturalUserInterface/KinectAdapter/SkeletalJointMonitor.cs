using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

using Utilities.Messaging;

using Microsoft.Research.Kinect.Nui;

using log4net;

namespace RoboNui.KinectAdapter
{
    /**
     * <summary>
     * This class monitors the Kinect data feed for updates to the Joint positions for certain monitored joints. 
     * The joints in question are passed in dynamically.
     * After a certain period, also passed in dynamically, the new Joint positions are forwarded to the JAT.
     * </summary>
     * 
     * <remarks>Author: Jon Eisen (jon.m.eisen@gmail.com)</remarks>
     * 
     */
    class SkeletalJointMonitor : Provider<JointSet>
    {
        /**
         * <summary>Log for logging events in this class</summary>
         */
        private ILog log;

        /**
         * <summary>
         * The time period between retrieval of the Kinect joints
         * </summary>
         */
        public double Period { get; set; }

        /**
         * <summary> The list of joints the SJM should forward every <see cref="F:Period"/>.</summary>
         */
        public List<JointID> InterestedJoints { get; set; }

        /**
         * <summary>The current controller's <see cref="F:Microsoft.Research.Kinect.Nui.SkeletonData.TrackID"/>.
         * 
         * Selected from the <see cref="PossibleTrackIDs"/></summary>
         */
        public int ControllerTrackID { get; set; }

        /**
         * <summary>A list of possible <see cref="F:Microsoft.Research.Kinect.Nui.SkeletonData.TrackID"/> to choose <see cref="F:ControllerTrackID"/> from.</summary>
         */
        public List<int> PossibleTrackIDs { get; set; }

        /**
         * <summary>Nui runtime for communicating with the Kinect</summary>
         */
        private Runtime nui;

        /**
         * <summary>The last time the joints were published</summary>
         */
        private DateTime lastTime;

        /**
         * <summary> Constructor of this class. This starts up the Nui.Runtime and initializes the class fields. </summary>
         */
        public SkeletalJointMonitor() :
            base()
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");

            Period = Double.MaxValue;
            InterestedJoints = new List<JointID>();
            lastTime = DateTime.MaxValue;
            ControllerTrackID = 0;
            PossibleTrackIDs = new List<int>();

            nui = new Runtime(); 
            try
            {
                nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking);
            }
            catch (InvalidOperationException e)
            {
                log.Error("Nui.Runtime initialization failed. Please make sure Kinect device is plugged in.", e);
                return;
            }

            try
            {
                nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.ColorInDepthSpace);
                nui.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
            }
            catch (InvalidOperationException e)
            {
                log.Error("Failed to open streams. Make sure resolution and type are specified correctly.", e);
                return;
            }

            lastTime = DateTime.Now;
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
        }

        /**
         * <summary>
         * This method is an Event Handler for new Skeleton data coming available from the Kinect Nui.
         * It periodically publishes a Joint Set composed of joints from the requested <see cref="F:ControllerTrackID"/> and the joints noted in 
         * <see cref="F:InterestedJoints"/>. It also populates the field <see cref="F:PossibleTrackIDs"/> every call.
         * </summary>
         * <param name="e">Skeleton Frame arguments</param>
         * <param name="sender">The caller of this method</param>
         */
        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame frame = e.SkeletonFrame;
            
            // Publish joints
            // Check to make sure period has been satisfied
            if (lastTime.AddMilliseconds(Period) < DateTime.Now)
            {
                JointSet jset = new JointSet();

                foreach (SkeletonData human in frame.Skeletons)
                {
                    if (human.TrackingID == ControllerTrackID)
                    {
                        foreach (ControllerJoints jcj in InterestedJoints)
                        {
                            JointID jid = (JointID)jcj;
                            Joint j = human.Joints[jid];
                            Position3d pos = new Position3d(j.Position.X, j.Position.Y, j.Position.Z);
                            jset.JointMap.Add(jcj, pos);
                        }
                    }
                }

                log.Info("Publishing " + jset.JointMap.Count + " joints.");
                Send(jset);
                lastTime = DateTime.Now;
            }

            
            // Update possible ID's every time
            PossibleTrackIDs.Clear();
            foreach (SkeletonData h in frame.Skeletons)
            {
                PossibleTrackIDs.Add(h.TrackingID);
            }
        }
    }
}
