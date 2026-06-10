using Lunara2D.Graphics;

namespace Lunara2D.Core.Components
{
    public class LSpriteComponent : LComponent
    {
        public LTexture? Texture;

        public override void Render()
        {
            if (Texture == null) return;

            var transform = Entity.GetComponent<LTransformComponent>();
            if (transform == null) return;

            Texture.Draw(transform.X, transform.Y);
        }
    }
}