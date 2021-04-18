using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MyGame;
using System.Drawing;
using System.Linq;

namespace MyGameTests
{
    [TestClass]
    public class GradientTests
    {
        [TestMethod]
        public void TestInt()
        {
            Gradient<int> gradient = new Gradient<int>();
            gradient.Add(0.0f, 0);
            gradient.Add(1.0f, 1);
            int result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestUInt()
        {
            Gradient<uint> gradient = new Gradient<uint>();
            gradient.Add(0.0f, 0u);
            gradient.Add(1.0f, 1u);
            uint result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0u, result);
        }

        [TestMethod]
        public void TestLong()
        {
            Gradient<long> gradient = new Gradient<long>();
            gradient.Add(0.0f, 0L);
            gradient.Add(1.0f, 1L);
            long result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0L, result);
        }

        [TestMethod]
        public void TestULong()
        {
            Gradient<ulong> gradient = new Gradient<ulong>();
            gradient.Add(0.0f, 0ul);
            gradient.Add(1.0f, 1ul);
            ulong result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0ul, result);
        }

        [TestMethod]
        public void TestFloat()
        {
            Gradient<float> gradient = new Gradient<float>();
            gradient.Add(0.0f, 0.0f);
            gradient.Add(1.0f, 1.0f);
            float result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0.5f, result, 0.00001f);
        }

        [TestMethod]
        public void TestDouble()
        {
            Gradient<double> gradient = new Gradient<double>();
            gradient.Add(0.0f, 0.0d);
            gradient.Add(1.0f, 1.0d);
            double result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0.5d, result);
        }

        [TestMethod]
        public void TestVec2()
        {
            Gradient<Vector2> gradient = new Gradient<Vector2>();
            gradient.Add(0.0f, new Vector2(0, 0));
            gradient.Add(1.0f, new Vector2(1, 1));
            Vector2 result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(new Vector2(0.5f, 0.5f), result);
        }

        [TestMethod]
        public void TestAdvancedLinearInterpolation()
        {
            Gradient<float> gradient = new Gradient<float>();
            gradient.Add(0.0f, -1.0f);
            gradient.Add(0.1f,  1.0f);
            gradient.Add(0.5f,  1.0f);
            gradient.Add(1.0f,  9.0f);
            Assert.AreEqual(0.0f, gradient.GetAtPosition(0.05f));
            Assert.AreEqual(1.0f, gradient.GetAtPosition(0.35f));
            Assert.AreEqual(5.0f, gradient.GetAtPosition(0.75f));
            Assert.AreEqual(1.8f, gradient.GetAtPosition(0.55f), 0.00001f);
        }

        [TestMethod]
        public void TestCosineInterpolation()
        {
            Gradient<float> gradient = new Gradient<float>(Gradient<float>.Interpolation.Cosine);
            gradient.Add(0.0f, 0.0f);
            gradient.Add(1.0f, 1.0f);
            float result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0.5f, result);
        }

        [TestMethod]
        public void TestAdvancedCosineInterpolation()
        {
            Gradient<float> gradient = new Gradient<float>(Gradient<float>.Interpolation.Cosine);
            gradient.Add(0.0f, -1.0f);
            gradient.Add(0.1f, 1.0f);
            gradient.Add(0.5f, 1.0f);
            gradient.Add(1.0f, 9.0f);
            Assert.AreEqual(0.0f, gradient.GetAtPosition(0.05f));
            Assert.AreEqual(1.0f, gradient.GetAtPosition(0.35f));
            Assert.AreEqual(5.0f, gradient.GetAtPosition(0.75f));
            float cos = (1 - Mathf.Cos(Mathf.Pi * (0.05f / 0.5f))) * 0.5f;
            float res = (1 * (1 - cos)) + (9 * cos);
            Assert.AreEqual(res, gradient.GetAtPosition(0.55f), 0.00001f);
        }

        [TestMethod]
        public void TestOutOfOrderStops()
        {
            Gradient<float> gradient = new Gradient<float>();
            gradient.Add(0.1f, 1.0f);
            gradient.Add(0.0f, -1.0f);
            gradient.Add(1.0f, 9.0f);
            gradient.Add(0.5f, 1.0f);
            Assert.AreEqual(0.0f, gradient.GetAtPosition(0.05f));
            Assert.AreEqual(1.0f, gradient.GetAtPosition(0.35f));
            Assert.AreEqual(5.0f, gradient.GetAtPosition(0.75f));
            Assert.AreEqual(1.8f, gradient.GetAtPosition(0.55f), 0.00001f);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidType()
        {
            Gradient<object> gradient = new Gradient<object>();
        }

        [TestMethod]
        public void TestOutOfBounds()
        {
            Gradient<float> gradient = new Gradient<float>();
            gradient.Add(0.0f, 0.0f);
            gradient.Add(0.5f, 1.0f);
            float result = gradient.GetAtPosition(1.0f);
            float result2 = gradient.GetAtPosition(-0.5f);
            Assert.AreEqual(1.0f, result);
            Assert.AreEqual(0.0f, result2);
        }

        [TestMethod]
        public void TestExtrapolation()
        {
            Gradient<float> gradient = new Gradient<float>(Gradient<float>.Interpolation.LinearExtrapolate);
            gradient.Add(0.25f, 0.0f);
            gradient.Add(0.5f, 1.0f);
            float result = gradient.GetAtPosition(1.0f);
            float result2 = gradient.GetAtPosition(0.0f);
            float result3 = gradient.GetAtPosition(0.375f);
            Assert.AreEqual(3.0f, result);
            Assert.AreEqual(-1.0f, result2);
            Assert.AreEqual(0.5f, result3);
        }

        [TestMethod]
        public void TestCollectionInitializer()
        {
            Gradient<float> gradient = new Gradient<float>()
            {
                new Gradient<float>.GradientStop(0.0f, 0.0f),
                new Gradient<float>.GradientStop(1.0f, 1.0f)
            };
            float result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0.5f, result);
        }

        [TestMethod]
        public void TestIEnumerableConstructor()
        {
            Gradient<float>.GradientStop[] arr = new Gradient<float>.GradientStop[]
            {
                new Gradient<float>.GradientStop(0.0f, 0.0f),
                new Gradient<float>.GradientStop(1.0f, 1.0f)
            };
            Gradient<float> gradient = new Gradient<float>(arr);
            float result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0.5f, result);
        }

        [TestMethod]
        public void TestEnumerateStops()
        {
            Gradient<float> gradient = new Gradient<float>();
            gradient.Add(0.0f, 0.0f);
            gradient.Add(1.0f, 1.0f);
            Assert.AreEqual(0.0f, gradient.ElementAt(0).Position);
            Assert.AreEqual(0.0f, gradient.ElementAt(0).Stop);
            Assert.AreEqual(1.0f, gradient.ElementAt(1).Position);
            Assert.AreEqual(1.0f, gradient.ElementAt(1).Stop);
        }

        [TestMethod]
        public void TestRemove()
        {
            Gradient<float> gradient = new Gradient<float>();
            gradient.Add(0.0f, 0.0f);
            gradient.Add(0.5f, 16.0f);
            gradient.Add(1.0f, 1.0f);
            gradient.Remove(0.5f);
            float result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0.5f, result, 0.00001f);
        }

        [TestMethod]
        public void TestReplace()
        {
            Gradient<float> gradient = new Gradient<float>();
            gradient.Add(0.0f, 0.0f);
            gradient.Add(1.0f, 12.0f);
            gradient.ReplaceStop(1.0f, 1.0f);
            float result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(0.5f, result, 0.00001f);
        }

        [TestMethod]
        public void TestGetAtStop()
        {
            Gradient<float> gradient = new Gradient<float>();
            gradient.Add(0.0f, 0.0f);
            gradient.Add(0.5f, 1.0f);
            gradient.Add(1.0f, 0.0f);
            float result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(1.0f, result, 0.00001f);
        }

        [TestMethod]
        public void TestExtrapolateGetAtStop()
        {
            Gradient<float> gradient = new Gradient<float>(Gradient<float>.Interpolation.LinearExtrapolate);
            gradient.Add(0.0f, 0.0f);
            gradient.Add(0.5f, 1.0f);
            gradient.Add(1.0f, 0.0f);
            float result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(1.0f, result, 0.00001f);
        }

        [TestMethod]
        public void TestCosineGetAtStop()
        {
            Gradient<float> gradient = new Gradient<float>(Gradient<float>.Interpolation.Cosine);
            gradient.Add(0.0f, 0.0f);
            gradient.Add(0.5f, 1.0f);
            gradient.Add(1.0f, 0.0f);
            float result = gradient.GetAtPosition(0.5f);
            Assert.AreEqual(1.0f, result, 0.00001f);
        }
    }
}
