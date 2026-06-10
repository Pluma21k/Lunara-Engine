using Lunara2D.Common;
using Lunara2D.Core;
using Lunara2D.Core.Components;
using Lunara2D.DataManagement;
using Lunara2D.Graphics;

namespace Lunara2D.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var window = new LGameWindow(args);

            if (window.Init("Lunara2D Engine", 620, 550))
            {
                LScene scene = new();
                LSceneRegister.RegisterScene("Main Scene Level", scene);

                LEntity player = new("Player");
                var controller = player.AddComponent<LPlayerController>();
                var sprite = player.AddComponent<LSpriteComponent>();
                sprite.Texture = LAssetManager.GetTexture("brick_wall");

                window.Run();
            }
        }
    }
}