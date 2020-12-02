using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public abstract class Tile : ITextureable
    {
        public IDString RegistryString { get; set; }

        public string Texture => RegistryString.Name;

        //TODO: Require registry string in ctor
        public Tile()
        {
            
        }
    }

    //public enum Tiles : uint
    //{
    //    Dirt = 1U,
    //    Grass = 2U,
    //    Stone = 3U
    //}
}
