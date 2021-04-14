using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    //https://www.what-could-possibly-go-wrong.com/the-dispatcher-pattern/
    public class Dispatcher
    {
        public List<Action> pending = new List<Action>();

        public void Invoke(Action fn)
        {
            lock (pending)
            {
                pending.Add(fn);
            }
        }

        public void InvokePending()
        {
            lock (pending)
            {
                foreach (Action action in pending)
                {
                    action();
                }

                pending.Clear();
            }
        }
    }
}
