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

        public ItemTile(Tile tile) : base(new IDString("Item", "Item" + tile.RegistryID.Name)) 
        {
            this.tile = tile;
        }

        public override void UseItem(Entity user, Vector2 position, ItemStack stack)
        {
            user.world.SetTile(new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y)), tile);
            base.UseItem(user, position, stack);
        }
    }
}
