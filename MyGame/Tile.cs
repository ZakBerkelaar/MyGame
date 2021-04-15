using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public abstract class Tile : IRegistrable
    {
        public IDString RegistryID { get; set; }

        protected Tile()
        {
            RegistryID = new IDString("Tile", GetType().Name);
        }
    }

    //public enum Tiles : uint
    //{
    //    Dirt = 1U,
    //    Grass = 2U,
    //    Stone = 3U
    //}
}
