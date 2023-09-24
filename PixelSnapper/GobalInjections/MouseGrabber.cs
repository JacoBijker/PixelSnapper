using System.Drawing;
using System.Runtime.InteropServices;

namespace PixelSnapper.GobalInjections
{
    public static class MouseGrabber
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        public static Point GetCursorPosition()
        {
            Point cursor = new Point();
            GetCursorPos(ref cursor);

            return cursor;
        }
    }
}
