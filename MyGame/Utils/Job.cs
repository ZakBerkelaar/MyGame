using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Utils
{
    public class Job
    {
        public readonly float Interval;
        public readonly Action Action;

        public Job(float interval, Action action)
        {
            Interval = interval;
            Action = action;
        }
    }
}
