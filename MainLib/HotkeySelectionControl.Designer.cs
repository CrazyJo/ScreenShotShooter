namespace MainLib
{
    partial class HotkeySelectionControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblHotkeyDescription = new System.Windows.Forms.Label();
            this.btnHotkey = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHotkeyDescription
            // 
            this.lblHotkeyDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHotkeyDescription.Location = new System.Drawing.Point(5, 1);
            this.lblHotkeyDescription.Name = "lblHotkeyDescription";
            this.lblHotkeyDescription.Size = new System.Drawing.Size(220, 21);
            this.lblHotkeyDescription.TabIndex = 0;
            this.lblHotkeyDescription.Text = "Description";
            this.lblHotkeyDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblHotkeyDescription.UseMnemonic = false;
            // 
            // btnHotkey
            // 
            this.btnHotkey.Location = new System.Drawing.Point(230, 0);
            this.btnHotkey.Name = "btnHotkey";
            this.btnHotkey.Size = new System.Drawing.Size(190, 23);
            this.btnHotkey.TabIndex = 1;
            this.btnHotkey.Text = "Hotkey";
            this.btnHotkey.UseVisualStyleBackColor = true;
            this.btnHotkey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnHotkey_KeyDown);
            this.btnHotkey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btnHotkey_KeyUp);
            this.btnHotkey.Leave += new System.EventHandler(this.btnHotkey_Leave);
            this.btnHotkey.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnHotkey_MouseClick);
            this.btnHotkey.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.btnHotkey_PreviewKeyDown);
            // 
            // HotkeySelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnHotkey);
            this.Controls.Add(this.lblHotkeyDescription);
            this.Name = "HotkeySelectionControl";
            this.Size = new System.Drawing.Size(424, 23);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnHotkey;
        private System.Windows.Forms.Label lblHotkeyDescription;
    }
}
