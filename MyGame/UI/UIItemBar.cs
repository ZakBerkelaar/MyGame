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
        private Player player;
        private ItemStackRenderer[] renderers;

        private static Texture boxTexture;
        private static Texture selectedBoxTexture;

        static UIItemBar()
        {
            boxTexture = new Texture(@"UI\Box");
            selectedBoxTexture = new Texture(@"UI\BoxSelected");
        }

        public UIItemBar(Player player)
        {
            this.player = player;
            this.renderers = player.items.Select(i => i != null ? new ItemStackRenderer(i) : null).ToArray();
        }

        protected override void DrawSelf()
        {
            const int size = 3;
            int total = boxTexture.width * player.items.Length * size;
            for (int i = 0; i < player.items.Length; i++)
            {
                if (i == player.CurrentItem)
                    selectedBoxTexture.Draw(i * boxTexture.width * size + ((Game.window.Width / 2) - total / 2), Game.window.Height - (boxTexture.height * size), size, size, 0);
                else
                    boxTexture.Draw(i * boxTexture.width * size + ((Game.window.Width / 2) - total / 2), Game.window.Height - (boxTexture.height * size), size, size, 0);
                renderers[i]?.Render(i * boxTexture.width * size + ((Game.window.Width / 2) - total / 2), Game.window.Height - (boxTexture.height * size));
            }
        }
    }
}
