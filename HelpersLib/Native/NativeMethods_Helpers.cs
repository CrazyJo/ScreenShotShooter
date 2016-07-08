using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace HelpersLib.Native
{
    public static partial class NativeMethods
    {
        public static Rectangle MaximizedWindowFix(IntPtr handle, Rectangle windowRect)
        {
            Size size;

            if (GetBorderSize(handle, out size))
            {
                windowRect = new Rectangle(windowRect.X + size.Width, windowRect.Y + size.Height, windowRect.Width - (size.Width * 2), windowRect.Height - (size.Height * 2));
            }

            return windowRect;
        }

        public static bool GetBorderSize(IntPtr handle, out Size size)
        {
            WINDOWINFO wi = new WINDOWINFO();

            bool result = NativeMethods.GetWindowInfo(handle, ref wi);

            if (result)
            {
                size = new Size((int)wi.cxWindowBorders, (int)wi.cyWindowBorders);
            }
            else
            {
                size = Size.Empty;
            }

            return result;
        }

        public static bool IsDWMEnabled()
        {
            return Helpers.IsWindowsVistaOrGreater() && NativeMethods.DwmIsCompositionEnabled();
        }

        public static bool GetExtendedFrameBounds(IntPtr handle, out Rectangle rectangle)
        {
            RECT rect;
            int result = NativeMethods.DwmGetWindowAttribute(handle, (int)DwmWindowAttribute.ExtendedFrameBounds, out rect, Marshal.SizeOf(typeof(RECT)));
            rectangle = rect;
            return result == 0;
        }

        public static Rectangle GetWindowRect(IntPtr handle)
        {
            RECT rect;
            NativeMethods.GetWindowRect(handle, out rect);
            return rect;
        }
    }
}
