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
        public List<ControllerJoints> InterestedJoints
        {
            set
            {
                _InterestedJoints.Clear();
                foreach (ControllerJoints cj in value)
                {
                    _InterestedJoints.Add((JointID)cj);
                }
            }
        }
        private List<JointID> _InterestedJoints;

        /**
         * <summary>The current controller's <see cref="F:Microsoft.Research.Kinect.Nui.SkeletonData.TrackID"/>.
         * 
         * Selected from the <see cref="PossibleTrackIDs"/></summary>
         */
        public int ControllerTrackID { get; set; }

        /**
         * <summary>A list of possible <see cref="F:Microsoft.Research.Kinect.Nui.SkeletonData.TrackID"/> to choose <see cref="F:ControllerTrackID"/> from.</summary>
         */
        private Dictionary<int, Vector> PossibleTrackIDPositions { get; set; }

        /**
         * <summary>The last time the joints were published</summary>
         */
        private DateTime lastTime;

        private bool reset = false;

        /**
         * <summary> Constructor of this class. This starts up the Nui.Runtime and initializes the class fields. </summary>
         */
        public SkeletalJointMonitor(object nui) :
            base()
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");

            Period = 1000000000000;
            _InterestedJoints = new List<JointID>();
            lastTime = DateTime.MaxValue;
            ControllerTrackID = -1;
            PossibleTrackIDPositions = new Dictionary<int, Vector>();

            Runtime rnui = (Runtime)nui;
            lastTime = DateTime.Now;
            rnui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
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
                    if (human.TrackingID == ControllerTrackID && human.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        foreach (JointID jid in _InterestedJoints)
                        {
                            Joint j = human.Joints[jid];
                            Position3d pos = new Position3d(j.Position.X, j.Position.Y, j.Position.Z);
                            jset.JointMap[(ControllerJoints)jid] = pos;
                        }
                    }
                }

                if (jset.JointMap.Count > 0)
                {
                    log.DebugFormat("Publishing {0} joints.", jset.JointMap.Count);
                    Send(jset);
                    reset = false;
                }
                else if (!reset)
                {
                    log.Debug("Resetting joints.");
                    Send(jset);
                    reset = true;
                }

                // Update possible ID's every time
                PossibleTrackIDPositions.Clear();
                foreach (SkeletonData h in frame.Skeletons)
                {
                    if (h.TrackingState == SkeletonTrackingState.Tracked)
                        PossibleTrackIDPositions[h.TrackingID] = h.Position;
                }

                lastTime = DateTime.Now;
            }

            // Until control of this has been enabled.
            //ControllerTrackID = PossibleTrackIDs.Max<int>();
        }

        public int getTrackIDFromAngle(double angle)
        {
            double threshold = 0.1;
            int winner = -1;
            foreach (var trackIDPosPair in PossibleTrackIDPositions)
            {
                double tang = Math.Atan2(trackIDPosPair.Value.X, trackIDPosPair.Value.Y);
                if (angle - tang < threshold)
                {
                    if (winner > -1)
                    {
                        double windist = Math.Sqrt(Math.Pow(PossibleTrackIDPositions[winner].X, 2) + Math.Pow(PossibleTrackIDPositions[winner].Y, 2));
                        double chadist = Math.Sqrt(Math.Pow(trackIDPosPair.Value.X, 2) + Math.Pow(trackIDPosPair.Value.Y, 2));
                        if (chadist < windist)
                            winner = trackIDPosPair.Key;
                    }
                    else
                        winner = trackIDPosPair.Key;
                }
            }
            return winner;
        }
    }
}
