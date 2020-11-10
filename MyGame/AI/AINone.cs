using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.AI
{
    public class AINone : AIBase
    {
        public AINone(Entity entity) : base(entity)
        {

        }

        public override void Update()
        {
            return;
        }
    }
}
