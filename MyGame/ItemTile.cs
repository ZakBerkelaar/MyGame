using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    [Registration.Registrable("MyGame", "Item", "ItemTile")]
    public sealed class ItemTile : Item
    {
        private readonly Tile tile;

        public ItemTile(Tile tile)
        {
            this.tile = tile;
            TextureOverride = new IDString("MyGame", "Item", "Item" + tile.RegistryID.Name);
        }

        public override void UseItem(Entity user, Vector2 position, ItemStack stack)
        {
            user.world.SetTile(new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y)), tile);
            base.UseItem(user, position, stack);
        }
    }
}
