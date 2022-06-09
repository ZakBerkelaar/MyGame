using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Commands
{
    public class CommandArgParseResult
    {
        public readonly bool Succeeded;
        public readonly string Message;
        public readonly object Result;

        private CommandArgParseResult(bool succeeded, string message, object result)
        {
            Succeeded = succeeded;
            Message = message;
            Result = result;
        }

        public static CommandArgParseResult Success(object result) => new CommandArgParseResult(true, null, result);
        public static CommandArgParseResult Failure(string message) => new CommandArgParseResult(false, message, null);
    }
}
