using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace MyGame
{
    public class Gradient<T> : IEnumerable<Gradient<T>.GradientStop>
    {
        public class GradientStop
        {
            public float Position;
            public T Stop;

            public GradientStop(float position, T stop)
            {
                Position = position;
                Stop = stop;
            }
        }

        public enum Interpolation
        {
            Linear,
            LinearExtrapolate,
            Cosine
        }

        private delegate T MultiplyDel(float val, T obj);
        private delegate T AddDel(T obj1, T obj2);

        private delegate T InterpolationDel(float position);

        private readonly SortedList<float, GradientStop> stops;
        private readonly Interpolation interpolation;

        private MultiplyDel multiply;
        private AddDel add;
        private InterpolationDel interpolate;

        public Gradient(Interpolation interpolation = Interpolation.Linear)
        {
            stops = new SortedList<float, GradientStop>();
            this.interpolation = interpolation;
            SetDelegates();
        }

        public Gradient(IEnumerable<GradientStop> stops, Interpolation interpolation = Interpolation.Linear)
        {
            this.stops = new SortedList<float, GradientStop>();
            foreach (var stop in stops)
            {
                this.stops.Add(stop.Position, stop);
            }
            this.interpolation = interpolation;
            SetDelegates();
        }

        public void Add(float position, T stop)
        {
            stops.Add(position, new GradientStop(position, stop));
        }

        public void Add(GradientStop stop)
        {
            stops.Add(stop.Position, stop);
        }

        public void AddStops(IEnumerable<GradientStop> stops)
        {
            foreach (var stop in stops)
            {
                this.stops.Add(stop.Position, stop);
            }
        }

        public void Remove(float position)
        {
            stops.Remove(position);
        }

        public void Remove(GradientStop stop)
        {
            stops.Remove(stop.Position);
        }

        public void ReplaceStop(float position, T newStop)
        {
            stops[position].Stop = newStop;
        }

        public void ReplaceStop(GradientStop oldStop, T newStop)
        {
            stops[oldStop.Position].Stop = newStop;
        }

        public T GetAtPosition(float position)
        {
            return interpolate(position);
        }

        private void SetDelegates()
        {
            Type type = typeof(T);
            if (type == typeof(int))
            {
                multiply = (MultiplyDel)Delegate.CreateDelegate(
                    typeof(MultiplyDel),
                    typeof(Gradient<T>).GetMethod("IntMul", BindingFlags.NonPublic | BindingFlags.Static), false);
                add = (AddDel)Delegate.CreateDelegate(
                    typeof(AddDel),
                    typeof(Gradient<T>).GetMethod("IntAdd", BindingFlags.NonPublic | BindingFlags.Static), false);
            }
            else if (type == typeof(uint))
            {
                multiply = (MultiplyDel)Delegate.CreateDelegate(
                    typeof(MultiplyDel),
                    typeof(Gradient<T>).GetMethod("UIntMul", BindingFlags.NonPublic | BindingFlags.Static), false);
                add = (AddDel)Delegate.CreateDelegate(
                    typeof(AddDel),
                    typeof(Gradient<T>).GetMethod("UIntAdd", BindingFlags.NonPublic | BindingFlags.Static), false);
            }
            else if (type == typeof(long))
            {
                multiply = (MultiplyDel)Delegate.CreateDelegate(
                    typeof(MultiplyDel),
                    typeof(Gradient<T>).GetMethod("LongMul", BindingFlags.NonPublic | BindingFlags.Static), false);
                add = (AddDel)Delegate.CreateDelegate(
                    typeof(AddDel),
                    typeof(Gradient<T>).GetMethod("LongAdd", BindingFlags.NonPublic | BindingFlags.Static), false);
            }
            else if (type == typeof(ulong))
            {
                multiply = (MultiplyDel)Delegate.CreateDelegate(
                    typeof(MultiplyDel),
                    typeof(Gradient<T>).GetMethod("ULongMul", BindingFlags.NonPublic | BindingFlags.Static), false);
                add = (AddDel)Delegate.CreateDelegate(
                    typeof(AddDel),
                    typeof(Gradient<T>).GetMethod("ULongAdd", BindingFlags.NonPublic | BindingFlags.Static), false);
            }
            else if (type == typeof(float))
            {
                multiply = (MultiplyDel)Delegate.CreateDelegate(
                    typeof(MultiplyDel),
                    typeof(Gradient<T>).GetMethod("FloatMul", BindingFlags.NonPublic | BindingFlags.Static), false);
                add = (AddDel)Delegate.CreateDelegate(
                    typeof(AddDel),
                    typeof(Gradient<T>).GetMethod("FloatAdd", BindingFlags.NonPublic | BindingFlags.Static), false);
            }
            else if (type == typeof(double))
            {
                multiply = (MultiplyDel)Delegate.CreateDelegate(
                    typeof(MultiplyDel),
                    typeof(Gradient<T>).GetMethod("DoubleMul", BindingFlags.NonPublic | BindingFlags.Static), false);
                add = (AddDel)Delegate.CreateDelegate(
                    typeof(AddDel),
                    typeof(Gradient<T>).GetMethod("DoubleAdd", BindingFlags.NonPublic | BindingFlags.Static), false);
            }
            else
            {
                try
                {
                    multiply = (MultiplyDel)Delegate.CreateDelegate(
                        typeof(MultiplyDel),
                        typeof(T).GetMethod("op_Multiply", new Type[] { typeof(float), typeof(T) }),
                        true);
                    add = (AddDel)Delegate.CreateDelegate(
                        typeof(AddDel),
                        typeof(T).GetMethod("op_Addition", new Type[] { typeof(T), typeof(T) }),
                        true);
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException("Unsupported generic type for gradient", e);
                }
            }
            switch (interpolation)
            {
                case Interpolation.Linear:
                    interpolate = LinearInterpolate;
                    break;
                case Interpolation.LinearExtrapolate:
                    interpolate = LinearExtrapolate;
                    break;
                case Interpolation.Cosine:
                    interpolate = CosineInterpolate;
                    break;
            }
        }

        public IEnumerator<GradientStop> GetEnumerator()
        {
            return stops.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)stops.Values).GetEnumerator();
        }

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable RCS1213 // Remove unused member declaration.
        #region MultiplicationMethods
        private static int IntMul(float coefficient, int val)
        {
            return (int)(coefficient * val);
        }

        private static uint UIntMul(float coefficient, uint val)
        {
            return (uint)(coefficient * val);
        }

        private static long LongMul(float coefficient, long val)
        {
            return (long)(coefficient * val);
        }

        private static ulong ULongMul(float coefficient, ulong val)
        {
            return (ulong)(coefficient * val);
        }

        private static float FloatMul(float coefficient, float val)
        {
            return (float)(coefficient * val);
        }


        private static double DoubleMul(float coefficient, double val)
        {
            return (double)(coefficient * val);
        }
        #endregion

        #region AdditionMethods
        private static int IntAdd(int obj1, int obj2)
        {
            return obj1 + obj2;
        }

        private static uint UIntAdd(uint obj1, uint obj2)
        {
            return obj1 + obj2;
        }

        private static long LongAdd(long obj1, long obj2)
        {
            return obj1 + obj2;
        }

        private static ulong ULongAdd(ulong obj1, ulong obj2)
        {
            return obj1 + obj2;
        }

        private static float FloatAdd(float obj1, float obj2)
        {
            return obj1 + obj2;
        }

        private static double DoubleAdd(double obj1, double obj2)
        {
            return obj1 + obj2;
        }
        #endregion
#pragma warning restore RCS1213 // Remove unused member declaration.
#pragma warning restore IDE0051 // Remove unused private members

        #region InterpolationMethods
        private T LinearInterpolate(float position)
        {
            GradientStop left = null, right = null;
            for (int i = 0; i < stops.Count - 1; i++)
            {
                if (stops.Values[i].Position < position && stops.Values[i + 1].Position > position)
                {
                    left = stops.Values[i];
                    right = stops.Values[i + 1];
                    goto exit;
                }
            }
            if (stops.Keys[stops.Count - 1] < position)
            {
                return stops.Values[stops.Count - 1].Stop;
            }
            else
            {
                if (stops.TryGetValue(position, out var val))
                    return val.Stop;
                return stops.Values[0].Stop;
            }
            exit:
            float dt = right.Position - left.Position;
            //float perRight = (position - left.Position) / dt;
            float perLeft = (right.Position - position) / dt;

            //return add(multiply(perLeft, left.Stop), multiply(perRight, right.Stop));
            return add(multiply(perLeft, left.Stop), multiply(1 - perLeft, right.Stop));
        }

        private T LinearExtrapolate(float position)
        {
            GradientStop left = null, right = null;
            for (int i = 0; i < stops.Count - 1; i++)
            {
                if (stops.Values[i].Position < position && stops.Values[i + 1].Position > position)
                {
                    left = stops.Values[i];
                    right = stops.Values[i + 1];
                    goto exit;
                }
            }
            if(stops.Keys[stops.Count - 1] < position)
            {
                left = stops.Values[stops.Count - 2];
                right = stops.Values[stops.Count - 1];
            }
            else
            {
                left = stops.Values[0];
                right = stops.Values[1];
            }
            exit:
            float dt = right.Position - left.Position;
            //float perRight = (position - left.Position) / dt;
            float perLeft = (right.Position - position) / dt;

            //return add(multiply(perLeft, left.Stop), multiply(perRight, right.Stop));
            return add(multiply(perLeft, left.Stop), multiply(1 - perLeft, right.Stop));
        }

        private T CosineInterpolate(float position)
        {
            GradientStop left = null, right = null;
            for (int i = 0; i < stops.Count - 1; i++)
            {
                if (stops.Values[i].Position < position && stops.Values[i + 1].Position > position)
                {
                    left = stops.Values[i];
                    right = stops.Values[i + 1];
                    goto exit;
                }
            }
            if (stops.TryGetValue(position, out var val))
                return val.Stop;
            exit:
            float dt = right.Position - left.Position;
            float perRight = (position - left.Position) / dt;
            //float perLeft = (right.Position - position) / dt;

            float mul = (1 - Mathf.Cos(perRight * Mathf.Pi)) * 0.5f;

            return add(multiply(1 - mul, left.Stop), multiply(mul, right.Stop));
        }
        #endregion
    }
}
