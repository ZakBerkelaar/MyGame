global using TestType = System.Int32;

using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Commands;

namespace MyGame.Networking.Packets
{
    [Registration.Registrable("MyGame", "Packet", "PacketCommand")]
    public class CommandPacket : NetworkPacket
    {
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;

        public override NetChannel NetChannel => NetChannel.Command;

        public Command Command { get; private set; }
        public string[] CommandArgs { get; private set; }

        public CommandPacket()
        {

        }

        public CommandPacket(Command command, string[] commandArgs)
        {
            Command = command;
            CommandArgs = commandArgs;
        }

        protected override void Deserialize(NetIncomingMessage msg)
        {
            Command = Registration.Registry2.GetRegistryCommand(msg.ReadUInt16());
            CommandArgs = new string[msg.ReadInt32()];
            for (int i = 0; i < CommandArgs.Length; i++)
            {
                CommandArgs[i] = msg.ReadString();
            }
        }

        protected override void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(Registration.Registry2.GetRegistryCommandNetID(Command.RegistryID));
            msg.Write(CommandArgs.Length);
            for (int i = 0; i < CommandArgs.Length; i++)
            {
                msg.Write(CommandArgs[i]);
            }
        }
    }
}
