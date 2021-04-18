using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Systems
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SystemReferenceAttribute : Attribute
    {
    }
}
