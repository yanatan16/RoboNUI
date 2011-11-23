using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Research.Kinect.Nui;
using Utilities.Messaging;
using log4net;

using RoboNui.Core;

using Emgu.CV;
using Emgu.CV.Structure;

namespace RoboNui.KinectAdapter
{
    /**
     * <summary>
     * This class monitors the Kinect data feed to track hand position.
     * It will find index and thumb points if in view
     * After a certain period, also passed in dynamically, the new Joint positions are forwarded to the JAT.
     * </summary>
     * 
     * <remarks>Author: Jon Eisen (jon.m.eisen@gmail.com)</remarks>
     * 
     */
    class HandTracker : Provider<JointSet>
    {
        /**
         * <summary>Tracking ID for the controller in control</summary>
         */
        public int ControllerTrackID { get; set; }

        /**
         * <summary>Use the right hand of the subject</summary>
         * <remarks>If false, use left</remarks>
         */
        public bool UseRightHand { get; set; }

        /**
         * <summary>
         * The time period between retrieval of the Kinect joints
         * </summary>
         */
        public double Period { get; set; }

        /**
         * <summary>
         * The depth threshold on the hand.
         * </summary>
         * <remarks>This is a configuration item</remarks>
         */
        public int BoundingBoxDepthThreshold { get; set; }


        /**
         * <summary>Log for logging events in this class</summary>
         */
        private ILog log;

        private Joint hand;
        private DateTime lastTime;
        private Runtime nui;

        public HandTracker(object nui) :
            base()
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");

            lastTime = DateTime.MaxValue;
            Period = 1000000000000;
            UseRightHand = true;
            ControllerTrackID = -1;

            this.nui = (Runtime)nui;
            this.nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(ht_SkeletonFrameReady);
            this.nui.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(ht_DepthFrameReady);
        }

        void ht_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            foreach (SkeletonData skel in e.SkeletonFrame.Skeletons)
            {
                if (skel.TrackingID == ControllerTrackID)
                {
                    if (UseRightHand)
                        hand = skel.Joints[JointID.HandRight];
                    else
                        hand = skel.Joints[JointID.HandLeft];
                }
            }
        }

        void ht_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            if (lastTime.AddMilliseconds(Period) < DateTime.Now)
            {
                Process(e.ImageFrame.Image, hand.Position);
                lastTime = DateTime.Now;
            }
            
        }

        private float[] convertDepthFrame(byte[] depthImage, int playerIndex)
        {
            float[] outputFrame = new float[depthImage.Length];
            for (int i = 0; i < depthImage.Length; i++)
            {
                int player = depthImage[i] & 0x07;
                if (player != playerIndex)
                {
                    outputFrame[i] = 0;
                }
                else
                {
                    //TODO Do I have to adjust depth values?
                    outputFrame[i] = (float)(depthImage[i] & 0xfff8 >> 3);
                }
            }
            return outputFrame;
        }

        void Process(PlanarImage depthImage, Vector handPosition)
        {
            // Convert the depth frame
            float[] depthFrame = convertDepthFrame(depthImage.Bits, ControllerTrackID);

            //#1 Get depth threshold around hand
            float depthX, depthY;
            short depthValue;
            nui.SkeletonEngine.SkeletonToDepthImage(handPosition, out depthX, out depthY, out depthValue);

            float depthLowerBound = (float)(depthValue - BoundingBoxDepthThreshold);
            float depthUpperBound = (float)(depthValue + BoundingBoxDepthThreshold);

            for (int i = 0; i < depthFrame.Length; i++)
            {
                if (depthFrame[i] > depthUpperBound || depthFrame[i] < depthLowerBound)
                    depthFrame[i] = 0;
            }

            //#2 Use OpenCV to get defect points and lines
            

            //#3 Convert defect points and lines into fingertip points
        }



    }
}
