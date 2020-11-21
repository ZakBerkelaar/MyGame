using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    //https://www.what-could-possibly-go-wrong.com/the-dispatcher-pattern/
    //TODO: Why singleton?
    class Dispatcher
    {
        private static Dispatcher instance;

        public static Dispatcher Instance
        {
            get
            {
                if (instance == null)
                    instance = new Dispatcher();
                return instance;
            }
        }

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
