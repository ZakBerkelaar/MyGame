using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.AI
{
    public class AIJump : AIBase
    {
        public AIJump(NPC npc) : base(npc)
        {

        }

        private int test;

        public override void Update()
        {
            if(test == 100)
            {
                test = 0;
                entity.velocity.y = 20;
                Console.WriteLine("Jump!!!!!!!!!!!!");
            }
            test++;
        }
    }
}
