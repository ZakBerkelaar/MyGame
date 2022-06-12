using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Content.Entities;

namespace MyGame.Registration
{
    public static class EntityRegister
    {
        public static void RegisterEntities()
        {
            Registry2.RegisterEntity(typeof(Player));
            Registry2.RegisterEntity(typeof(TestEntity));
        }
    }
}
