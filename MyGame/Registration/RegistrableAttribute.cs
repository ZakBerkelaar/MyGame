using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Registration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegistrableAttribute : Attribute
    {
        public IDString IDString { get; }

        public RegistrableAttribute(string @namespace, string type, string name)
        {
            IDString = new IDString(@namespace, type, name);
        }
    }
}
