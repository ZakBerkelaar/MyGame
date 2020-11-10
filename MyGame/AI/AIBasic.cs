using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.AI
{
    public class AIBasic : AIBase
    {
        public AIBasic(NPC npc) : base(npc)
        {

        }

        public override void Update()
        {
            //TODO: Probably does not need to update this every update cycle
            Player closest = null;
            float smallestDistance = float.PositiveInfinity;
            foreach (Player player in entity.world.entities.players)
            {
                float distance = Vector2.SquareDistance(entity.position, player.position);
                if(distance < smallestDistance)
                {
                    smallestDistance = distance;
                    closest = player;
                }
            }

            float side = (entity.position - closest.position).x;

            if (side > 0f)
                entity.velocity.x = -6;
            else
                entity.velocity.x = 6;

            if (entity.world.GetTile(new Vector2Int((int)(entity.position.x + (entity.size.x / 16)) + 1, (int)entity.position.y)) != null ||
                entity.world.GetTile(new Vector2Int((int)entity.position.x - 1, (int)entity.position.y)) != null)
            {
                entity.velocity.y += 2;
            }
        }
    }
}
