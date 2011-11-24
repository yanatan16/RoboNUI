using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using Utilities.Messaging;

using RoboNui.Core;
using RoboNui.RobotAdapter;
using RoboNui.KinectAdapter;

namespace RoboNui.Management
{
    /**
     * <summary>
     * This class is the primary manager of the RoboNUI system.
     * It performs actions based on commands from the <see cref="T:RoboNui.KinectAdapter.VoiceCommandInterpreter"/> or the GUI, as well as manage the current on/off state of the system. 
     * It should also control the primary data path through the system, determining who the current human controller is, as well as setting the <see cref="JointAngleTranslator"/> with the correct <see cref="T:RoboNui.Core.IRoboticModel"/> and the correct <see cref="T:RoboNui.Core.IAngleConsumer"/>.
     * </summary>
     */
    class StateManager : IConsumer<StateCommand>
    {

        //-------------Private Members---------------
        // Active boolean
        private bool _Active;

        // All Components
        private SkeletalJointMonitor sjm;
        private HandTracker ht;
        private VoiceControlInterpreter vci;
        private JointAngleTranslator jat;
        private RoboticArmServoController rsc_arm;
        private RoboticMarionetteServoController rsc_mar;

        // Currently active components
        private IConsumer<AngleSet> _CurrentAngleConsumer;
        private Provider<AngleSet> _CurrentAngleProvider;
        private IConsumer<JointSet> _CurrentJointConsumer;
        private Provider<JointSet> _CurrentJointProvider1;
        private Provider<JointSet> _CurrentJointProvider2;
        private IRoboticModel _CurrentRoboticModel;

        /**
         * <summary>Log for logging events in this class</summary>
         */
        private ILog log;

        /**
         * <summary>Configuration struct for the system</summary>
         */
        public StateConfiguration config;

        object runtimeNui;

        /**
         * <summary>Active system boolean. An inactive system will not operate.</summary>
         */
        public bool Active
        {
            get { return _Active; }
            set
            {
                _Active = value;
                if (value)
                {
                    if (CurrentJointProvider1 != null)
                        CurrentJointProvider1.Activate();
                    if (CurrentAngleProvider != null)
                        CurrentAngleProvider.Activate();

                    log.Info("System Activated!");
                }
                else
                {
                    if (CurrentJointProvider1 != null)
                        CurrentJointProvider1.Deactivate();
                    if (CurrentAngleProvider != null)
                        CurrentAngleProvider.Deactivate();

                    log.Info("System Deactivated.");
                }
            }
        }
        
        /**
         * <summary>
         * The currently active <see cref="Provider{JointSet}"/> in the system.
         * Setting this field will deactivate and clear the previous one and activate and enable the new one.
         * </summary>
         */
        Provider<JointSet> CurrentJointProvider1 { 
            get 
            {
                return _CurrentJointProvider1;
            }
            set 
            {
                if (_CurrentJointProvider1 != null)
                {
                    _CurrentJointProvider1.ClearAllConsumers();
                    if (Active)
                        _CurrentJointProvider1.Deactivate();
                }
                _CurrentJointProvider1 = value;
                if (_CurrentJointProvider1 != null)
                {
                    if (_CurrentJointConsumer != null)
                        _CurrentJointProvider1.AddConsumer(_CurrentJointConsumer);
                    if (Active)
                        _CurrentJointProvider1.Activate();
                }
            }
        }


        /**
         * <summary>
         * The currently active <see cref="Provider{JointSet}"/> in the system.
         * Setting this field will deactivate and clear the previous one and activate and enable the new one.
         * </summary>
         */
        Provider<JointSet> CurrentJointProvider2
        {
            get
            {
                return _CurrentJointProvider2;
            }
            set
            {
                if (_CurrentJointProvider2 != null)
                {
                    _CurrentJointProvider2.ClearAllConsumers();
                    if (Active)
                        _CurrentJointProvider2.Deactivate();
                }
                _CurrentJointProvider2 = value;
                if (_CurrentJointProvider2 != null)
                {
                    if (_CurrentJointConsumer != null)
                        _CurrentJointProvider2.AddConsumer(_CurrentJointConsumer);
                    if (Active)
                        _CurrentJointProvider2.Activate();
                }
            }
        }

        /**
         * <summary>
         * The currently active <see cref="IConsumer{JointSet}"/> in the system.
         * Setting this field will clear the previous one and enable the new one.
         * </summary>
         */
        IConsumer<JointSet> CurrentJointConsumer
        {
            get
            {
                return _CurrentJointConsumer;
            }
            set
            {
                if (_CurrentJointConsumer != null &&
                    _CurrentJointProvider1 != null)
                {
                    _CurrentJointProvider1.RemoveConsumer(_CurrentJointConsumer);
                }
                _CurrentJointConsumer = value;
                if (_CurrentJointConsumer != null &&
                    _CurrentJointProvider1 != null)
                {
                    _CurrentJointProvider1.AddConsumer(_CurrentJointConsumer);
                }
            }
        }

        /**
        * <summary>
        * The currently active <see cref="Provider{AngleSet}"/> in the system.
         * Setting this field will deactivate and clear the previous one and activate and enable the new one.
        * </summary>
        */
        Provider<AngleSet> CurrentAngleProvider
        {
            get
            {
                return _CurrentAngleProvider;
            }
            set
            {
                if (_CurrentAngleProvider != null)
                {
                    _CurrentAngleProvider.ClearAllConsumers();
                    if (Active)
                        _CurrentAngleProvider.Deactivate();
                }
                _CurrentAngleProvider = value;
                if (_CurrentAngleProvider != null)
                {
                    if (_CurrentAngleConsumer != null)
                        _CurrentAngleProvider.AddConsumer(_CurrentAngleConsumer);
                    if (Active)
                        _CurrentAngleProvider.Activate();
                }
            }
        }

        /**
         * <summary>
         * The currently active <see cref="IConsumer{AngleSet}"/> in the system.
         * Setting this field will deactivate and clear the previous one and activate and enable the new one.
         * </summary>
         */
        IConsumer<AngleSet> CurrentAngleConsumer
        {
            get
            {
                return _CurrentAngleConsumer;
            }
            set
            {
                if (_CurrentAngleConsumer != null &&
                    _CurrentAngleProvider != null)
                {
                    _CurrentAngleProvider.RemoveConsumer(_CurrentAngleConsumer);
                }
                _CurrentAngleConsumer = value;
                if (_CurrentAngleConsumer != null &&
                    _CurrentAngleProvider != null)
                {
                    _CurrentAngleProvider.AddConsumer(_CurrentAngleConsumer);
                }
            }
        }

        /**
         * <summary>The current <see cref="IRoboticModel"/> setup for the SJM</summary>
         * <seealso cref="SkeletalJointMonitor"/>
         */
        IRoboticModel CurrentRoboticModel
        {
            get
            {
                return _CurrentRoboticModel;
            }
            set
            {
                _CurrentRoboticModel = value;
                jat.Model = _CurrentRoboticModel;
                sjm.InterestedJoints = _CurrentRoboticModel.NeededJoints;
            }
        }

        /**
         * <summary>The current human controller</summary>
         */
        public int CurrentControllerID
        {
            get { return sjm.ControllerTrackID; }
            set { sjm.ControllerTrackID = value; }
        }

        /**
         * <summary>A list of possible controllers in view of the Kinect.</summary>
         */
        public List<int> PossibleControllerIDs
        {
            get { return sjm.PossibleTrackIDs; }
            set { sjm.PossibleTrackIDs = value; }
        }

        /**
         * <summary>
         * Constructor for the State Manager. Set all current components to null.
         * </summary>
         */
        public StateManager(StateConfiguration cfg, object nui)
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");

            config = cfg;
            runtimeNui = nui;

            sjm = null;
            vci = null;
            jat = null;
            rsc_arm = null;
            rsc_mar = null;

            CurrentJointProvider1 = null;
            CurrentJointConsumer = null;
            CurrentAngleProvider = null;
            CurrentAngleConsumer = null;
        }

        /**
         * <summary>
         * Construct the system components.
         * </summary>
         */
        public void Initialize()
        {
            // Core
            jat = new JointAngleTranslator();

            // Kinect Adapter
            sjm = new SkeletalJointMonitor(runtimeNui);
            ht = new HandTracker(runtimeNui);

            // Robot Adapter
            rsc_arm = new RoboticArmServoController(config.RobotAdapter.Arm.Port, config.RobotAdapter.Arm.Channels, config.RobotAdapter.Arm.Speed);
            rsc_mar = new RoboticMarionetteServoController(config.RobotAdapter.Marionette.Port, config.RobotAdapter.Marionette.Channels);

            Startup();
            log.Info("Default System configuration Initialized.");
        }

        /**
         * <summary>
         * Start the system up in a default configuration.
         * 
         * The SJM, JAT, and Robotic Arm RSC will be enabled.
         * </summary>
         */
        public void Startup()
        {
            IRoboticModel irm;
            if (config.RobotAdapter.UseArm == 1)
            {
                // Set up SJM-JAT-RSC/Arm path
                irm = new RoboticArmModel();
                CurrentAngleConsumer = rsc_arm;
            } else {
                irm = new RoboticMarionetteModel();
                CurrentAngleConsumer = rsc_mar;
            }

            sjm.InterestedJoints = irm.NeededJoints;
            sjm.Period = config.KinectAdapter.Period;
            CurrentJointProvider1 = sjm;
            ht.Period = config.KinectAdapter.Period;
            CurrentJointProvider2 = ht;

            CurrentRoboticModel = irm;
            CurrentJointConsumer = jat;
            CurrentAngleProvider = jat;

            // And...GO!
            Active = true;
        }

        /**
         * <summary>Handle the State Command by performing an action in the State Manager</summary>
         * <seealso cref="M:IConsumer{StateCommand}.Update" />
         */
        void IConsumer<StateCommand>.Update(StateCommand com)
        {
            switch (com.ComType)
            {
                case CommandType.Activation:
                    Active = (bool) com.Argument;
                    break;

                case CommandType.ControllerIDSelect:
                    sjm.ControllerTrackID = (int) com.Argument;
                    ht.ControllerTrackID = (int)com.Argument;
                    break;

                case CommandType.RoboticServoControllerSelect:
                    switch ((RoboticServoControllerType)com.Argument)
                    {
                        case RoboticServoControllerType.Arm:
                            CurrentAngleConsumer = rsc_arm;
                            break;
                        case RoboticServoControllerType.Marionette:
                            CurrentAngleConsumer = rsc_mar;
                            break;
                        default:
                            log.Warn("RoboticServoControllerSelect has unknown RoboticServoControllerType.");
                            break;
                    }
                    break;

                default:
                    log.Warn("Unable to handle command type: " + com.ComType);
                    break;
            }
        }
    }
}
