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
        public float BoundingBoxDepthThreshold { get; set; }


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

        private float[,,] convertDepthFrame(PlanarImage depthImage, int playerIndex)
        {
            float[,,] outputFrame = new float[depthImage.Width, depthImage.Height, 1];
            int elementStride = 2;
            int vectorStride = elementStride * depthImage.Height;
            for (int i = 0; i < depthImage.Bits.Length; i+=2)
            {
                int player = depthImage.Bits[i] & 0x07;
                int value = (depthImage.Bits[i + 1] << 5) | (depthImage.Bits[i] >> 3);
                int jw = i / vectorStride;
                int jh = (i / elementStride) % depthImage.Height;
                if (player != playerIndex)
                {
                    outputFrame[jw, jh, 0] = 0;
                }
                else
                {
                    //TODO Do I have to calibrate depth values?
                    outputFrame[jw, jh, 0] = (float) value;
                }
            }
            return outputFrame;
        }

        void Process(PlanarImage depthImage, Vector handPosition)
        {
            //#1 Get depth threshold around hand
            float depthX, depthY;
            short depthValue;
            nui.SkeletonEngine.SkeletonToDepthImage(handPosition, out depthX, out depthY, out depthValue);

            int depthActual = (depthValue & 0xfff8) >> 3;
            int playerIndex = depthValue & 0x07;

            float depthLowerBound = (depthActual - BoundingBoxDepthThreshold);
            float depthUpperBound = (depthActual + BoundingBoxDepthThreshold);

            // Convert the depth frame
            float[,,] depthFrame = convertDepthFrame(depthImage, playerIndex);

            for (int i = 0; i < depthFrame.GetUpperBound(0); i++)
            {
                for (int j = 0; j < depthFrame.GetUpperBound(1); j++)
                {
                    if (depthFrame[i,j,0] > depthUpperBound || 
                        depthFrame[i,j,0] < depthLowerBound)
                        depthFrame[i,j,0] = 0;
                }
            }

            //#2 Use OpenCV to get defect points and lines
            Image<Gray,Single> image = new Image<Gray, Single>(depthFrame);
            
            //CvInvoke.cvFindContours(

            //#3 Convert defect points and lines into fingertip points
        }



    }
}
