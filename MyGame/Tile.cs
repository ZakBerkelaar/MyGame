using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public abstract class Tile : ITextureable, IRegistrable
    {
        public IDString RegistryID { get; set; }

        public string Texture => RegistryID.Name;

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
