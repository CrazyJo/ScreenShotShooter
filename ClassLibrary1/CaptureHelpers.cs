using System;
using System.Drawing;
using System.Windows.Forms;
using HelpersLib;
using HelpersLib.Native;
using NativeMethods = HelpersLib.Native.NativeMethods;

namespace ScreenCaptureLib
{
    public static class CaptureHelpers
    {
        public static Rectangle GetScreenBounds()
        {
            return Screen.PrimaryScreen.Bounds;
        }

        public static Point GetCursorPosition()
        {
            POINT point;

            if (NativeMethods.GetCursorPos(out point))
            {
                return (Point)point;
            }

            return Point.Empty;
        }

        public static Rectangle CreateRectangle(int x, int y, int x2, int y2)
        {
            int width, height;

            if (x <= x2)
            {
                width = x2 - x + 1;
            }
            else
            {
                width = x - x2 + 1;
                x = x2;
            }

            if (y <= y2)
            {
                height = y2 - y + 1;
            }
            else
            {
                height = y - y2 + 1;
                y = y2;
            }

            return new Rectangle(x, y, width, height);
        }

        public static Rectangle GetWindowRectangle(IntPtr handle)
        {
            Rectangle rect = Rectangle.Empty;

            if (NativeMethods.IsDWMEnabled())
            {
                Rectangle tempRect;

                if (NativeMethods.GetExtendedFrameBounds(handle, out tempRect))
                {
                    rect = tempRect;
                }
            }

            if (rect.IsEmpty)
            {
                rect = NativeMethods.GetWindowRect(handle);
            }

            if (!Helpers.IsWindows10OrGreater() && NativeMethods.IsZoomed(handle))
            {
                rect = NativeMethods.MaximizedWindowFix(handle, rect);
            }

            return rect;
        }

    }
}
