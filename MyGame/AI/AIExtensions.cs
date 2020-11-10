using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.AI
{
    internal static class AIExtensions
    {
        internal static void Jump(this NPC npc, float velocity)
        {
            npc.velocity.y = velocity;
        }
    }
}
