using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.UI
{
    public class UICheckbox : UIElement
    {
        public bool Checked { get; set; }

        private static readonly Texture box;
        private static readonly Texture boxChecked;

        static UICheckbox()
        {
            box = new Texture(new IDString("UI", "Box"));
            boxChecked = new Texture(new IDString("UI", "BoxChecked"));
        }

        public UICheckbox()
        {
            width = box.Width;
            height = box.Height;
        }

        public UICheckbox(bool @checked)
        {
            Checked = @checked;
            width = box.Width;
            height = box.Height;
        }

        protected override void DrawSelf()
        {
            if (Checked)
                boxChecked.Draw(globalLeft, globalTop);
            else
                box.Draw(globalLeft, globalTop);
        }

        public override void MouseDown()
        {
            Checked = !Checked;
            base.MouseDown();
        }
    }
}
