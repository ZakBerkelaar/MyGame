using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public readonly struct IDString
    {
        public readonly string Namespace;
        public readonly string Name;
        public readonly string Type;

        public IDString(string @namespace, string type, string name)
        {
            Namespace = @namespace;
            Name = name;
            Type = type;
        }

        public IDString(string type, string name)
        {
            Namespace = "MyGame";
            Name = name;
            Type = type;
        }

        public IDString(string idString)
        {
            if (idString.Contains(':'))
            {
                var nameSplit = idString.Split(':');
                Namespace = nameSplit[0];
                var typeSplit = nameSplit[1].Split('/');
                Type = typeSplit[0];
                Name = typeSplit[1];
            }
            else
            {
                Namespace = "MyGame";
                var typeSplit = idString.Split('/');
                Type = typeSplit[0];
                Name = typeSplit[1];
            }
        }

        public static implicit operator string(IDString iDString) => iDString.ToString();

        
        public override string ToString()
        {
            return $"{Namespace}:{Type}/{Name}";
        }

        public override bool Equals(object obj)
        {
            return obj is IDString @string &&
                   Namespace == @string.Namespace &&
                   Name == @string.Name &&
                   Type == @string.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Namespace, Name, Type);
        }

        public static bool operator ==(IDString left, IDString right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IDString left, IDString right)
        {
            return !(left == right);
        }
    }
}
