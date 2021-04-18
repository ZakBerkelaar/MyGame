using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Rendering
{
    public class NetworkEntityRenderer : EntityRenderer
    {
        private readonly UpdateInterpolator<Vector2> interpolator;

        public NetworkEntityRenderer(Entity entity) : base(entity)
        {
            interpolator = new UpdateInterpolator<Vector2>(entity.position, 6);
        }

        public override void PosUpdated()
        {
            interpolator.UpdateCounter();
        }

        public override void CalculateRenderPos(float alpha)
        {
            renderPos = interpolator.GetAtPosition(alpha);
        }

        public void SetNewPosition(Vector2 position)
        {
            interpolator.PushBack(position);
        }
    }
}
