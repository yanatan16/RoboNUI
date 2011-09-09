using RoboNui.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RoboNui.RobotAdapter;
using Utilities.Messaging;
using System.Collections.Generic;

namespace RoboNuiTest
{
    
    
    /// <summary>
    ///This is a test class for JointAngleTranslatorTest and is intended
    ///to contain all JointAngleTranslatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JointAngleTranslatorTest : IConsumer<AngleSet>
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
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for JointAngleTranslator Constructor
        ///</summary>
        [TestMethod()]
        public void JointAngleTranslatorConstructorTest()
        {
            IRoboticModel model = new RoboticArmModel();
            JointAngleTranslator target = new JointAngleTranslator(model);
            (target as Provider<AngleSet>).AddConsumer(this);
            Assert.IsNotNull(target);
        }

        public AngleSet response;

        /// <summary>
        /// Get the response from the JAT
        /// </summary>
        /// <param name="angles"></param>
        void IConsumer<AngleSet>.Update(AngleSet angles)
        {
            response = angles;
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            IRoboticModel model = new RoboticArmModel();
            JointAngleTranslator target = new JointAngleTranslator(model);
            (target as Provider<AngleSet>).AddConsumer(this);
            (target as Provider<AngleSet>).Activate();

            JointSet js = new JointSet();
            for (int i = 0; i < model.NeededJoints.Count; i++)
                js.JointMap.Add(model.NeededJoints[i], new Position3d(i, 0, 0));
            target.Update(js);

            RoboticAngle[] angles0 = { RoboticAngle.ArmBaseRotate, RoboticAngle.ArmWristRotate, RoboticAngle.ArmHandGrasp };
            RoboticAngle[] anglesPi = { RoboticAngle.ArmElbowBend, RoboticAngle.ArmWristTilt, RoboticAngle.ArmShoulderLift };
            for (int i = 0; i < angles0.Length; i++)
                Assert.AreEqual(response.AngleMap[angles0[i]], 0.0);

            for (int i = 0; i < anglesPi.Length; i++)
                Assert.AreEqual(response.AngleMap[anglesPi[i]], Math.PI);
        }
    }
}
