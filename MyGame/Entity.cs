using System;
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
        private int VBO;

        public Vector2 position;
        public Vector2 velocity;

        public Vector2 size;

        protected Entity()
        {
            VBO = GL.GenBuffer();

            /*Game.window.RenderFrame += FrameInternal;
            Game.window.UpdateFrame += UpdateInternal;*/
        }

        ~Entity()
        {
            //GL.DeleteBuffer(VBO); //TODO: Must be ran on main thread not the GC finializer thread!!!!!!
            Dispatcher.Instance.Invoke(() => GL.DeleteBuffer(VBO));
        }

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

        //TODO: Fix protection on method
        public void GenerateVBO()
        {
            float[] vertices = new float[6 * 5];

            //Bottom left
            Vector2 v1 = RenderHelper.ScreenToNormal(new Vector2(0, 0));
            vertices[0] = v1.x;
            vertices[1] = v1.y;
            vertices[2] = 0;
            //Texture coords
            vertices[3] = 0;
            vertices[4] = 1;

            //Top left
            Vector2 v2 = RenderHelper.ScreenToNormal(new Vector2(0, size.y));
            vertices[5] = v2.x;
            vertices[6] = v2.y;
            vertices[7] = 0;
            //Texture coords
            vertices[8] = 0;
            vertices[9] = 0;

            //Bottom right
            Vector2 v3 = RenderHelper.ScreenToNormal(new Vector2(size.x, 0));
            vertices[10] = v3.x;
            vertices[11] = v3.y;
            vertices[12] = 0;
            //Texture coords
            vertices[13] = 1;
            vertices[14] = 1;

            //Top right
            Vector2 v4 = RenderHelper.ScreenToNormal(new Vector2(size.x, size.y));
            vertices[15] = v4.x;
            vertices[16] = v4.y;
            vertices[17] = 0;
            //Texture coords
            vertices[18] = 1;
            vertices[19] = 0;

            //Top left
            Vector2 v5 = RenderHelper.ScreenToNormal(new Vector2(0, size.y));
            vertices[20] = v5.x;
            vertices[21] = v5.y;
            vertices[22] = 0;
            //Texture coords
            vertices[23] = 0;
            vertices[24] = 0;

            //Bottom right
            Vector2 v6 = RenderHelper.ScreenToNormal(new Vector2(size.x, 0));
            vertices[25] = v6.x;
            vertices[26] = v6.y;
            vertices[27] = 0;
            //Texture coords
            vertices[28] = 1;
            vertices[29] = 1;

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }

        public virtual void Render()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            //Pass vertex array to buffer
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            //Pass texture coords array to buffer
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
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
