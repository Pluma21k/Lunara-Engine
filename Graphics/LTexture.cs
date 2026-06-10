using Lunara2D.Common;
using Lunara2D.DataManagement;
using SDL3;

namespace Lunara2D.Graphics
{
    public class LTexture : IDisposable
    {
        private protected IntPtr rendererHandle => LGlobal.GetWindowRendererContext().Handle;

        public IntPtr Handle { get; private set; }

        public float Width { get; private set; }
        public float Height { get; private set; }

        public uint[] Pixels { get; private set; }

        public int PixelWidth => (int)Width;
        public int PixelHeight => (int)Height;

        public LTexture(string name)
        {
            string path = Path.Combine(LAssetManager.TexturesFolderPath, name + ".png");

            IntPtr surface = SDL.LoadPNG(path);

            if (surface == IntPtr.Zero)
                LDebug.LogError($"Failed to load texture: {name}");

            unsafe
            {
                SDL.Surface* s = (SDL.Surface*)surface;

                Width = s->Width;
                Height = s->Height;

                int count = (int)(Width * Height);
                Pixels = new uint[count];

                byte* ptr = (byte*)s->Pixels;
                int i = 0;

                for (int y = 0; y < Height; y++)
                {
                    byte* row = ptr + y * s->Pitch;

                    for (int x = 0; x < Width; x++)
                    {
                        byte b = row[x * 4 + 0];
                        byte g = row[x * 4 + 1];
                        byte r = row[x * 4 + 2];
                        byte a = row[x * 4 + 3];

                        Pixels[i++] =
                            ((uint)a << 24) |
                            ((uint)r << 16) |
                            ((uint)g << 8) |
                            (uint)b;
                    }
                }
            }

            Handle = SDL.CreateTextureFromSurface(rendererHandle, surface);

            SDL.DestroySurface(surface);
        }

        public void Draw(float x, float y)
        {
            SDL.FRect dst = new SDL.FRect
            {
                X = x,
                Y = y,
                W = Width,
                H = Height
            };

            SDL.RenderTexture(rendererHandle, Handle, IntPtr.Zero, dst);
        }

        public void Draw(float x, float y, float width, float height)
        {
            SDL.FRect dst = new SDL.FRect
            {
                X = x,
                Y = y,
                W = width,
                H = height
            };

            SDL.RenderTexture(rendererHandle, Handle, IntPtr.Zero, dst);
        }

        public void Dispose()
        {
            if (Handle != IntPtr.Zero)
            {
                SDL.DestroyTexture(Handle);
                Handle = IntPtr.Zero;
            }
        }
    }
}