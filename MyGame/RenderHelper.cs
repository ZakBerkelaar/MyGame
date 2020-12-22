using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public static Vector2 ScreenToNormalNew(Vector2 point)
        {
            float newX = (point.x / (Game.window.Width / 2)) - 1;
            float newY = -(point.y / (Game.window.Height / 2)) + 1;
            return new Vector2(newX, newY);
        }

        public static Vector2 ScreenToWorld(Vector2 point)
        {
            float newX = Mathf.CeilToInt(((point.x - (Game.window.Width / 2f)) / 16f) + Game.activePlayer.position.x);
            float newY = Mathf.CeilToInt((((Game.window.Height - point.y) - (Game.window.Height / 2f)) / 16f) + 1 + Game.activePlayer.position.y);
            return new Vector2(newX, newY);
        }
    }
}
