using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Generation
{
    public interface IWorldGenerator
    {
        void AddPassAfter(IWorldGenPass after, IWorldGenPass pass);
        void AddPassBefore(IWorldGenPass before, IWorldGenPass pass);
        void AppendPass(IWorldGenPass pass);

        T GetData<T>() where T : class, IWorldGenerationData, new();
    }
}
