using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class IDHolder<T> : IEnumerable<T> where T : class, IIDable
    {
        private List<T> entities;

        public IDHolder()
        {
            entities = new List<T>();
        }

        public virtual T this[uint ID]
        {
            get
            {
                foreach (T entity in entities)
                {
                    if (entity.ID == ID)
                        return entity;
                }
                return null;
            }
        }

        public int Count => entities.Count;

        public virtual void Add(T entity)
        {
            entities.Add(entity);
        }

        public virtual bool Remove(uint ID)
        {
            foreach (T entity in entities)
            {
                if (entity.ID == ID)
                {
                    return entities.Remove(entity);
                }
            }
            return false;
        }

        public bool ContainsID(uint ID)
        {
            foreach (T entity in entities)
            {
                if (entity.ID == ID)
                    return true;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return entities.GetEnumerator();
        }
    }
}
