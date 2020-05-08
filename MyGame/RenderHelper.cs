using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public static class RenderHelper
    {
        public static Vector2 ScreenToNormal(Vector2 point)
        {
            float newX = (point.x / (Game.window.Width / 2)) - 1;
            float newY = (point.y / (Game.window.Height / 2)) - 1;
            return new Vector2(newX, newY);
        }

        public static Vector2 NormalToScreen(Vector2 point)
        {
            float newX = (Game.window.Width * (point.x + 1)) / 2;
            float newY = (Game.window.Height * (point.y + 1)) / 2;
            return new Vector2(newX, newY);
        }
    }
}
