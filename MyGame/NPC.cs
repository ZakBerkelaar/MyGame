using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace MyGame
{
    class NPC : Entity
    {
        public NPC()
        {
            size = new Vector2(32, 64);

            GenerateVBO();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(Key.P))
                velocity.y = 25;  

            velocity.y -= 20 * Game.deltaTime;
        }
    }
}
