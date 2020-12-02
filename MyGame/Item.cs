using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public abstract class Item
    {
        public IDString RegistryString { get; private set; }

        public Item(IDString regStr)
        {
            RegistryString = regStr;
        }

        public virtual void UseItem(Entity user, Vector2 position) { }
    }
}
