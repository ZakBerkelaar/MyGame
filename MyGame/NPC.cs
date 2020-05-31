using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace MyGame
{
    public class NPC : Entity
    {
        private int test;

        public NPC()
        {
            size = new Vector2(32, 64);
            type = Entities.Test;
        }

        protected override void Update()
        {
            if(test == 100)
            {
                test = 0;
                velocity.y = 20;
                Console.WriteLine("JUMP!!!!!!");
            }
            test++;

            velocity.y -= 20 * world.deltaTime;
        }
    }
}
