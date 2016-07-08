using System.Diagnostics;
using Microsoft.Win32;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Enables or disables the autostart (with the OS) of the application.
    /// </summary>
    public static class AutoStart
    {
        private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private static readonly string VALUE_NAME;

        static AutoStart()
        {
            VALUE_NAME = Process.GetCurrentProcess().ProcessName;
        }

        /// <summary>
        /// Enables autostart value for the assembly.
        /// </summary>
        public static void EnableAutoStart()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.SetValue(VALUE_NAME, Program.ExeLocation);
        }

        /// <summary>
        /// Returns whether auto start is enabled.
        /// </summary>
        public static bool IsAutoStartEnabled
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION);
                if (key == null)
                    return false;

                string value = (string)key.GetValue(VALUE_NAME);
                if (value == null)
                    return false;
                return value == Program.ExeLocation;
            }
        }

        /// <summary>
        /// Disables autostart value for the assembly.
        /// </summary>
        public static void DisableSetAutoStart()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.DeleteValue(VALUE_NAME);
        }
    }
}
