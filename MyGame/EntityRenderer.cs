using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace MyGame
{
    class EntityRenderer
    {
        private Entity entity;

        private Vector2 prevPos;

        public Vector2 renderPos;

        private int VBO;

        public EntityRenderer(Entity entity)
        {
            this.entity = entity;
            prevPos = entity.position;

            VBO = GL.GenBuffer();
            UpdateVBO();
        }

        ~EntityRenderer()
        {
            Dispatcher.Instance.Invoke(() => GL.DeleteBuffer(VBO));
        }

        public void UpdateVBO()
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
            Vector2 v2 = RenderHelper.ScreenToNormal(new Vector2(0, entity.size.y));
            vertices[5] = v2.x;
            vertices[6] = v2.y;
            vertices[7] = 0;
            //Texture coords
            vertices[8] = 0;
            vertices[9] = 0;

            //Bottom right
            Vector2 v3 = RenderHelper.ScreenToNormal(new Vector2(entity.size.x, 0));
            vertices[10] = v3.x;
            vertices[11] = v3.y;
            vertices[12] = 0;
            //Texture coords
            vertices[13] = 1;
            vertices[14] = 1;

            //Top right
            Vector2 v4 = RenderHelper.ScreenToNormal(new Vector2(entity.size.x, entity.size.y));
            vertices[15] = v4.x;
            vertices[16] = v4.y;
            vertices[17] = 0;
            //Texture coords
            vertices[18] = 1;
            vertices[19] = 0;

            //Top left
            Vector2 v5 = RenderHelper.ScreenToNormal(new Vector2(0, entity.size.y));
            vertices[20] = v5.x;
            vertices[21] = v5.y;
            vertices[22] = 0;
            //Texture coords
            vertices[23] = 0;
            vertices[24] = 0;

            //Bottom right
            Vector2 v6 = RenderHelper.ScreenToNormal(new Vector2(entity.size.x, 0));
            vertices[25] = v6.x;
            vertices[26] = v6.y;
            vertices[27] = 0;
            //Texture coords
            vertices[28] = 1;
            vertices[29] = 1;

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }

        public void PosUpdated()
        {
            prevPos = entity.position;
        }

        public void CalculateRenderPos(float alpha)
        {
            renderPos = entity.position * alpha + prevPos * (1f - alpha);
        }

        public void Render()
        {
            Vector2 final = RenderHelper.ScreenToNormal(new Vector2(((Game.window.Width / 2) + renderPos.x * 16) - entity.size.x / 2, ((Game.window.Height / 2) + renderPos.y * 16) - entity.size.y / 2) + -Game.playerRenderer.renderPos * 16);
            final += Vector2.one;
            Game.window.entityShader.SetVector2("pos", final);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            //Pass vertex array to buffer
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            //Pass texture coords array to buffer
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
    }
}
