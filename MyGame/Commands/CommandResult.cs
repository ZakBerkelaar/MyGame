using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Commands
{
    public class CommandResult
    {
        public readonly bool Succeeded;
        public readonly string Message;

        private CommandResult(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public static CommandResult Success() => new CommandResult(true, null);
        public static CommandResult Failure(string message) => new CommandResult(false, message);
    }
}
