using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Commands
{
    public class CommandManager
    {
        private Dictionary<string, Command> commands;

        public CommandManager(Command[] commands)
        {
            this.commands = commands.ToDictionary(c => c.Name, c => c);
        }

        public CommandResult ExecuteCommand(string command)
        {
            string[] split = command.Split(' ');
            if (commands.ContainsKey(split[0]))
                return commands[split[0]].Run(split[1..]);
            else
                return CommandResult.Failure("Command not found");
        }
    }
}
