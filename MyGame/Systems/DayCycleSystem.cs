using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Systems
{
    [Registration.Registrable("MyGame", "System", "SystemDayCycle")]
    public class DayCycleSystem : NetworkedWorldSystem
    {
        public override int UpdateInterval => 30;

        public int time;
        public int dayLength;

        public override void InitialDataSerialize(NetOutgoingMessage msg)
        {
            msg.Write(dayLength);
        }

        public override void InitialDataDeserialize(NetIncomingMessage msg)
        {
            dayLength = msg.ReadInt32();
        }

        public override void Deserialize(NetIncomingMessage msg)
        {
            time = msg.ReadInt32();
        }

        public override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(time);
        }

        protected override void NetworkedUpdate()
        {
            time++;
        }
    }
}
