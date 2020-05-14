using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public enum NetCommand : byte
    {
        PlayerConnected,
        UpdatePosition,
        EntityList,
        SetTile,
        Chunk
    }

    public enum NetChannel : uint
    {
        Position,
        Tile
    }
}
