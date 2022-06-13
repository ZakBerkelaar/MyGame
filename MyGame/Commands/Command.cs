using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Networking;
using MyGame.Networking.Packets;

namespace MyGame.Commands
{
    public abstract class Command : RegistryObject
    {
        private class ArgManager
        {
            private ICommandArgumentType[] argTypes;

            public ArgManager(params ICommandArgumentType[] argumentTypes)
            {
                argTypes = argumentTypes;
            }

            public bool ValidateArgs(string[] args)
            {
                return argTypes.Zip(args, (argType, arg) => argType.ParseArgument(arg).Succeeded).All(s => s);
            }

            public CommandArgParseResult[] Parse(string[] args)
            {
                return argTypes.Zip(args, (argType, arg) => argType.ParseArgument(arg)).ToArray();
            }
        }

        private ArgManager argManager;

        public string Name { get; }
        public abstract Side Side { get; }
        protected abstract CommandResult Execute(params CommandArgParseResult[] args);

        public CommandResult Run(string[] args)
        {
            if(Game.Side == Side.Client)
            {
                if(Side == Side.Client)
                {
                    CommandArgParseResult[] argResults = argManager.Parse(args);
                    CommandArgParseResult[] fails = argResults.Where(r => r.Succeeded == false).ToArray();
                    if (fails.Length != 0)
                        return CommandResult.Failure(String.Join("\n", fails.Select(f => f.Message)));
                    else
                        return Execute(argResults);
                }
                else
                {
                    Game.networkerClient.SendMessage(new CommandPacket(this, args));
                    return CommandResult.Success();
                }
            }
            else
            {
                if(Side == Side.Server)
                {
                    CommandArgParseResult[] argResults = argManager.Parse(args);
                    CommandArgParseResult[] fails = argResults.Where(r => r.Succeeded == false).ToArray();
                    if (fails.Length != 0)
                        return CommandResult.Failure(String.Join("\n", fails.Select(f => f.Message)));
                    else
                        return Execute(argResults);
                }
                else
                {
                    throw new Exception("Cannot run a client side command from the server");
                }
            }
        }

        public Command(string name, params ICommandArgumentType[] argumentTypes)
        {
            Name = name;
            argManager = new ArgManager(argumentTypes);
        }


    }
}
