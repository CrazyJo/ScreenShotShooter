using System;
using System.Drawing;
using System.Windows.Forms;
using HotKeyLib;
using MainLib.AppSettings;
using MainLib.Properties;

namespace MainLib
{
    public partial class HotkeySelectionControl : HKSControl
    {
        //public event EventHandler HotkeyChanged;

        public static HotKeysConfig HotKeysConfig { get; set; }

        public HotkeySettings HotkeySetting { get; set; }

        public bool EditingHotkey { get; private set; }

        public HotkeySelectionControl(HotkeySettings hotkeySetting)
        {
            InitializeComponent();

            HotkeySetting = hotkeySetting;

            UpdateDescription();
            UpdateHotkeyText();
        }

        public void UpdateDescription()
        {
            //lblHotkeyDescription.Text = HotkeySetting.Job.ToString();
            lblHotkeyDescription.Text = HotkeySetting.JobDescription;
        }

        private void UpdateHotkeyText()
        {
            btnHotkey.Text = HotkeySetting.HotkeyInfo.ToString();
        }

        private void StartEditing()
        {
            EditingHotkey = true;

            HotKeysConfig.IgnoreHotkeys = true;

            btnHotkey.BackColor = Color.FromArgb(225, 255, 225);
            btnHotkey.Text = Resources.HotkeySelectionControl_Select_a_hotkey;

            HotkeySetting.HotkeyInfo.Hotkey = Keys.None;
            HotkeySetting.HotkeyInfo.Win = false;
            OnHotkeyChanged();
        }

        private void StopEditing()
        {
            EditingHotkey = false;

            HotKeysConfig.IgnoreHotkeys = false;

            if (HotkeySetting.HotkeyInfo.IsOnlyModifiers)
            {
                HotkeySetting.HotkeyInfo.Hotkey = Keys.None;
            }

            btnHotkey.BackColor = SystemColors.Control;
            btnHotkey.UseVisualStyleBackColor = true;

            OnHotkeyChanged();
            UpdateHotkeyText();
        }

        //protected void OnHotkeyChanged()
        //{
        //    HotkeyChanged?.Invoke(this, EventArgs.Empty);
        //}


        #region Event Handlers

        private void btnHotkey_MouseClick(object sender, MouseEventArgs e)
        {
            if (EditingHotkey)
            {
                StopEditing();
            }
            else
            {
                StartEditing();
            }
        }

        private void btnHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            if (EditingHotkey)
            {
                if (e.KeyData == Keys.Escape)
                {
                    HotkeySetting.HotkeyInfo.Hotkey = Keys.None;
                    StopEditing();
                }
                else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin)
                {
                    HotkeySetting.HotkeyInfo.Win = !HotkeySetting.HotkeyInfo.Win;
                    UpdateHotkeyText();
                }
                else if (new HotkeyInfo(e.KeyData).IsValidHotkey)
                {
                    HotkeySetting.HotkeyInfo.Hotkey = e.KeyData;
                    StopEditing();
                }
                else
                {
                    HotkeySetting.HotkeyInfo.Hotkey = e.KeyData;
                    UpdateHotkeyText();
                }
            }
        }

        private void btnHotkey_KeyUp(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            if (EditingHotkey)
            {
                // PrintScreen not trigger KeyDown event
                if (e.KeyCode == Keys.PrintScreen)
                {
                    HotkeySetting.HotkeyInfo.Hotkey = e.KeyData;
                    StopEditing();
                }
            }
        }

        private void btnHotkey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (EditingHotkey)
            {
                // For handle Tab key etc.
                e.IsInputKey = true;
            }
        }
        private void btnHotkey_Leave(object sender, EventArgs e)
        {
            if (EditingHotkey)
            {
                StopEditing();
            }
        }

        #endregion

    }
}
