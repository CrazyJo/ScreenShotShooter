using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApplication1.Properties;
using HelpersLib;
using MainLib;
using MainLib.AppSettings;

namespace WindowsFormsApplication1
{
    public sealed partial class SettingsForm : Form
    {
        private readonly ApplicationConfig _settings;
        private readonly HotKeySettingTab _hotKeySettingTab;
        private bool ready;

        private SettingsForm()
        {
            InitializeComponent();

            comboBox_ImageFormat.Items.AddRange(new object[] { "PNG", "JPEG", "BMP", "TIFF" });

            Icon = Resources.custom_settings_512;
        }

        public SettingsForm(ApplicationConfig settings) : this()
        {
            _settings = settings;
            var factory = UserControlFactory.GetFactory(settings);


            cB_PlaySound.Checked = _settings.GeneralSettings.PlaySoundAfterPrintScreen;

            comboBox_ImageFormat.Text = _settings.ImageSettings.PictureFormat;

            tBox_SavingFolder.Text = _settings.ImageSettings.PictureSaveFolder;

            cB__AutoStart.Checked = AutoStart.IsAutoStartEnabled;
            cB__AutoStart.CheckedChanged += cB__AutoStart_CheckedChanged;

            _hotKeySettingTab = new HotKeySettingTab(flpHotkeys, Program.HotkeyManager,_settings.HotKeysConfig, factory);
            _hotKeySettingTab.AddControls();
            checkBox_Enabled.Checked = _hotKeySettingTab.IsHotKeysEnable;


            ChangeLanguage(_settings.GeneralSettings.Language);
            InitializeLanguageMenu();

            ready = true;
        }

        private void InitializeLanguageMenu()
        {
            foreach (SupportedLanguage language in Helpers.GetEnums<SupportedLanguage>())
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(language.GetLocalizedDescription())
                {
                    Image = GetLanguageIcon(language),
                    ImageScaling = ToolStripItemImageScaling.None
                };

                SupportedLanguage lang = language;
                tsmi.Click += (sender, e) => ChangeLanguage(lang);
                cmsLanguages.Items.Add(tsmi);
            }
        }

        private void ChangeLanguage(SupportedLanguage language)
        {
            btnLanguages.Text = language.GetLocalizedDescription();
            btnLanguages.Image = GetLanguageIcon(language);

            if (ready)
            {
                Program.Settings.GeneralSettings.Language = language;

                if (LanguageHelper.ChangeLanguage(Program.Settings.GeneralSettings.Language) &&
                    MessageBox.Show(Resources.ApplicationSettingsForm_cbLanguage_SelectedIndexChanged_Language_Restart,
                    @"Screen Shot Shooter", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                //if(LanguageHelper.ChangeLanguage(Program.Settings.GeneralSettings.Language))
                {
                    Program.Restart();
                }
            }
        }

        private Image GetLanguageIcon(SupportedLanguage language)
        {
            Image icon;

            switch (language)
            {
                default:
                case SupportedLanguage.Automatic:
                    icon = Resources.globe_icon;
                    break;
                case SupportedLanguage.Russian:
                    icon = Resources.ru;
                    break;
                case SupportedLanguage.English:
                    icon = Resources.uk;
                    break;
            }

            return icon;
        }

        #region Event Handlers

        private void cB__AutoStart_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;

            if (checkBox.Checked)
            {
                AutoStart.EnableAutoStart();
            }
            else
            {
                AutoStart.DisableSetAutoStart();
            }
        }

        private void comboBox_ImageFormat_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var comBox = (ComboBox)sender;
            _settings.ImageSettings.PictureFormat = (string)comBox.SelectedItem;
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.ShowDialog();

            var selectedPath = fbd.SelectedPath;

            if (!string.IsNullOrWhiteSpace(selectedPath))
            {
                _settings.ImageSettings.PictureSaveFolder = selectedPath;
                tBox_SavingFolder.Text = selectedPath;
            }
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            _hotKeySettingTab.RestoreDefaultHotkeys();
        }

        private void checkBox_Enabled_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = (CheckBox) sender;
            _hotKeySettingTab.IsHotKeysEnable = enabled.Checked;
        }

        private void cB_PlaySound_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = (CheckBox)sender;
            _settings.GeneralSettings.PlaySoundAfterPrintScreen = enabled.Checked;
        }

        #endregion

    }
}
