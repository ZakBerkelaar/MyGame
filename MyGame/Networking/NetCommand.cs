using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Networking
{
    public enum NetChannel : uint
    {
        Position,
        Tile,
        Init,
        System,
        Command
    }
}
