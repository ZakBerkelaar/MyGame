using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MyGame;

namespace MyGameTests
{

    [TestClass]
    public class Vector2Tests
    {
        [TestMethod]
        public void TestComponents()
        {
            Vector2 vector2 = new Vector2(5.0f, 10.0f);
            Assert.AreEqual(5.0f, vector2.x);
            Assert.AreEqual(10.0f, vector2.y);
        }

        [TestMethod]
        public void TestAddition()
        {
            Vector2 vec1 = new Vector2(7.0f, 12.0f);
            Vector2 vec2 = new Vector2(1.0f, 20.0f);
            Assert.AreEqual(new Vector2(8.0f, 32.0f), vec1 + vec2);
        }

        [TestMethod]
        public void TestSubtraction()
        {
            Vector2 vec1 = new Vector2(7.0f, 12.0f);
            Vector2 vec2 = new Vector2(1.0f, 20.0f);
            Assert.AreEqual(new Vector2(-6.0f, 8.0f), vec2 - vec1);
        }

        [TestMethod]
        public void TestNegation()
        {
            Vector2 vec1 = new Vector2(7.0f, 12.0f);
            Assert.AreEqual(new Vector2(-7.0f, -12.0f), -vec1);
        }

        [TestMethod]
        public void TestScalarMultiplication()
        {
            Vector2 vec1 = new Vector2(7.0f, 12.0f);
            Assert.AreEqual(new Vector2(14.0f, 24.0f), vec1 * 2);
            Assert.AreEqual(new Vector2(14.0f, 24.0f), 2 * vec1);
        }

        [TestMethod]
        public void TestMagnitude()
        {
            Vector2 vec = new Vector2(3.0f, 4.0f);
            Assert.AreEqual(5.0f, vec.Magnitude);
        }

        [TestMethod]
        public void TestSqrMagnitude()
        {
            Vector2 vec = new Vector2(3.0f, 4.0f);
            Assert.AreEqual(25.0f, vec.SqrMagnitude);
        }

        [TestMethod]
        public void TestDistance()
        {
            Vector2 vec1 = new Vector2(7.0f, 21.0f);
            Vector2 vec2 = new Vector2(10.0f, 25.0f);
            Assert.AreEqual(5.0f, Vector2.Distance(vec1, vec2));
        }

        [TestMethod]
        public void TestSqrDistance()
        {
            Vector2 vec1 = new Vector2(7.0f, 21.0f);
            Vector2 vec2 = new Vector2(10.0f, 25.0f);
            Assert.AreEqual(25.0f, Vector2.SquareDistance(vec1, vec2));
        }
    }
}
