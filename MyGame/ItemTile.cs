using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class ItemTile : Item
    {
        private Tile tile;

        public ItemTile(Tile tile) : base(new IDString("Item" + tile.RegistryString.Name)) 
        {
            this.tile = tile;
        }

        public override void UseItem(Entity user, Vector2 position)
        {
            base.UseItem(user, position);
        }
    }
}
