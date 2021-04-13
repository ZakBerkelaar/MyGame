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
    public class IDStringTests
    {
        [TestMethod]
        public void TestComponents()
        {
            const string @namespace = "TestSpace";
            const string name = "TestName";
            const string type = "TestType";
            IDString idString = new IDString(@namespace, type, name);
            Assert.AreEqual(@namespace, idString.Namespace);
            Assert.AreEqual(type, idString.Type);
            Assert.AreEqual(name, idString.Name);
        }

        [TestMethod]
        public void TestDefaultNamespace()
        {
            const string name = "TestName";
            const string type = "TestType";
            IDString idString = new IDString(type, name);
            Assert.AreEqual("MyGame", idString.Namespace);
            Assert.AreEqual(type, idString.Type);
            Assert.AreEqual(name, idString.Name);
        }

        [TestMethod]
        public void TestParse()
        {
            const string name = "TestName";
            const string type = "TestType";
            IDString idString = new IDString($"{type}/{name}");
            Assert.AreEqual("MyGame", idString.Namespace);
            Assert.AreEqual(type, idString.Type);
            Assert.AreEqual(name, idString.Name);
        }

        [TestMethod]
        public void TestFullParse()
        {
            const string @namespace = "TestSpace";
            const string name = "TestName";
            const string type = "TestType";
            IDString idString = new IDString($"{@namespace}:{type}/{name}");
            Assert.AreEqual(@namespace, idString.Namespace);
            Assert.AreEqual(type, idString.Type);
            Assert.AreEqual(name, idString.Name);
        }

        [TestMethod]
        public void TestImplicitConversion()
        {
            const string @namespace = "TestSpace";
            const string name = "TestName";
            const string type = "TestType";
            IDString idString = new IDString(@namespace, type, name);
            IDString idString2 = new IDString(idString);
            Assert.AreEqual(idString, idString2);
        }
    }
}
