using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Commands
{
    public interface ICommandArgumentType
    {
        public abstract string[] GetSuggestions(string start);
        public abstract CommandArgParseResult ParseArgument(string argument);
    }
}
