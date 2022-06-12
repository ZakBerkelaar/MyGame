using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public abstract class Item : RegistryObject
    {
        public Item()
        {
            
        }

        public virtual void UseItem(Entity user, Vector2 position, ItemStack stack) { }
    }
}
