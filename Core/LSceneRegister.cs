using Lunara2D.Common;

namespace Lunara2D.Core
{
    public static class LSceneRegister
    {
        public static LScene? ActiveScene { get; private set; }
        private static readonly Dictionary<string, LScene> _scenes = new();

        public static void LoadScene(string name)
        {
            if (!_scenes.TryGetValue(name, out var scene))
            {
                LDebug.LogError($"Scene '{name}' not found.");
                return;
            }

            LDebug.LogWarn($"Loaded new scene '{name}'");
            ActiveScene?.Dispose();
            ActiveScene = scene;
        }

        /// <summary>
        /// If the scene dosent exist it creates new one
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scene"></param>
        public static void RegisterScene(string name, LScene scene)
        {
            if (_scenes.TryAdd(name, scene))
            {
                LoadScene(name);
                return;
            }

            LDebug.LogError($"Scene '{name}' already exists.");
        }

        public static void UnregisterScene(string name)
        {
            if (_scenes.Remove(name, out var scene))
            {
                if (ActiveScene == scene)
                {
                    ActiveScene = null;
                }
            }
        }

        internal static void Update()
        {
            if (ActiveScene != null)
            {
                ActiveScene.Update();
            }
        }

        internal static void Render()
        {
            if (ActiveScene != null)
            {
                ActiveScene.Render();
            }
        }

        internal static void Dispose()
        {
            ActiveScene?.Dispose();
            ActiveScene = null;

            _scenes.Clear();

        }
    }
}