namespace WindowsFormsApplication1
{
    partial class SettingsForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tP_General = new System.Windows.Forms.TabPage();
            this.btnLanguages = new MainLib.MenuButton();
            this.cmsLanguages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cB_PlaySound = new System.Windows.Forms.CheckBox();
            this.label_language = new System.Windows.Forms.Label();
            this.cB__AutoStart = new System.Windows.Forms.CheckBox();
            this.tP_Image = new System.Windows.Forms.TabPage();
            this.comboBox_ImageFormat = new System.Windows.Forms.ComboBox();
            this.label_ImageFormat = new System.Windows.Forms.Label();
            this.button_Browse = new System.Windows.Forms.Button();
            this.tBox_SavingFolder = new System.Windows.Forms.TextBox();
            this.label_SavingFolder = new System.Windows.Forms.Label();
            this.tP_HotKeys = new System.Windows.Forms.TabPage();
            this.flpHotkeys = new System.Windows.Forms.FlowLayoutPanel();
            this.button_Reset = new System.Windows.Forms.Button();
            this.checkBox_Enabled = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tP_General.SuspendLayout();
            this.tP_Image.SuspendLayout();
            this.tP_HotKeys.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tP_General);
            this.tabControl1.Controls.Add(this.tP_Image);
            this.tabControl1.Controls.Add(this.tP_HotKeys);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tP_General
            // 
            this.tP_General.Controls.Add(this.btnLanguages);
            this.tP_General.Controls.Add(this.cB_PlaySound);
            this.tP_General.Controls.Add(this.label_language);
            this.tP_General.Controls.Add(this.cB__AutoStart);
            resources.ApplyResources(this.tP_General, "tP_General");
            this.tP_General.Name = "tP_General";
            this.tP_General.UseVisualStyleBackColor = true;
            // 
            // btnLanguages
            // 
            resources.ApplyResources(this.btnLanguages, "btnLanguages");
            this.btnLanguages.Menu = this.cmsLanguages;
            this.btnLanguages.Name = "btnLanguages";
            this.btnLanguages.UseVisualStyleBackColor = true;
            // 
            // cmsLanguages
            // 
            this.cmsLanguages.Name = "cmsLanguages";
            resources.ApplyResources(this.cmsLanguages, "cmsLanguages");
            // 
            // cB_PlaySound
            // 
            resources.ApplyResources(this.cB_PlaySound, "cB_PlaySound");
            this.cB_PlaySound.Name = "cB_PlaySound";
            this.cB_PlaySound.UseVisualStyleBackColor = true;
            this.cB_PlaySound.CheckedChanged += new System.EventHandler(this.cB_PlaySound_CheckedChanged);
            // 
            // label_language
            // 
            resources.ApplyResources(this.label_language, "label_language");
            this.label_language.Name = "label_language";
            // 
            // cB__AutoStart
            // 
            resources.ApplyResources(this.cB__AutoStart, "cB__AutoStart");
            this.cB__AutoStart.Name = "cB__AutoStart";
            this.cB__AutoStart.UseVisualStyleBackColor = true;
            // 
            // tP_Image
            // 
            this.tP_Image.Controls.Add(this.comboBox_ImageFormat);
            this.tP_Image.Controls.Add(this.label_ImageFormat);
            this.tP_Image.Controls.Add(this.button_Browse);
            this.tP_Image.Controls.Add(this.tBox_SavingFolder);
            this.tP_Image.Controls.Add(this.label_SavingFolder);
            resources.ApplyResources(this.tP_Image, "tP_Image");
            this.tP_Image.Name = "tP_Image";
            this.tP_Image.UseVisualStyleBackColor = true;
            // 
            // comboBox_ImageFormat
            // 
            this.comboBox_ImageFormat.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_ImageFormat, "comboBox_ImageFormat");
            this.comboBox_ImageFormat.Name = "comboBox_ImageFormat";
            this.comboBox_ImageFormat.SelectionChangeCommitted += new System.EventHandler(this.comboBox_ImageFormat_SelectionChangeCommitted);
            // 
            // label_ImageFormat
            // 
            resources.ApplyResources(this.label_ImageFormat, "label_ImageFormat");
            this.label_ImageFormat.Name = "label_ImageFormat";
            // 
            // button_Browse
            // 
            resources.ApplyResources(this.button_Browse, "button_Browse");
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // tBox_SavingFolder
            // 
            resources.ApplyResources(this.tBox_SavingFolder, "tBox_SavingFolder");
            this.tBox_SavingFolder.Name = "tBox_SavingFolder";
            // 
            // label_SavingFolder
            // 
            resources.ApplyResources(this.label_SavingFolder, "label_SavingFolder");
            this.label_SavingFolder.Name = "label_SavingFolder";
            // 
            // tP_HotKeys
            // 
            this.tP_HotKeys.Controls.Add(this.flpHotkeys);
            this.tP_HotKeys.Controls.Add(this.button_Reset);
            this.tP_HotKeys.Controls.Add(this.checkBox_Enabled);
            resources.ApplyResources(this.tP_HotKeys, "tP_HotKeys");
            this.tP_HotKeys.Name = "tP_HotKeys";
            this.tP_HotKeys.UseVisualStyleBackColor = true;
            // 
            // flpHotkeys
            // 
            resources.ApplyResources(this.flpHotkeys, "flpHotkeys");
            this.flpHotkeys.Name = "flpHotkeys";
            // 
            // button_Reset
            // 
            resources.ApplyResources(this.button_Reset, "button_Reset");
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // checkBox_Enabled
            // 
            resources.ApplyResources(this.checkBox_Enabled, "checkBox_Enabled");
            this.checkBox_Enabled.Name = "checkBox_Enabled";
            this.checkBox_Enabled.UseVisualStyleBackColor = true;
            this.checkBox_Enabled.CheckedChanged += new System.EventHandler(this.checkBox_Enabled_CheckedChanged);
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "SettingsForm";
            this.tabControl1.ResumeLayout(false);
            this.tP_General.ResumeLayout(false);
            this.tP_General.PerformLayout();
            this.tP_Image.ResumeLayout(false);
            this.tP_Image.PerformLayout();
            this.tP_HotKeys.ResumeLayout(false);
            this.tP_HotKeys.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tP_General;
        private System.Windows.Forms.CheckBox cB__AutoStart;
        private System.Windows.Forms.TabPage tP_Image;
        private System.Windows.Forms.TabPage tP_HotKeys;
        private System.Windows.Forms.Label label_language;
        private System.Windows.Forms.CheckBox cB_PlaySound;
        private System.Windows.Forms.Button button_Browse;
        private System.Windows.Forms.TextBox tBox_SavingFolder;
        private System.Windows.Forms.Label label_SavingFolder;
        private System.Windows.Forms.ComboBox comboBox_ImageFormat;
        private System.Windows.Forms.Label label_ImageFormat;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.CheckBox checkBox_Enabled;
        private System.Windows.Forms.FlowLayoutPanel flpHotkeys;
        private MainLib.MenuButton btnLanguages;
        private System.Windows.Forms.ContextMenuStrip cmsLanguages;
    }
}

