using System;
using MyGame.AI;

namespace MyGame
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EntityInfoAttribute : Attribute
    {
        public string name;
        public int health;
        public Type aiType;
        public float sizeX;
        public float sizeY;
        public object[] aiParams;

        public EntityInfoAttribute(string name, int health, float sizeX, float sizeY, Type aiType, params object[] aiParams)
        {
            if (!aiType.IsSubclassOf(typeof(AIBase)))
                throw new Exception("AI type must extend from AIBase");

            this.name = name;
            this.health = health;
            this.aiType = aiType;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.aiParams = aiParams;
        }
    }
}
