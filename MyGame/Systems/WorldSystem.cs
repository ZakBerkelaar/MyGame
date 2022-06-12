using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Systems;
using Lidgren.Network;

namespace MyGame.Systems
{
    public abstract class WorldSystem : RegistryObject
    {
        public World world;

        public abstract void Update();

        public virtual void InitialDataSerialize(NetOutgoingMessage msg) { }
        public virtual void InitialDataDeserialize(NetIncomingMessage msg) { }
    }
}
