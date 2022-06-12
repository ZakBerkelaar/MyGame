using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Registration
{
    public static class CommandRegister
    {
        public static void RegisterCommands()
        {
            Registry2.RegisterCommand(new Commands.TestComand());
        }
    }
}
