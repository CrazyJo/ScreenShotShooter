using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelpersLib;
using MainLib.AppSettings;
using MainLib.Properties;
using ScreenCaptureLib;

namespace MainLib
{

    public sealed class ScreenShotEngine
    {
        private delegate Image ScreenCaptureDelegate();

        private static ScreenShotEngine _screenShotEngine;
        private readonly GeneralSettings _generalSettings;
        private readonly ImageSettings _imageSettings;

        private ScreenShotEngine(GeneralSettings genSett, ImageSettings imgSett)
        {
            _generalSettings = genSett;
            _imageSettings = imgSett;

            //todo: описание создания HotkeyManager
            //HotkeyManager = new HotkeyManager();
            //HotkeyManager.HotkeyTrigger += _hotkeyManager_HotkeyTrigger;
            //HotkeyManager.UpdateHotkeys(applicationConfig.Hotkeys);
        }

        public static ScreenShotEngine GetScreenShotEngine(GeneralSettings genSett, ImageSettings imgSett)
        {

            LazyInitializer.EnsureInitialized(ref _screenShotEngine,
                () => new ScreenShotEngine(genSett, imgSett));

            return _screenShotEngine;
        }

        public void ExecuteJob(CaptureType captureType)
        {
            ExecuteJob(captureType, false);
        }

        private async void ExecuteJob(CaptureType captureType, bool withDelay)
        {
            if (withDelay)
                await Task.Delay(250);

            switch (captureType)
            {
                case CaptureType.PrintScreen:
                    DoCaptureWork(Screenshot.CaptureFullscreen);
                    break;
                case CaptureType.ActiveWindow:
                    DoCaptureWork(Screenshot.CaptureActiveWindow);
                    break;
                case CaptureType.Rectangle:
                    DoCaptureWork(CaptureRectangle);
                    break;
            }
        }

        private void DoCaptureWork(ScreenCaptureDelegate capture)
        {
            Image img = capture();
            AfterCapture(img);
        }

        private void AfterCapture(Image img)
        {
            if (_generalSettings.PlaySoundAfterPrintScreen)
                Helpers.PlaySoundAsync(Resources.ShutterSound);

            img.SaveAs(_imageSettings.PictureSaveFolder, _imageSettings.PictureFormat.ParseImageFormat());
        }

        private Image CaptureRectangle()
        {
            Image img = null;

            using (RectangleRegionLightForm rectangleLight = new RectangleRegionLightForm())
            {
                if (rectangleLight.ShowDialog() == DialogResult.OK)
                {
                    img = rectangleLight.GetAreaImage();
                }
            }

            return img;
        }

        public void CaptureFullScreen()
        {
            ExecuteJob(CaptureType.PrintScreen, true);
        }
        public void CaptureActiveWindow()
        {
            ExecuteJob(CaptureType.ActiveWindow, true);
        }
        public void CaptureRegion()
        {
            ExecuteJob(CaptureType.Rectangle, true);
        }
    }
}
