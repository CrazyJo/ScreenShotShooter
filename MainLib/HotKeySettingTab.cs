using System;
using System.Windows.Forms;
using HotKeyLib;
using MainLib.AppSettings;

namespace MainLib
{
    public class HotKeySettingTab
    {
        private readonly Control _flpHotkeys;
        private readonly HotkeyManager _hotkeyManager;
        private readonly HotKeysConfig _hotKeysConfig;
        private readonly IUserControlFactory _factory;

        public bool IsHotKeysEnable
        {
            get { return !_hotKeysConfig.IgnoreHotkeys; }
            set { _hotKeysConfig.IgnoreHotkeys = !value; }
        }

        public HotKeySettingTab(Control flpHotkeys, HotkeyManager hotkeyManager, HotKeysConfig hotKeysConfig, IUserControlFactory factory)
        {
            _flpHotkeys = flpHotkeys;
            _hotkeyManager = hotkeyManager;
            _hotKeysConfig = hotKeysConfig;
            _factory = factory;
        }

        public void RestoreDefaultHotkeys()
        {
            _hotkeyManager.ResetHotkeys();
            AddControls();
        }

        public void AddControls()
        {
            _flpHotkeys.Controls.Clear();

            foreach (HotkeySettings hotkeySetting in _hotkeyManager.Hotkeys)
            {
                AddHotkeySelectionControl(hotkeySetting);
            }
        }

        private void AddHotkeySelectionControl(HotkeySettings hotkeySetting)
        {
            HKSControl control = _factory.CreateHKControl(hotkeySetting);

            control.Margin = new Padding(25, 0, 0, 2);
            control.HotkeyChanged += control_HotkeyChanged;

            _flpHotkeys.Controls.Add(control);
        }

        private void control_HotkeyChanged(object sender, EventArgs e)
        {
            HotkeySelectionControl control = (HotkeySelectionControl)sender;
            _hotkeyManager.RegisterHotkey(control.HotkeySetting);
        }
    }
}
