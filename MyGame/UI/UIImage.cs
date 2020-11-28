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
        public UIImage(string texture)
        {
            this.texture = new Texture(@"UI\" + texture);
            width = this.texture.width;
            height = this.texture.height;
        }

        protected override void DrawSelf()
        {
            texture.Draw(top, left);
        }

        public override void MouseDown()
        {
            Console.WriteLine("Clicked me");
            base.MouseDown();
        }
    }
}
