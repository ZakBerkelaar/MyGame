using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MyGame.Networking;
using MyGame.Networking.Packets;

namespace MyGame.Systems
{
    public abstract class NetworkedWorldSystem : WorldSystem
    {
        public abstract int UpdateInterval { get; }
        private int counter = 0;

        public override void Update()
        {
            NetworkedUpdate();
            if (counter == UpdateInterval)
            {
                SystemUpdatePacket packet = new SystemUpdatePacket(world.worldID, this);
                Game.SendMessage(packet);
                counter = 0;
            }
            counter++;
        }

        public abstract void Serialize(NetOutgoingMessage msg);
        public abstract void Deserialize(NetIncomingMessage msg);
        protected abstract void NetworkedUpdate();
    }
}
