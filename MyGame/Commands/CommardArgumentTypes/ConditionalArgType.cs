using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Commands.CommardArgumentTypes
{
    internal class ConditionalArgType : ICommandArgumentType
    {
        private Func<bool> suggestCondition;
        private Func<bool> parseCondition;

        private ICommandArgumentType type1;
        private ICommandArgumentType type2;

        public ConditionalArgType(Func<bool> parseCondition, ICommandArgumentType type1, ICommandArgumentType type2)
        {
            this.parseCondition = parseCondition;
            this.suggestCondition = parseCondition;
            this.type1 = type1;
            this.type2 = type2;
        }

        public ConditionalArgType(Func<bool> suggestCondition, Func<bool> parseCondition, ICommandArgumentType type1, ICommandArgumentType type2)
        {
            this.suggestCondition = suggestCondition;
            this.parseCondition = parseCondition;
            this.type1 = type1;
            this.type2 = type2;
        }

        public string[] GetSuggestions(string start)
        {
            if (suggestCondition == null)
            {
                return Array.Empty<string>();
            }
            else
            {
                if(suggestCondition())
                    return type1.GetSuggestions(start);
                else
                    return type2.GetSuggestions(start);
            }
        }

        public CommandArgParseResult ParseArgument(string argument)
        {
            if(parseCondition())
                return type1.ParseArgument(argument);
            else
                return type2.ParseArgument(argument);
        }
    }
}
