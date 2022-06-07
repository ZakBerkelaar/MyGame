using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Generation
{
    public class WorldGenerator : IEnumerable<IWorldGenPass>, IWorldGenerator
    {
        private readonly LinkedList<IWorldGenPass> passes;
        private readonly Dictionary<Type, IWorldGenerationData> genData;
        private readonly int seed;

        public WorldGenerator(int seed)
        {
            passes = new LinkedList<IWorldGenPass>();
            genData = new Dictionary<Type, IWorldGenerationData>();
            this.seed = seed;
        }

        public void Add(IWorldGenPass pass)
        {
            passes.AddLast(pass);
        }

        public void AddPassAfter(IWorldGenPass after, IWorldGenPass pass)
        {
            passes.AddAfter(passes.Find(after), pass);
        }

        public void AddPassBefore(IWorldGenPass before, IWorldGenPass pass)
        {
            passes.AddBefore(passes.Find(before), pass);
        }

        public void AppendPass(IWorldGenPass pass)
        {
            passes.AddLast(pass);
        }

        public World GenWorld()
        {
            Random r = new Random(seed);
            World world = new World(30, 3); //TOOD: Create generator for this

            var enumerator = passes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Pass(world, r, this);
            }
            enumerator.Dispose();

            return world;
        }

        public T GetData<T>() where T : class, IWorldGenerationData, new()
        {
            if(genData.TryGetValue(typeof(T), out var dataOut))
            {
                return (T)dataOut;
            }
            else
            {
                T data = new T();
                genData[typeof(T)] = data;
                return data;
            }
        }

        public IEnumerator<IWorldGenPass> GetEnumerator()
        {
            return ((IEnumerable<IWorldGenPass>)passes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)passes).GetEnumerator();
        }
    }
}
