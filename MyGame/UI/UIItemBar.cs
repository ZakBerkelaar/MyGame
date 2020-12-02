using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Rendering;

namespace MyGame.UI
{
    public class UIItemBar : UIElement
    {
        private ItemStack[] items;
        private ItemStackRenderer[] renderers;

        private static Texture boxTexture;

        static UIItemBar()
        {
            boxTexture = new Texture(@"UI\Box");
        }

        public UIItemBar(ItemStack[] items)
        {
            this.items = items;
            this.renderers = items.Select(i => i != null ? new ItemStackRenderer(i) : null).ToArray();
        }

        protected override void DrawSelf()
        {
            const int size = 3;
            int total = boxTexture.width * items.Length * size;
            for (int i = 0; i < items.Length; i++)
            {
                boxTexture.Draw(i * boxTexture.width * size + ((Game.window.Width / 2) - total / 2), Game.window.Height - (boxTexture.height * size), size, size, 0);
                renderers[i]?.Render(i * boxTexture.width * size + ((Game.window.Width / 2) - total / 2), Game.window.Height - (boxTexture.height * size));
            }
        }
    }
}
