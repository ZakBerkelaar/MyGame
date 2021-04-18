using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Systems;

namespace MyGame.Rendering
{
    public abstract class RenderSystem
    {
        public World world;

        public virtual void RenderLight() { }
        public virtual void RenderOther() { }
    }
}
