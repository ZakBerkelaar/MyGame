using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class EntityHolder : IEnumerable<Entity>
    {
        private List<Entity> entities;

        public EntityHolder()
        {
            entities = new List<Entity>();
        }

        public Entity this[uint ID]
        {
            get
            {
                foreach (Entity entity in entities)
                {
                    if (entity.ID == ID)
                        return entity;
                }
                return null;
            }
        }

        public void Add(Entity entity)
        {
            entities.Add(entity);
        }

        public bool ContainsID(uint ID)
        {
            foreach (Entity entity in entities)
            {
                if (entity.ID == ID)
                    return true;
            }
            return false;
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return entities.GetEnumerator();
        }
    }
}
