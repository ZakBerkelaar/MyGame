using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class ItemStack
    {
        public Item item;
        public int count;

        public ItemStack(Item item, int count)
        {
            this.item = item ?? throw new ArgumentNullException(nameof(item));
            this.count = count;
        }
    }
}
