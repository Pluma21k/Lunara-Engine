using SDL3;

namespace Lunara2D.Graphics
{
    public struct LColor
    {
        public float r, g, b, a;

        public LColor(float R, float G, float B, float A)
        {
            r = R;
            g = G;
            b = B;
            a = A;
        }

        public static LColor White => new LColor(1f, 1f, 1f, 1f);
        public static LColor Black => new LColor(0f, 0f, 0f, 1f);
        public static LColor Red => new LColor(1f, 0f, 0f, 1f);
        public static LColor Green => new LColor(0f, 1f, 0f, 1f);
        public static LColor Blue => new LColor(0f, 0f, 1f, 1f);
        public static LColor Yellow => new LColor(1f, 1f, 0f, 1f);
        public static LColor Cyan => new LColor(0f, 1f, 1f, 1f);
        public static LColor Magenta => new LColor(1f, 0f, 1f, 1f);
        public static LColor Gray => new LColor(0.5f, 0.5f, 0.5f, 1f);
        public static LColor Clear => new LColor(0f, 0f, 0f, 0f); // Fully transparent
    }

    public struct LDrawCommand
    {
        public LDrawCmdType Type;
        public float X, Y, W, H;
        public LColor Color;
    }

    public enum LDrawCmdType
    {
        Text,
        Window,
        Clear,
        Rect,
        Line,
        Point
    }

    public class LGameRenderer
    {
        private IntPtr _renderer;
        private readonly Stack<LDrawCommand> drawCalls = new();

        public IntPtr Handle => _renderer;
        public int DrawCallCount => drawCalls.Count;

        public LGameRenderer(IntPtr renderer)
        {
            _renderer = renderer;
        }

        public void Clear(LColor color)
        {
            SDL.SetRenderDrawColor(_renderer, (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
            SDL.RenderClear(_renderer);
        }

        public void DrawRect(float x, float y, float w, float h, LColor color)
        {
            SDL.FRect rect = new SDL.FRect { X = x, Y = y, W = w, H = h };
            SDL.SetRenderDrawColor(_renderer, (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
            SDL.RenderRect(_renderer, in rect);

            drawCalls.Push(new LDrawCommand
            {
                Type = LDrawCmdType.Rect,
                X = x,
                Y = y,
                W = w,
                H = h,
                Color = color
            });
        }

        public void FillRect(float x, float y, float w, float h, LColor color)
        {
            SDL.FRect rect = new SDL.FRect { X = x, Y = y, W = w, H = h };
            SDL.SetRenderDrawColor(_renderer, (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
            SDL.RenderFillRect(_renderer, in rect);

            drawCalls.Push(new LDrawCommand
            {
                Type = LDrawCmdType.Rect,
                X = x,
                Y = y,
                W = w,
                H = h,
                Color = color
            });
        }

        public void DrawLine(float x1, float y1, float x2, float y2, LColor color)
        {
            SDL.SetRenderDrawColor(_renderer, (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
            SDL.RenderLine(_renderer, x1, y1, x2, y2);

            drawCalls.Push(new LDrawCommand
            {
                Type = LDrawCmdType.Line,
                X = x1,
                Y = y1,
                W = x2,
                H = y2,
                Color = color
            });
        }

        public void DrawPoint(float x, float y, LColor color)
        {
            SDL.SetRenderDrawColor(_renderer, (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
            SDL.RenderPoint(_renderer, x, y);

            drawCalls.Push(new LDrawCommand
            {
                Type = LDrawCmdType.Point,
                X = x,
                Y = y,
                W = 1,
                H = 1,
                Color = color
            });
        }

        public void DrawTexture(IntPtr texture, float sx, float sy, float sw, float sh, float dx, float dy, float dw, float dh)
        {
            if (texture == IntPtr.Zero) return;

            SDL.FRect srcRect = new SDL.FRect { X = sx, Y = sy, W = sw, H = sh };
            SDL.FRect dstRect = new SDL.FRect { X = dx, Y = dy, W = dw, H = dh };

            SDL.RenderTexture(_renderer, texture, in srcRect, in dstRect);

            drawCalls.Push(new LDrawCommand
            {
                Type = LDrawCmdType.Window,
                X = dx,
                Y = dy,
                W = dw,
                H = dh,
                Color = new LColor { r = 1, g = 1, b = 1, a = 1 }
            });
        }

        public void DrawTexture(IntPtr texture, float x, float y, float w, float h)
        {
            if (texture == IntPtr.Zero) return;

            SDL.FRect dstRect = new SDL.FRect { X = x, Y = y, W = w, H = h };
            SDL.RenderTexture(_renderer, texture, IntPtr.Zero, in dstRect);

            drawCalls.Push(new LDrawCommand
            {
                Type = LDrawCmdType.Window,
                X = x,
                Y = y,
                W = w,
                H = h,
                Color = new LColor { r = 1, g = 1, b = 1, a = 1 }
            });
        }

        public void DrawScaledPixelBuffer(uint[] buffer, int srcWidth, int srcHeight, int dstWidth, int dstHeight)
        {
            float scaleX = (float)dstWidth / srcWidth;
            float scaleY = (float)dstHeight / srcHeight;

            for (int y = 0; y < dstHeight; y++)
            {
                for (int x = 0; x < dstWidth; x++)
                {
                    // Find the original pixel coordinates
                    int srcX = (int)(x / scaleX);
                    int srcY = (int)(y / scaleY);

                    uint color = buffer[srcX + srcY * srcWidth];

                    // Extract components
                    byte a = (byte)((color >> 24) & 0xFF);
                    byte r = (byte)((color >> 16) & 0xFF);
                    byte g = (byte)((color >> 8) & 0xFF);
                    byte b = (byte)(color & 0xFF);

                    SDL.SetRenderDrawColor(_renderer, r, g, b, a);
                    SDL.RenderPoint(_renderer, x, y);
                }
            }
        }
        public void DrawPixelBuffer(uint[] buffer, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    uint color = buffer[x + y * width];
                    byte a = (byte)((color >> 24) & 0xFF);
                    byte r = (byte)((color >> 16) & 0xFF);
                    byte g = (byte)((color >> 8) & 0xFF);
                    byte b = (byte)(color & 0xFF);

                    SDL.SetRenderDrawColor(_renderer, r, g, b, a);
                    SDL.RenderPoint(_renderer, x, y);
                }
            }
        }

        public void Present()
        {
            SDL.RenderPresent(_renderer);
        }

        public void ResetDrawCalls()
        {
            drawCalls.Clear();
        }
    }
}