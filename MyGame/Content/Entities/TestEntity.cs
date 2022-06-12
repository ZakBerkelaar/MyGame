using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Content.Entities
{
    [Registration.Registrable("MyGame", "Entity", "TestEntity")]
    public class TestEntity : Entity
    {
        public TestEntity()
        {
            size.x = 32;
            size.y = 64;
        }

        protected override void Update()
        {
            Player closest = null;
            float smallestDistance = float.PositiveInfinity;
            foreach (Player player in world.entities.players)
            {
                float distance = Vector2.SquareDistance(position, player.position);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    closest = player;
                }
            }

            float side = (position - closest.position).x;

            if (side > 0f)
                velocity.x = -6;
            else
                velocity.x = 6;

            if (world.GetTile(new Vector2Int((int)(position.x + (size.x / 16)) + 1, (int)position.y)) != Registration.Tiles.Air ||
                world.GetTile(new Vector2Int((int)position.x - 1, (int)position.y)) != Registration.Tiles.Air)
            {
                velocity.y += 2;
            }

            velocity.y -= world.Gravity * world.deltaTime;

            base.Update();
        }
    }
}
