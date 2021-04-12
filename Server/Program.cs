using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lidgren.Network;
using MyGame;
using MyGame.Networking;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using MyGame.Registration;

namespace Server
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                Server.Start();
            }
            else
            {
                try
                {
                    Server.Start();
                }
                catch (Exception e)
                {
                    Logger.LogError(e.ToString());
                    throw;
                }
            }
        }
    }
}