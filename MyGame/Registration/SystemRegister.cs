using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Registration
{
    public static class SystemRegister
    {
        public static void RegisterSystems()
        {
            Registry2.RegisterSystem(typeof(Systems.DayCycleSystem));
        }
    }
}
