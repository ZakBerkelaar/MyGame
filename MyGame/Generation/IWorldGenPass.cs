using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Generation
{
    public interface IWorldGenPass
    {
        void Pass(World world, Random r, IWorldGenerator generator);
    }
}
