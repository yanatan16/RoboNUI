using RoboNui.RobotAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using RoboNui.Core;
using System.Collections.Generic;
using Utilities.Messaging;

using log4net.Config;

namespace RoboNuiTest
{
    
    
    /// <summary>
    ///This is a test class for RoboticArmServoControllerTest and is intended
    ///to contain all RoboticArmServoControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RoboticArmServoControllerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Assert.IsNotNull(target);
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        static string defaultPortName = "COM5";
        static Dictionary<RoboticAngle, uint> defaultChannelMap;
        static ulong defaultSpeed;

        static RoboticArmServoController target;

        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext tc)
        {
            BasicConfigurator.Configure();

            defaultChannelMap = new Dictionary<RoboticAngle, uint>();
            RoboticAngle[] angles = { RoboticAngle.ArmBaseRotate, RoboticAngle.ArmElbowBend, RoboticAngle.ArmHandGrasp, 
                                       RoboticAngle.ArmShoulderLift, RoboticAngle.ArmWristRotate, RoboticAngle.ArmWristTilt };
            uint[] channels = { 0, 1, 2, 3, 4, 5 };
            for (uint i = 0; i < angles.Length; i++)
                defaultChannelMap.Add(angles[i], channels[i]);
            defaultSpeed = 1000;
            target = new RoboticArmServoController(defaultPortName, defaultChannelMap, defaultSpeed);
        }

        /// <summary>
        ///A test for GetAngles
        ///</summary>
//        [TestMethod()]
        public void StartGetAnglesTest()
        {
            List<RoboticAngle> roboticAngleList = new List<RoboticAngle>(defaultChannelMap.Keys);
            AngleSet expected = new AngleSet();
            for (int i = 0; i < roboticAngleList.Count; i++)
                expected.AngleMap.Add(roboticAngleList[i], 0);

            AngleSet actual;
            actual = target.GetAngles(roboticAngleList);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AngleMap, actual.AngleMap);
        }

        /// <summary>
        ///A test for Utilities.Messaging.IConsumer<RoboNui.Core.AngleSet>.Update
        ///</summary>
        [TestMethod()]
        [DeploymentItem("RoboticNaturalUserInterface.exe")]
        public void UpdateTest()
        {
            List<RoboticAngle> roboticAngleList = new List<RoboticAngle>(defaultChannelMap.Keys);
            AngleSet requested = new AngleSet();
            for (int i = 0; i < roboticAngleList.Count; i++)
                requested.AngleMap.Add(roboticAngleList[i], 0.5);
            (target as IConsumer<AngleSet>).Update(requested);

            AngleSet actual = target.GetAngles(roboticAngleList);
            Assert.AreEqual(requested, actual);
        }

        /// <summary>
        ///A test for IsMovementFinished
        ///</summary>
//        [TestMethod()]
        public void IsMovementFinishedTest()
        {
            List<RoboticAngle> roboticAngleList = new List<RoboticAngle>(defaultChannelMap.Keys);
            AngleSet requested = new AngleSet(); 
            for (int i = 0; i < roboticAngleList.Count; i++)
                requested.AngleMap.Add(roboticAngleList[i], 0.5);
            (target as IConsumer<AngleSet>).Update(requested);

            bool actual = target.IsMovementFinished();
            Assert.IsFalse(actual);

            Thread.Sleep(5000);

            actual = target.IsMovementFinished();
            Assert.IsTrue(actual);
        }

    }
}
