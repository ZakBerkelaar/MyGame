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
        public UIImage(IDString texture)
        {
            this.texture = new Texture(texture);
            width = this.texture.Width;
            height = this.texture.Height;
        }

        protected override void DrawSelf()
        {
            texture.Draw(globalTop, globalLeft);
        }

        public override void MouseDown()
        {
            Console.WriteLine("Clicked me");
            base.MouseDown();
        }
    }
}
