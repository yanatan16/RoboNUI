using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.KinectAdapter;

using Utilities.Messaging;

using log4net;

namespace RoboNui.Core
{
    /**
     * <summary>
     * Thrown when the <see cref="JointAngleTranslator"/> has no <see cref="IRoboticModel"/> when its <see cref="M:JointAngleTranslator.Update"/> is called.
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
     * Base Class: <see cref="Provider{AngleSet}"/>
     * Interface: <see cref="IConsumer{JointSet}"/>
     * </summary>
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     * 
     * <seealso cref="T:Provider{AngleSet}"/>
     * <seealso cref="T:IConsumer{JointSet}"/>
     */
    class JointAngleTranslator : ProviderConsumer <AngleSet, JointSet>
    {
        /**
         * <summary>Log for logging events in this class</summary>
         */
        private ILog log;

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
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");

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

        /**
         * <summary>
         * This will translate the positions to angles through the Robotic Model.
         * Then it will forward those angles to the Robotic Consumers who are registered.
         * </summary>
         * <exception cref="NoRoboticModelException">Thrown when Model is not set</exception>
         * <seealso cref="IRoboticModel"/>
         * <remarks>See <see cref="M:IConsumer{JointSet}.Update"/> for inherited method comments. </remarks>
         */
        public override void Update(JointSet js)
        {
            log.Debug("Updated JointSet. Translating to AngleSet and Sending");
            if (Model != null)
                base.Send(Model.Translate(js));
            else
            {
                NoRoboticModelException e = new NoRoboticModelException();
                log.Error("Tried to update angles upon receipt of joints. No Model Found.", e);
                throw e;
            }
        }

    }
}
