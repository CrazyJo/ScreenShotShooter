using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using MainLib;
using MainLib.AppSettings;

namespace WindowsFormsApplication1
{
    static class Program
    {
        private static string _exeLocation;
        private static string _appConfigFilePath;
        private static bool _restarting;

        public static string ExeLocation
        {
            get
            {
                if (_exeLocation != null)
                {
                    return _exeLocation;
                }
                _exeLocation = Assembly.GetExecutingAssembly().Location;

                return _exeLocation;
            }
        }
        public static string ApplicationConfigFilePath
        {
            get
            {
                if (_appConfigFilePath == null)
                {
                    _appConfigFilePath = Path.Combine(Path.GetDirectoryName(ExeLocation), "ApplicationConfig.json");
                    return _appConfigFilePath;
                }

                return _appConfigFilePath;
            }
        }
        public static HotkeyManager HotkeyManager { get; set; }
        public static ApplicationConfig Settings { get; set; }
        public static ScreenShotEngine ScreenShotEngine { get; set; }


        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
	    // 123
            using (var mutex = new Mutex(false, "Screen Shot Shooter-{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}"))
            {
                if (!mutex.WaitOne(TimeSpan.FromSeconds(3), false)) return;
                Run();
            }
        }

        private static void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Load();
            InitializeComponent();

            Application.Run(new ScreenCaptureApplicationContext());

            ApplicationClosing();
        }

        private static void ApplicationClosing()
        {
            HotkeyManager.Dispose();

            Save();

            if (_restarting)
            {
                Process.Start(Application.ExecutablePath);
            }
        }

        public static void Restart()
        {
            _restarting = true;
            Application.Exit();
        }

        private static void InitializeComponent()
        {
            LanguageHelper.ChangeLanguage(Settings.GeneralSettings.Language);
            ScreenShotEngine = ScreenShotEngine.GetScreenShotEngine(Settings.GeneralSettings, Settings.ImageSettings);
            HotkeyManager = new HotkeyManager(Settings.HotKeysConfig);
        }

        private static void Load()
        {
            Settings = ApplicationConfig.Load(ApplicationConfigFilePath);
        }

        private static void Save()
        {
            Settings.Save(ApplicationConfigFilePath);
        }
    }
}
