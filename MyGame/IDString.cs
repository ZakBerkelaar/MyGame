using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public struct IDString
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public IDString(string @namespace, string type, string name)
        {
            Namespace = @namespace;
            Name = name;
            Type = type;
        }

        public IDString(string type, string name)
        {
            if (name == null)
            {
                throw new ArgumentException("Name must not be null");
            }
            else if (name.Contains(':'))
            {
                var chars = name.Split(':');
                if (chars.Length != 2)
                    throw new ArgumentException("The namespace and name must be separated by a colon \':\' ");
                Namespace = chars[0];
                Name = chars[1];
            }
            else
            {
                Namespace = "MyGame";
                Name = name;
            }
            Type = type;
        }

        public static implicit operator string(IDString iDString) => iDString.ToString();

        public override string ToString()
        {
            return $"{Namespace}:{Type}/{Name}";
            return Namespace + ":" + Name;
        }
    }
}
