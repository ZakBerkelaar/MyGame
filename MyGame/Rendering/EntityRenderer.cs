using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace MyGame.Rendering
{
    public class EntityRenderer : IIDable
    {
        public uint ID { get; private set; }

        private Entity entity;

        private Vector2 prevPos;

        public Vector2 renderPos;

        private int VBO;

        private Texture entityTexture;

        public EntityRenderer(Entity entity)
        {
            this.entity = entity;
            this.ID = entity.ID;

            prevPos = entity.position;

            entityTexture = new Texture(entity.RegistryID);
        }

        ~EntityRenderer()
        {
            Game.activeWorld.dispatcher.Invoke(() => GL.DeleteBuffer(VBO));
        }

        public void UpdateVBO()
        {
            float[] vertices = new float[6 * 5];
            //TextureUV uv = TextureAtlas.GetTexturePos(entity.type);
            TextureUV uv = TextureAtlas.GetAtlasLocationNew(entity.RegistryID).uv;

            //Bottom left
            Vector2 v1 = RenderHelper.ScreenToNormal(new Vector2(0, 0));
            vertices[0] = v1.x;
            vertices[1] = v1.y;
            vertices[2] = 0;
            //Texture coords
            vertices[3] = uv.BL.x;
            vertices[4] = uv.BL.y;

            //Top left
            Vector2 v2 = RenderHelper.ScreenToNormal(new Vector2(0, entity.size.y));
            vertices[5] = v2.x;
            vertices[6] = v2.y;
            vertices[7] = 0;
            //Texture coords
            vertices[8] = uv.TL.x;
            vertices[9] = uv.TL.y;

            //Bottom right
            Vector2 v3 = RenderHelper.ScreenToNormal(new Vector2(entity.size.x, 0));
            vertices[10] = v3.x;
            vertices[11] = v3.y;
            vertices[12] = 0;
            //Texture coords
            vertices[13] = uv.BR.x;
            vertices[14] = uv.BR.y;

            //Top right
            Vector2 v4 = RenderHelper.ScreenToNormal(new Vector2(entity.size.x, entity.size.y));
            vertices[15] = v4.x;
            vertices[16] = v4.y;
            vertices[17] = 0;
            //Texture coords
            vertices[18] = uv.TR.x;
            vertices[19] = uv.TR.y;

            //Top left
            Vector2 v5 = RenderHelper.ScreenToNormal(new Vector2(0, entity.size.y));
            vertices[20] = v5.x;
            vertices[21] = v5.y;
            vertices[22] = 0;
            //Texture coords
            vertices[23] = uv.TL.x;
            vertices[24] = uv.TL.y;

            //Bottom right
            Vector2 v6 = RenderHelper.ScreenToNormal(new Vector2(entity.size.x, 0));
            vertices[25] = v6.x;
            vertices[26] = v6.y;
            vertices[27] = 0;
            //Texture coords
            vertices[28] = uv.BR.x;
            vertices[29] = uv.BR.y;

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
            //Vector2 final = RenderHelper.ScreenToNormal(new Vector2(((Game.window.Width / 2) + renderPos.x * 16) - entity.size.x / 2, ((Game.window.Height / 2) + renderPos.y * 16) - entity.size.y / 2) + -Game.playerRenderer.renderPos * 16);
            //final += Vector2.one;
            //Game.window.entityShader.SetVector2("pos", final);

            //GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            ////Pass vertex array to buffer
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            ////GL.EnableVertexAttribArray(0);
            ////Pass texture coords array to buffer
            //GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            ////GL.EnableVertexAttribArray(1);

            //GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            //Vector2 final = RenderHelper.ScreenToNormal(new Vector2(((Game.window.Width / 2) + renderPos.x * 16) - entity.size.x / 2, ((Game.window.Height / 2) + renderPos.y * 16) - entity.size.y / 2) + -Game.playerRenderer.renderPos * 16);

            //(Game.window.Width / 2) Centers the entity x wise
            //(renderPos.x * 16) Adds the entities position
            //(entity.size.x / 2) Removes half the entity
            Vector2 test2 = new Vector2((Game.window.Width / 2) + (renderPos.x * 16) - (entity.size.x / 2), (Game.window.Height / 2) - (renderPos.y * 16) - (entity.size.y / 2)) - (new Vector2(Game.playerRenderer.renderPos.x, -Game.playerRenderer.renderPos.y) * 16);

            entityTexture.Draw(test2.x, test2.y, 1, 1, 0);
        }
    }
}
