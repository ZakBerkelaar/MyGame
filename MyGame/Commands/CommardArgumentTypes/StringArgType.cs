using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Commands.CommardArgumentTypes
{
    public class StringArgType : ICommandArgumentType
    {
        public string[] GetSuggestions(string start)
        {
            return Array.Empty<string>();
        }

        public CommandArgParseResult ParseArgument(string argument)
        {
            return CommandArgParseResult.Success(argument);
        }
    }
}
