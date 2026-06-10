using System;
using Lunara2D.Common;
using Lunara2D.Core;
using Lunara2D.Core.Components;
using Lunara2D.DataManagement;
using SDL3;

namespace Lunara2D.Graphics
{
    public class LGameWindow : IDisposable
    {
        private IntPtr rawWindow = IntPtr.Zero;
        private IntPtr rawRenderer = IntPtr.Zero;
        private LGameRenderer? renderer;

        private ulong lastCounter;
        private readonly ulong frequency;

        public bool IsRunning { get; private set; }

        public LGameWindow(string[] args)
        {
            frequency = SDL.GetPerformanceFrequency();
        }

        public bool Init(string title, int width, int height)
        {
            if (string.IsNullOrEmpty(title) || !SDL.Init(SDL.InitFlags.Video))
                return false;

            SDL.CreateWindowAndRenderer(
                title,
                width,
                height,
                SDL.WindowFlags.OpenGL,
                out rawWindow,
                out rawRenderer);

            if (rawWindow == IntPtr.Zero || rawRenderer == IntPtr.Zero)
                return false;

            renderer = new(rawRenderer);

            LGlobal.Init(this);
            LAssetManager.Init();

            lastCounter = SDL.GetPerformanceCounter();

            IsRunning = true;
            return true;
        }

        public void Run()
        {
            while (IsRunning)
            {
                UpdateDeltaTime();
                PollEvents();
                UpdateLogic();
                RenderFrame();
            }
        }

        public LGameRenderer? GetWindowRenderer() => renderer;

        private void UpdateDeltaTime()
        {
            ulong currentCounter = SDL.GetPerformanceCounter();

            LTime.deltaTime =
                (float)(currentCounter - lastCounter) / frequency;

            lastCounter = currentCounter;
        }

        private void UpdateLogic()
        {
            LSceneRegister.ActiveScene?.Update();
        }

        private void RenderFrame()
        {
            renderer?.Clear(LColor.Clear);

            LSceneRegister.ActiveScene?.Render();

            renderer?.Present();
        }

        private void PollEvents()
        {
            while (SDL.PollEvent(out SDL.Event ev))
            {
                if (ev.Type == (uint)SDL.EventType.Quit)
                    IsRunning = false;

                if (ev.Type == (uint)SDL.EventType.KeyDown &&
                    ev.Key.Key == SDL.Keycode.Escape)
                    IsRunning = false;
            }
        }

        public void Dispose()
        {
            if (rawRenderer != IntPtr.Zero)
                SDL.DestroyRenderer(rawRenderer);

            if (rawWindow != IntPtr.Zero)
                SDL.DestroyWindow(rawWindow);

            LAssetManager.Dispose();
            LSceneRegister.Dispose();

            SDL.Quit();
        }
    }
}