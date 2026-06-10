using Lunara2D.DataManagement;
using Lunara2D.Graphics;
using System.Runtime.InteropServices;

namespace Lunara2D
{
    /// <summary>
    /// A static global class to access important instanced data all across the engine
    /// </summary>
    internal static class LGlobal
    {
        //public class LGlobalState
        //{
        //    public LGameWindow? _window;
        //    public LGameRenderer? _renderer;

        //    public void SetState(LGameWindow window, LGameRenderer renderer)
        //    {
        //        _window = window;
        //        _renderer = renderer;
        //    }
        //}

        //static LGlobalState? _state;
        private static LGameWindow? _window;
        private static LGameRenderer? _renderer;

        internal static void Init(LGameWindow win)
        {
            //LAssetManager.Init();
            _window = win;
            _renderer = win.GetWindowRenderer();
            //_state.SetState(win, win.GetWindowRenderer());
        }

        internal static LGameWindow GetWindowContext() => _window;
        internal static LGameRenderer GetWindowRendererContext() => _renderer;


        public class LGameConfig
        {
            public string? GameName;
            public int? Version;

            public LGameConfig Create(string name, int version)
            {
                return new LGameConfig { GameName = name, Version = version };
            }

            public void Load(string engineExeFile)
            {

            }
        }
    }
}
