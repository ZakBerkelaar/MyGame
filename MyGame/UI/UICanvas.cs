using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.UI
{
    public class UICanvas : UIElement
    {
        public UICanvas()
        {
            top = 0;
            left = 0;
            width = Game.window.Width;
            height = Game.window.Height;
        }

        protected sealed override void DrawSelf()
        {
            return;
        }
    }
}
