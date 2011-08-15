using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.KinectAdapter;

using Microsoft.Research.Kinect.Nui;

namespace RoboNui.Core
{
    /**
     * <summary>
     * Thrown when the <see cref="JointAngleTranslator"/> has no <see cref="IRoboticModel"/> when its <see cref="JointAngleTranslator.JointPositions"/> is set.
     * </summary>
     * 
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     * 
     * <seealso cref="Exception"/>
     * <seealso cref="JointAngleTranslator"/>
     * <seealso cref="IRoboticModel"/>
     */
    class NoRoboticModelException : Exception
    {
        public NoRoboticModelException() :
            base("No IRoboticModel provided to the Joint Angle Translator")
        {
        }
    }

    /**
     * <summary>
     * This component translates between Kinect Joints and Robotic Angles
     * After being called with new Joints' positions, the JAT forwards them
     * to the currently active IRoboticAngleConsumer, which is set by the
     * State Manager.
     * 
     * Base Class: <see cref="RoboticAngleProvider"/>
     * </summary>
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     * 
     * <seealso cref="RoboticAngleProvider"/>
     */
    class JointAngleTranslator : RoboticAngleProvider
    {
        /**
         * <summary>
         * Setter for the joint positions. 
         * This will translate the positions to angles through the Robotic Model.
         * Then it will forward those angles to the Robotic Consumers who are registered.
         * </summary>
         * <value>Positions of human controller's joints</value>
         * <exception cref="NoRoboticModelException">Thrown when Model is not set</exception>
         * <seealso cref="IRoboticModel"/>
         */
        public JointSet JointPositions
        {
            get
            {
                return JointPositions;
            }
            set 
            {
                if (Model != null)
                    base.SendAngles(Model.Translate(JointPositions));
                else
                    throw new NoRoboticModelException();
            }
        }

        /**
         * <summary>
         * The Model for translating between joint positions to robot angles
         * </summary>
         * <seealso cref="T:RoboNui.Core.IRoboticModel"/>
         */
        public IRoboticModel Model { set; get; }

        /**
         * <summary>
         * Constructor for this class
         * </summary>
         * 
         * <remarks>
         * Setting the JointPositions parameter will throw a <see cref="NoRoboticModelException"/> if Model is not set.
         * </remarks>
         */
        public JointAngleTranslator() :
            base()
        {
            Model = null;
        }

        /**
         * <summary>
         * Constructor requiring a model to be passed in.
         * </summary>
         * 
         * <param name="model">Robotic Model to use on construction</param>
         */
        public JointAngleTranslator(IRoboticModel model)
        {
            Model = model;
        }

    }
}
