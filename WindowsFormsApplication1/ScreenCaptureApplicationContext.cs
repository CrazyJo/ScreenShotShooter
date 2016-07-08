using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApplication1.Properties;
using HelpersLib;
using MainLib;

namespace WindowsFormsApplication1
{
    sealed class ScreenCaptureApplicationContext : ApplicationContext
    {
        private NotifyIcon _nIcon;
        private ContextMenuStrip _contextMenu;
        private readonly ScreenShotEngine _shotEngine;

        public ScreenCaptureApplicationContext()
        {
            CreateContextMenu();
            CreateNotificationIcon();

            _shotEngine = Program.ScreenShotEngine;
            Program.HotkeyManager.HotkeyTrigger += HotkeyManager_HotkeyTrigger;
        }

        void CreateContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            foreach (TrayFn fn in Helpers.GetEnums<TrayFn>())
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(fn.GetLocalizedDescription())
                {
                    Image = GetTrayIcon(fn),

                    // Получает или задает значение, показывающее, изменяется ли автоматически размер изображения на ToolStripItem 
                    // для соответствия размерам контейнера.
                    ImageScaling = ToolStripItemImageScaling.None
                };

                tsmi.Click += GetTrayHandler(fn);
                contextMenu.Items.Add(tsmi);
            }

            _contextMenu = contextMenu;
        }

        void CreateNotificationIcon()
        {
            _nIcon = new NotifyIcon
            {
                Icon = Resources.AppIcon,

                //Text = @"Click here to take a screenshot. Right-click to open the menu.",
                Visible = true
            };

            //_nIcon.Click += _nIcon_Click;
            //_nIcon.BalloonTipClicked += nIcon_BalloonClick;

            if (_contextMenu != null)
                _nIcon.ContextMenuStrip = _contextMenu;

        }

        private EventHandler GetTrayHandler(TrayFn fn)
        {
            EventHandler handler = null;

            switch (fn)
            {
                case TrayFn.FullScreen:
                    handler += FullScreen_Click;
                    break;
                case TrayFn.ActiveWindow:
                    handler += ActiveWindow_Click;
                    break;
                case TrayFn.Region:
                    handler += Region_Click;
                    break;
                case TrayFn.Settings:
                    handler += Settings_Click;
                    break;
                case TrayFn.Exit:
                    handler += ExitMenuItem_Click;
                    break;
            }

            return handler;
        }

        private Image GetTrayIcon(TrayFn fn)
        {
            Image icon;

            switch (fn)
            {
                default:
                case TrayFn.FullScreen:
                    icon = Resources.layer_fullscreen;
                    break;
                case TrayFn.ActiveWindow:
                    icon = Resources.active_window;
                    break;
                case TrayFn.Region:
                    icon = Resources.layer_shape;
                    break;
                case TrayFn.Settings:
                    icon = Resources.settings;
                    break;
                case TrayFn.Exit:
                    icon = Resources.exit;
                    break;
            }

            return icon;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _nIcon.Dispose();
            _contextMenu.Dispose();
        }

        #region Event Handlers

        private void HotkeyManager_HotkeyTrigger(HotkeySettings hotkeySetting)
        {
            _shotEngine.ExecuteJob(hotkeySetting.Job);
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            new SettingsForm(Program.Settings).Show();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            ExitThread();
        }

        private void ActiveWindow_Click(object sender, EventArgs e)
        {
            _shotEngine.CaptureActiveWindow();
        }

        private void FullScreen_Click(object sender, EventArgs e)
        {
            _shotEngine.CaptureFullScreen();
        }

        private void Region_Click(object sender, EventArgs e)
        {
            _shotEngine.CaptureRegion();
        }

        #endregion

    }
}
