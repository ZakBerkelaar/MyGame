using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame;

namespace MyGameTests
{
    [TestClass]
    public class ConfigTests
    {
        private Config config;

        [TestInitialize]
        public void ConfigTestInit()
        {
            config = new Config(new IDString("Testing", "Config", "Test"));
        }

        [TestMethod]
        public void TestString()
        {
            const string test = "";
            config.Write("TestString", test);
            Assert.AreEqual(test, config.Read<string>("TestString"));
        }

        [TestMethod]
        public void TestInt()
        {
            const int test = 21;
            config.Write("TestInt", test);
            Assert.AreEqual(test, config.Read<int>("TestInt"));
        }
        

        [TestMethod]
        public void TestFloat()
        {
            const float test = 6.9f;
            config.Write("TestFloat", test);
            Assert.AreEqual(test, config.Read<float>("TestFloat"));
        }

        [TestMethod]
        public void TestDouble()
        {
            const double test = 4.2d;
            config.Write("TestDouble", test);
            Assert.AreEqual(test, config.Read<double>("TestDouble"));
        }

        [TestMethod]
        public void TestDecimal()
        {
            const decimal test = 12.1m;
            config.Write("TestDecimal", test);
            Assert.AreEqual(test, config.Read<decimal>("TestDecimal"));
        }

        [TestMethod]
        public void TestDuplicateKey()
        {
            config.Write("TestDupe", 0);
            config.Write("TestDupe", 5);
            Assert.AreEqual(5, config.Read<int>("TestDupe"));
        }
    }
}
