using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Networking;

namespace MyGame.Commands
{
    public abstract class Command : IRegistrable
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
        public IDString RegistryID { get; set; }
        public abstract Side Side { get; }
        protected abstract CommandResult Execute(params CommandArgParseResult[] args);

        public CommandResult Run(string[] args)
        {
            CommandArgParseResult[] argResults = argManager.Parse(args);
            CommandArgParseResult[] fails = argResults.Where(r => r.Succeeded == false).ToArray();
            if (fails.Length != 0)
                return CommandResult.Failure(String.Join("\n", fails.Select(f => f.Message)));
            else
                return Execute(argResults);
        }

        public Command(string name, params ICommandArgumentType[] argumentTypes)
        {
            Name = name;
            argManager = new ArgManager(argumentTypes);
        }


    }
}
