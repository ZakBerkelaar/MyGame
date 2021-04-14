using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Generation;
using MyGame;

namespace MyGameTests
{
    [TestClass]
    public class WorldGeneratorTests
    {
        #region TestPasses
        private class TestPassData : IWorldGenerationData
        {
            public string TestString = "Testing";
        }

        private class PassDataCreator : IWorldGenPass
        {
            public void Pass(World world, Random r, IWorldGenerator generator)
            {
                var data = generator.GetData<TestPassData>();
                data.TestString = "Changed";
            }
        }

        private class NewPassDataCollector : IWorldGenPass
        {
            public void Pass(World world, Random r, IWorldGenerator generator)
            {
                var data = generator.GetData<TestPassData>();
                Assert.AreEqual("Testing", data.TestString);
            }
        }

        private class ChangedPassDataCollector : IWorldGenPass
        {
            public void Pass(World world, Random r, IWorldGenerator generator)
            {
                var data = generator.GetData<TestPassData>();
                Assert.AreEqual("Changed", data.TestString);
            }
        }
        #endregion

        [ClassInitialize]
        public static void WorldGeneratorTestsInit(TestContext context)
        {
            MyGame.Registration.TileRegister.RegisterTiles();
        }

        [TestMethod]
        public void TestAdd()
        {
            WorldGenerator generator = new WorldGenerator(0);
            var pass1 = new PassDataCreator();
            var pass2 = new NewPassDataCollector();
            generator.Add(pass1);
            generator.Add(pass2);
            Assert.AreEqual(pass1, generator.ElementAt(0));
            Assert.AreEqual(pass2, generator.ElementAt(1));
        }

        [TestMethod]
        public void TestCollectionInitializer()
        {
            var pass1 = new PassDataCreator();
            var pass2 = new NewPassDataCollector();
            WorldGenerator generator = new WorldGenerator(0)
            {
                pass1,
                pass2
            };
            Assert.AreEqual(pass1, generator.ElementAt(0));
            Assert.AreEqual(pass2, generator.ElementAt(1));
        }

        [TestMethod]
        public void TestAppend()
        {

            WorldGenerator generator = new WorldGenerator(0);
            var pass1 = new PassDataCreator();
            var pass2 = new NewPassDataCollector();
            generator.Add(pass1);
            generator.Add(pass2);
            var pass3 = new PassDataCreator();
            generator.Add(pass3);
            Assert.AreEqual(pass1, generator.ElementAt(0));
            Assert.AreEqual(pass2, generator.ElementAt(1));
            Assert.AreEqual(pass3, generator.ElementAt(2));
        }

        [TestMethod]
        public void TestInsertBefore()
        {
            WorldGenerator generator = new WorldGenerator(0);
            var pass1 = new PassDataCreator();
            var pass2 = new NewPassDataCollector();
            generator.Add(pass1);
            generator.Add(pass2);
            var pass3 = new PassDataCreator();
            generator.AddPassBefore(pass1, pass3);
            Assert.AreEqual(pass3, generator.ElementAt(0));
            Assert.AreEqual(pass1, generator.ElementAt(1));
            Assert.AreEqual(pass2, generator.ElementAt(2));
        }

        [TestMethod]
        public void TestInsertAfter()
        {
            WorldGenerator generator = new WorldGenerator(0);
            var pass1 = new PassDataCreator();
            var pass2 = new NewPassDataCollector();
            generator.Add(pass1);
            generator.Add(pass2);
            var pass3 = new PassDataCreator();
            generator.AddPassAfter(pass1, pass3);
            Assert.AreEqual(pass1, generator.ElementAt(0));
            Assert.AreEqual(pass3, generator.ElementAt(1));
            Assert.AreEqual(pass2, generator.ElementAt(2));
        }

        [TestMethod]
        public void TestPassData1()
        {
            WorldGenerator generator = new WorldGenerator(0);
            var pass1 = new PassDataCreator();
            var pass2 = new ChangedPassDataCollector();
            generator.Add(pass1);
            generator.Add(pass2);
            generator.GenWorld();
        }

        [TestMethod]
        public void TestPassData2()
        {
            WorldGenerator generator = new WorldGenerator(0);
            var pass2 = new NewPassDataCollector();
            generator.Add(pass2);
            generator.GenWorld();
        }
    }
}
