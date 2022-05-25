using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.UI
{
    public class UIImage : UIElement
    {
        private Texture texture;
        public UIImage(Texture texture)
        {
            this.texture = texture;
            width = this.texture.Width;
            height = this.texture.Height;
        }

        public UIImage(IDString texture) : this(new Texture(texture))
        {

        }

        protected override void DrawSelf()
        {
            texture.Draw(globalLeft, globalTop);
        }
    }
}
