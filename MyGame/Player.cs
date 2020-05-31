using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace MyGame
{
    public class Player : Entity
    {
        private float fallMultiplier = 2.5f;
        private float lowJumpMultiplier = 2f;

        public Player()
        {
            size = new Vector2(32, 64);
            type = Entities.Player;
        }

        protected override void Update()
        {
            if (isRemote)
                return;

            if (velocity.x != 0)
            {
                //TODO: Not using Game.deltaTime
                velocity.x /= 2f;
            }

            if(velocity.y < 0)
            {
                velocity += Vector2.up * (-20f) * (fallMultiplier - 1) * world.deltaTime;
            }
            else if (velocity.y > 0 && !Input.GetKey(Key.W))
            {
                velocity += Vector2.up * (-20f) * (lowJumpMultiplier - 1) * world.deltaTime;
            }

            if (Input.GetKeyDown(Key.W))
                velocity.y = 25;

            if (Input.GetKey(Key.A))
                velocity.x = -10;
            if (Input.GetKey(Key.S))
                Console.WriteLine("Down");
            if (Input.GetKey(Key.D))
                velocity.x = 10;

            velocity.y -= 20 * world.deltaTime;
        }
    }
}
