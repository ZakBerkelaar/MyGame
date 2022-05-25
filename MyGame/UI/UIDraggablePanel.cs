using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.UI
{
    public class UIDraggablePanel : UIPanel
    {
        private bool dragging = false;
        private Vector2Int mouseStart;
        private Vector2Int panelStart;

        protected override void DrawSelf()
        {
            if(dragging)
            {
                Vector2Int mouseNow = new Vector2Int((int)Game.window.MousePosition.X, (int)Game.window.MousePosition.Y);
                Vector2Int diff = mouseNow - mouseStart;
                left = panelStart.x + diff.x;
                top = panelStart.y + diff.y;
            }
            base.DrawSelf();
        }

        public override void MouseDown()
        {
            dragging = true;
            mouseStart = new Vector2Int((int)Game.window.MousePosition.X, (int)Game.window.MousePosition.Y);
            panelStart = new Vector2Int(left, top);
            base.MouseDown();
        }

        public override void MouseUp()
        {
            dragging=false;
            base.MouseUp();
        }
    }
}
