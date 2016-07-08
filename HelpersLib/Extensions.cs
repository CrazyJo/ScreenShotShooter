using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using HelpersLib.Properties;

namespace HelpersLib
{
    public static class Extensions
    {
        public static void DrawRectangleProper(this Graphics g, Pen pen, Rectangle rect)
        {
            if (pen.Width == 1)
            {
                rect = rect.SizeOffset(-1);
            }

            if (rect.Width > 0 && rect.Height > 0)
            {
                g.DrawRectangle(pen, rect);
            }
        }

        public static void ForceActivate(this Form form)
        {
            if (!form.Visible)
            {
                form.Show();
            }

            if (form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Normal;
            }

            form.BringToFront();
            form.Activate();
        }

        public static Rectangle SizeOffset(this Rectangle rect, int width, int height)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width + width, rect.Height + height);
        }

        public static Rectangle SizeOffset(this Rectangle rect, int offset)
        {
            return rect.SizeOffset(offset, offset);
        }

        public static ImageFormat ParseImageFormat(this string format)
        {
            switch (format.ToUpper())
            {
                case "PNG":
                    return ImageFormat.Png;
                case "JPEG":
                    return ImageFormat.Jpeg;
                case "BMP":
                    return ImageFormat.Bmp;
                case "TIFF":
                    return ImageFormat.Tiff;
                default:
                    throw new AggregateException("It does not match any of the formats");
            }
        }

        public static bool SaveAs(this Image img, string saveFolder, ImageFormat format)
        {
            if (img == null || saveFolder == null || format == null)
                throw new ArgumentException();

            string fileName = DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + "." + format.ToString().ToLower();
            string fullfileName = Path.Combine(saveFolder, fileName);

            img.Save(fullfileName, format);

            return true;
        }

        public static string GetLocalizedDescription(this Enum value)
        {
            return value.GetLocalizedDescription(Resources.ResourceManager);
        }

        public static string GetLocalizedDescription(this Enum value, ResourceManager resourceManager)
        {
            string resourceName = value.GetType().Name + "_" + value;
            string description = resourceManager.GetString(resourceName);

            if (string.IsNullOrEmpty(description))
            {
                description = value.GetDescription();
            }

            return description;
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }

            return value.ToString();
        }
    }
}
