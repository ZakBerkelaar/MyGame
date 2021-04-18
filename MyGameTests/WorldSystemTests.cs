using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame;
using MyGame.Systems;

namespace MyGameTests
{
    [TestClass]
    public class WorldSystemTests
    {
        #region TestSystems
        public class MethodCalledException : Exception
        {
            public MethodCalledException() : base()
            {
            }

            public MethodCalledException(string message) : base(message)
            {
            }

            public MethodCalledException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }

        public class TestTrowSystem : WorldSystem
        {
            public override void Update()
            {
                throw new MethodCalledException();
            }
        }

        public class TestSystem : WorldSystem
        {
            public override void Update()
            {
                
            }
        }

        public class TestInjectionAttriuteSystem : WorldSystem
        {
            [SystemReference]
            public TestSystem system;

            public override void Update()
            {
                Assert.IsNotNull(system);
            }
        }
        #endregion

        [TestMethod]
        [ExpectedException(typeof(MethodCalledException))]
        public void TestSystemUpdate()
        {
            World world = new World(0, 0);
            TestTrowSystem system1 = new TestTrowSystem();
            world.AddSystem(system1);
            world.Update(0.0f);
        }

        [TestMethod]
        public void TestSystemUpdateRemote()
        {
            World world = new World(0, 0);
            world.isRemote = true;
            TestTrowSystem system1 = new TestTrowSystem();
            world.AddSystem(system1);
            world.Update(0.0f);
        }

        [TestMethod]
        public void TestGetSystem()
        {
            World world = new World(0, 0);
            TestTrowSystem system1 = new TestTrowSystem();
            world.AddSystem(system1);
            TestTrowSystem system2 = world.GetSystem<TestTrowSystem>();
            Assert.AreEqual(system1, system2);
        }

        [TestMethod]
        public void TestSystemInjection()
        {
            World world = new World(0, 0);
            TestSystem system1 = new TestSystem();
            TestInjectionAttriuteSystem system2 = new TestInjectionAttriuteSystem();
            world.AddSystem(system1);
            world.AddSystem(system2);
            world.Update(0.0f);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestSystemNullInjection()
        {
            World world = new World(0, 0);
            TestSystem system1 = new TestSystem();
            TestInjectionAttriuteSystem system2 = new TestInjectionAttriuteSystem();
            world.AddSystem(system2);
            world.AddSystem(system1);
            world.Update(0.0f);
        }
    }
}
