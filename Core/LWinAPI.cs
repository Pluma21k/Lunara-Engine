using System.Runtime.InteropServices;

namespace Lunara2D.Core
{
    public static class LWinAPI
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        public static void ShowMessageBox(string title, string fmt)
        {
            MessageBox(IntPtr.Zero, title, fmt, 0);
        }
    }
}
