using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MyGame.Rendering
{
    public class WorldRenderer
    {
        private readonly World world;
        private readonly Dictionary<Type, RenderSystem> systems = new Dictionary<Type, RenderSystem>();
        private readonly List<ChunkRenderer> chunks = new List<ChunkRenderer>();

        public WorldRenderer(World world)
        {
            this.world = world;
            foreach (Chunk chunk in world.chunks)
            {
                ChunkRenderer renderer = new ChunkRenderer(chunk);
                renderer.UpdateVBO();
                chunks.Add(renderer);
            }
        }

        public void UpdateVBOs()
        {
            foreach (ChunkRenderer renderer in chunks)
            {
                renderer.UpdateVBO();
            }
        }

        public void AddRenderSystem(RenderSystem system)
        {
            foreach (FieldInfo field in system.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(f => f.GetCustomAttribute<Systems.SystemReferenceAttribute>() != null))
            {
                field.SetValue(system, world.GetSystem(field.FieldType));
            }
            system.world = world;
            systems.Add(system.GetType(), system);
        }

        public T GetRenderSystem<T>() where T : RenderSystem
        {
            return (T)systems[typeof(T)];
        }

        public void RenderLighting()
        {
            foreach (var system in systems)
            {
                system.Value.RenderLight();
            }
        }

        public void Render()
        {
            foreach (ChunkRenderer renderer in chunks)
            {
                renderer.Render();
            }
            foreach (var system in systems)
            {
                system.Value.RenderOther();
            }
        }
    }
}
