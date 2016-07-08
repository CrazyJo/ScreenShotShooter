using System;
using System.Drawing;
using HelpersLib.Native;

namespace ScreenCaptureLib
{
    public static class Screenshot
    {
        public static bool CaptureCursor = false;

        public static Image CaptureFullscreen()
        {
            //TODO: change
            Rectangle bounds = CaptureHelpers.GetScreenBounds();
            //Rectangle bounds = new Rectangle(481, 229, 600, 400);

            return CaptureRectangle(bounds);
        }

        public static Image CaptureRectangle(Rectangle rect)
        {
            return CaptureRectangleNative(rect, CaptureCursor);
        }

        public static Image CaptureRectangleNative(Rectangle rect, bool captureCursor = false)
        {
            return CaptureRectangleNative(NativeMethods.GetDesktopWindow(), rect, captureCursor);
        }

        public static Image CaptureRectangleNative(IntPtr handle, Rectangle rect, bool captureCursor = false)
        {
            if (rect.Width == 0 || rect.Height == 0)
            {
                return null;
            }

            IntPtr hdcSrc = NativeMethods.GetWindowDC(handle);
            IntPtr hdcDest = NativeMethods.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = NativeMethods.CreateCompatibleBitmap(hdcSrc, rect.Width, rect.Height);
            IntPtr hOld = NativeMethods.SelectObject(hdcDest, hBitmap);
            NativeMethods.BitBlt(hdcDest, 0, 0, rect.Width, rect.Height, hdcSrc, rect.X, rect.Y, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);

            NativeMethods.SelectObject(hdcDest, hOld);
            NativeMethods.DeleteDC(hdcDest);
            NativeMethods.ReleaseDC(handle, hdcSrc);
            Image img = Image.FromHbitmap(hBitmap);
            NativeMethods.DeleteObject(hBitmap);

            return img;
        }

        public static Image CaptureActiveWindow()
        {
            IntPtr handle = NativeMethods.GetForegroundWindow();

            return CaptureWindow(handle);
        }

        public static Image CaptureWindow(IntPtr handle)
        {
            if (handle.ToInt32() > 0)
            {
                Rectangle rect = CaptureHelpers.GetWindowRectangle(handle);

                return CaptureRectangle(rect);
            }

            return null;
        }

    }
}
