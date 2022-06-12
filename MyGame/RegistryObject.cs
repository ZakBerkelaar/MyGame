using MyGame.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public abstract class RegistryObject
    {
        public readonly IDString RegistryID;

        public RegistryObject()
        {
            RegistryID = ((RegistrableAttribute)Attribute.GetCustomAttribute(GetType(), typeof(RegistrableAttribute), false) ?? throw new Exception()).IDString;
        }
    }

    public static class RegistryObjectExtensions
    {
        public static IDString GetRegistryID<T>() where T : RegistryObject
        {
            return ((RegistrableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(RegistrableAttribute), false)).IDString;
        }

        public static IDString GetRegistryID(this Type type)
        {
            return ((RegistrableAttribute)Attribute.GetCustomAttribute(type, typeof(RegistrableAttribute), false)).IDString;
        }
    }
}
