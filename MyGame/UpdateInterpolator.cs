using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class UpdateInterpolator<T>
    {
        private readonly Gradient<T> gradient;
        private readonly int interval;
        private int intervalCounter;

        public UpdateInterpolator(T initial, int interval)
        {
            gradient = new Gradient<T>(Gradient<T>.Interpolation.LinearExtrapolate);
            gradient.Add(0.0f, initial);
            gradient.Add(1.0f, initial);

            this.interval = interval;
        }

        public void PushBack(T element)
        {
            gradient.ReplaceStop(0.0f, gradient.GetAtPosition(1.0f));
            gradient.ReplaceStop(1.0f, element);

            intervalCounter = 0;
        }

        public T GetAtPosition(float frameAlpha)
        {
            float updateAlpha = (float)intervalCounter / interval;
            updateAlpha += (frameAlpha * (1.0f / 30.0f));

            return gradient.GetAtPosition(updateAlpha);
        }

        public void UpdateCounter()
        {
            intervalCounter++;
        }
    }
}
