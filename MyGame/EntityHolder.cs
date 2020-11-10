using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class EntityHolder : IDHolder<Entity>
    {
        public List<Player> players;

        public EntityHolder()
        {
            players = new List<Player>();
        }

        public override void Add(Entity entity)
        {
            if (entity is Player p)
                players.Add(p);
            base.Add(entity);
        }

        public override bool Remove(uint ID)
        {
            if (this[ID] is Player p)
                players.Remove(p);
            return base.Remove(ID);
        }
    }
}
