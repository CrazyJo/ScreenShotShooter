using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;

namespace HelpersLib
{
    public static class Helpers
    {
        public static readonly Version OSVersion = Environment.OSVersion.Version;

        public static void PlaySoundAsync(Stream stream)
        {
            if (stream != null)
            {
                Task.Run(() =>
                {
                    using (stream)
                    using (SoundPlayer soundPlayer = new SoundPlayer(stream))
                    {
                        soundPlayer.PlaySync();
                    }
                });
            }
        }

        public static bool IsWindows10OrGreater()
        {
            return OSVersion.Major >= 10;
        }

        public static bool IsWindowsVistaOrGreater()
        {
            return OSVersion.Major >= 6;
        }

        public static string GetUniqueID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static Image CropImage(Image img, Rectangle rect)
        {
            if (img != null && rect.X >= 0 && rect.Y >= 0 && rect.Width > 0 && rect.Height > 0 && new Rectangle(0, 0, img.Width, img.Height).Contains(rect))
            {
                using (Bitmap bmp = new Bitmap(img))
                {
                    return bmp.Clone(rect, bmp.PixelFormat);
                }
            }

            return null;
        }

        public static T[] GetEnums<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static void SetDefaultUICulture(CultureInfo culture)
        {
            Type type = typeof(CultureInfo);

            try
            {
                // .NET 4.0
                type.InvokeMember("s_userDefaultUICulture", BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static, null, culture, new object[] { culture });
            }
            catch
            {
                try
                {
                    // .NET 2.0
                    type.InvokeMember("m_userDefaultUICulture", BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static, null, culture, new object[] { culture });
                }
                catch
                {
                    throw new InvalidOperationException("SetDefaultUICulture failed: " + culture.DisplayName);
                }
            }
        }

    }
}
