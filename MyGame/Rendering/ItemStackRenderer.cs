using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Rendering
{
    public class ItemStackRenderer
    {
        private ItemStack item;
        private Texture itemTexture;

        public ItemStackRenderer(ItemStack stack)
        {
            this.item = stack;
            this.itemTexture = new Texture(stack.item.RegistryID);
        }

        public void Render(int x, int y)
        {
            itemTexture.Draw(x, y);
        }
    }
}
