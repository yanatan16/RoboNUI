﻿using RoboNui.KinectAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using log4net.Config;

namespace RoboNuiTest
{
    
    
    /// <summary>
    ///This is a test class for VoiceControlInterpreterTest and is intended
    ///to contain all VoiceControlInterpreterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VoiceControlInterpreterTest
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

        
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext tc)
        {
            BasicConfigurator.Configure();
        }

        /// <summary>
        ///A test for VoiceControlInterpreter Constructor
        ///</summary>
        [TestMethod()]
        public void VoiceControlInterpreterConstructorTest()
        {
            VoiceControlInterpreter target = new VoiceControlInterpreter();
            
        }
    }
}
