using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.UI
{
    public class UIPanel : UIElement
    {
        private static Texture panelBackgroud;

        static UIPanel()
        {
            panelBackgroud = new Texture(new IDString("UI", "Panel"));
        }

        public UIPanel()
        {

        }

        protected override void DrawSelf()
        {
            panelBackgroud.Draw(globalLeft, globalTop, width / panelBackgroud.Width, height / panelBackgroud.Height);
            base.DrawSelf();
        }
    }
}
