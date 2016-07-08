using System.Windows.Forms;
using HelpersLib;

namespace HotKeyLib
{
    public class HotkeySettings
    {
        public HotkeyInfo HotkeyInfo { get; set; }

        public CaptureType Job { get; set; }

        public HotkeySettings()
        {
            HotkeyInfo = new HotkeyInfo();
            Job = CaptureType.None;
        }

        public HotkeySettings(CaptureType job, Keys hotkey = Keys.None) : this()
        {
            Job = job;
            HotkeyInfo = new HotkeyInfo(hotkey);
        }

        public override string ToString()
        {
            if (HotkeyInfo != null)
            {
                return $"Hotkey: {HotkeyInfo}, Job: {Job}";
            }

            return "";
        }
    }
}
