﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace MyGame
{
    public abstract class Entity
    {
        public uint ID;

        public Vector2 position;
        public Vector2 velocity;

        public Vector2 size;

        internal void FrameInternal(object sender, OpenTK.FrameEventArgs e)
        {
            Frame();
        }

        internal void UpdateInternal(object sender, OpenTK.FrameEventArgs e)
        {
            Update();

            UpdatePosition();
        }

        //TODO: Fix collision detection on edge of tiles
        private void UpdatePosition()
        {
            position += velocity * Game.deltaTime;

            //Collision
            //https://github.com/CodyHenrichsen-CTEC/Platformer_4_0/blob/master/Platformer/Platformer/Model/Player.cs
            //https://web.archive.org/web/20110919172052/http://create.msdn.com/en-US/education/catalog/sample/platformer

            int leftTile = Mathf.FloorToInt(position.x);
            int rightTile = Mathf.CeilToInt(position.x + (size.x / 16));
            int topTile = Mathf.CeilToInt(position.y + (size.y / 16));
            int bottomTile = Mathf.FloorToInt(position.y);

            //Console.WriteLine(string.Format("Position: {4}, Left: {0}, Right: {1}, Top: {2}, Bottom: {3}", leftTile, rightTile, topTile, bottomTile, position));

            for (int x = leftTile; x <= rightTile; x++)
            {
                for (int y = bottomTile; y <= topTile; y++)
                {
                    if (Game.activeWorld.GetTile(new Vector2Int(x, y)) != null)
                    {
                        Vector2 tile = new Vector2(x, y);
                        if (position.x < tile.x + 1 &&
                            position.x + (size.x / 16) > tile.x &&
                            position.y < tile.y + 1 &&
                            position.y + (size.y / 16) > tile.y)
                        {
                            //Console.WriteLine("COLLISION");

                            //Determine collision depth with direction
                            //TODO: Is this really the fastest way? (Should still be very fast but needs to be as called several times per frame per entity)
                            Vector2 depth = new Vector2(1 - (position.x - tile.x), 1 - (position.y - tile.y));
                            if (depth.x > 1)
                                depth += new Vector2(-(size.x / 16) - 1, 0);
                            if (depth.y > 1)
                                depth += new Vector2(0, -(size.y / 16) - 1);

                            //Resolve collision along smallest axis
                            float absDepthX = Math.Abs(depth.x);
                            float absDepthY = Math.Abs(depth.y);

                            if (absDepthY < absDepthX)
                            {
                                position = new Vector2(position.x, position.y + depth.y);
                                velocity.y = 0;
                            }
                            else
                            {
                                position = new Vector2(position.x + depth.x, position.y);
                                velocity.x = 0;
                            }

                            //Console.WriteLine(depth);
                        }
                    }
                }
            }
        }

        protected virtual void Frame() { }
        protected virtual void Update() { }
    }

    public enum Entities
    {
        Player,
        NPC,
        Enemy
    }
}
