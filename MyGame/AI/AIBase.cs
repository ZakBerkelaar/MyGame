using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MyGame.AI
{
    public abstract class AIBase
    {
        protected Entity entity;

        public AIBase(Entity entity)
        {
            this.entity = entity;
        }

        public abstract void Update();

        //private static Dictionary<AITypes, Type> aiTypesDictionary = new Dictionary<AITypes, Type>();

        //public static AIBase CreateAI(AITypes type, NPC npc)
        //{
        //    if(!aiTypesDictionary.ContainsKey(type))
        //    {
        //        Type aiClass = typeof(AIBase).Assembly.GetTypes().FirstOrDefault(x => x.GetCustomAttributes<AI>().Any(k => k.type == type));
        //        aiTypesDictionary.Add(type, aiClass);
        //    }
        //    return (AIBase)Activator.CreateInstance(aiTypesDictionary[type], npc);
        //}

        public static AIBase CreateAI(Type type, Entity entity, params object[] args)
        {
            var list = args.ToList();
            list.Insert(0, entity);
            return (AIBase)Activator.CreateInstance(type, list.ToArray());
        }
    }
}
