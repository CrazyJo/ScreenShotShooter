using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HelpersLib;
using HotKeyLib;
using MainLib.AppSettings;

namespace MainLib
{
    public sealed class HotkeyManager : IDisposable
    {
        public delegate void HotkeyTriggerEventHandler(HotkeySettings hotkeySetting);

        public event HotkeyTriggerEventHandler HotkeyTrigger;

        private readonly Hotkey _hotkey;

        private readonly HotKeysConfig _hotKeysConfig;
        public List<HotkeySettings> Hotkeys { get; private set; }

        public HotkeyManager(HotKeysConfig hotKeysConfig)
        {
            _hotkey = new Hotkey();
            _hotkey.HotkeyPress += HotkeyHotkeyPress;
            Application.AddMessageFilter(_hotkey);

            _hotKeysConfig = hotKeysConfig;

            UpdateHotkeys(hotKeysConfig.Hotkeys);
        }

        public void UpdateHotkeys(List<HotkeySettings> hotkeys)
        {
            if (Hotkeys != null)
            {
                UnregisterAllHotkeys();
            }

            Hotkeys = hotkeys;

            RegisterAllHotkeys();
        }

        private void HotkeyHotkeyPress(ushort id, Keys key, Modifiers modifier)
        {
            if (!_hotKeysConfig.IgnoreHotkeys)
            {
                HotkeySettings hotkeySetting = Hotkeys.Find(x => x.HotkeyInfo.ID == id);

                if (hotkeySetting != null)
                {
                    OnHotkeyTrigger(hotkeySetting);
                }
            }
        }

        private void OnHotkeyTrigger(HotkeySettings hotkeySetting)
        {
            if (HotkeyTrigger != null)
            {
                HotkeyTrigger(hotkeySetting);
            }
        }

        public void RegisterHotkey(HotkeySettings hotkeySetting)
        {
            UnregisterHotkey(hotkeySetting, false);

            if (hotkeySetting.HotkeyInfo.Status != HotkeyStatus.Registered && hotkeySetting.HotkeyInfo.IsValidHotkey)
            {
                _hotkey.RegisterHotkey(hotkeySetting.HotkeyInfo);
            }

            if (!Hotkeys.Contains(hotkeySetting))
            {
                Hotkeys.Add(hotkeySetting);
            }
        }

        public void RegisterAllHotkeys()
        {
            foreach (HotkeySettings hotkeySetting in Hotkeys)
            {
                RegisterHotkey(hotkeySetting);
            }
        }

        public void UnregisterHotkey(HotkeySettings hotkeySetting, bool removeFromList = true)
        {
            if (hotkeySetting.HotkeyInfo.Status == HotkeyStatus.Registered)
            {
                _hotkey.UnregisterHotkey(hotkeySetting.HotkeyInfo);
            }

            if (removeFromList)
            {
                Hotkeys.Remove(hotkeySetting);
            }
        }

        public void UnregisterAllHotkeys(bool removeFromList = true, bool temporary = false)
        {
            if (Hotkeys != null)
            {
                foreach (HotkeySettings hotkeySetting in Hotkeys.ToArray())
                {
                    if (!temporary || (temporary && hotkeySetting.Job != CaptureType.None))
                    {
                        UnregisterHotkey(hotkeySetting, removeFromList);
                    }
                }
            }
        }

        public void ResetHotkeys()
        {
            UnregisterAllHotkeys();
            Hotkeys.AddRange(GetDefaultHotkeyList());
            RegisterAllHotkeys();
        }

        public static List<HotkeySettings> GetDefaultHotkeyList()
        {
            return new List<HotkeySettings>
            {
                new HotkeySettings(CaptureType.PrintScreen, Keys.PrintScreen),
                new HotkeySettings(CaptureType.ActiveWindow, Keys.Alt | Keys.PrintScreen),
                new HotkeySettings(CaptureType.Rectangle, Keys.Control | Keys.PrintScreen),
            };
        }

        public void Dispose()
        {
            UnregisterAllHotkeys(false);
        }
    }
}
