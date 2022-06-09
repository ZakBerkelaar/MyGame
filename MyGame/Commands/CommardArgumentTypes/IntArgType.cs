using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Commands.CommardArgumentTypes
{
    public class IntArgType : ICommandArgumentType
    {
        public string[] GetSuggestions(string start)
        {
            return Array.Empty<string>();
        }

        public CommandArgParseResult ParseArgument(string argument)
        {
            if (int.TryParse(argument, out int result))
                return CommandArgParseResult.Success(result);
            else
                return CommandArgParseResult.Failure($"Could not parse {argument} as int");
        }
    }
}
