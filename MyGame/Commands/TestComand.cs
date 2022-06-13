using MyGame.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Commands
{
    [Registration.Registrable("MyGame", "Command", "CommandTest")]
    internal class TestComand : Command
    {
        public override Side Side => Side.Client;

        public TestComand() : base("test", new CommardArgumentTypes.StringArgType())
        {

        }

        protected override CommandResult Execute(params CommandArgParseResult[] args)
        {
            Logger.LogDebug((string)args[0].Result);
            return CommandResult.Success();
        }
    }

    [Registration.Registrable("MyGame", "Command", "CommandTest2")]
    internal class TestComand2 : Command
    {
        public override Side Side => Side.Server;

        public TestComand2() : base("test2", new CommardArgumentTypes.StringArgType())
        {

        }

        protected override CommandResult Execute(params CommandArgParseResult[] args)
        {
            Logger.LogDebug((string)args[0].Result);
            return CommandResult.Success();
        }
    }
}
