using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class IDString
    {
        public string Namespace { get; set; }
        public string Name { get; set; }

        public IDString(string @namespace, string name)
        {
            Namespace = @namespace;
            Name = name;
        }

        public IDString(string name)
        {
            if (name == null)
                throw new ArgumentException("Name must not be equal to null");
            else if (name == "")
                return;
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
        }

        public static implicit operator string(IDString iDString) => iDString.Namespace + ":" + iDString.Name;

        public override string ToString()
        {
            return Namespace + ":" + Name;
        }
    }
}
