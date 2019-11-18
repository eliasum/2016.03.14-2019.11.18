namespace Birthday
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sb_OK = new DevExpress.XtraEditors.SimpleButton();
            this.sB_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.chE_debugMode = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chE_debugMode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // sb_OK
            // 
            this.sb_OK.Location = new System.Drawing.Point(12, 226);
            this.sb_OK.Name = "sb_OK";
            this.sb_OK.Size = new System.Drawing.Size(75, 23);
            this.sb_OK.TabIndex = 5;
            this.sb_OK.Text = "OK";
            this.sb_OK.Click += new System.EventHandler(this.sb_OK_Click);
            // 
            // sB_Cancel
            // 
            this.sB_Cancel.Location = new System.Drawing.Point(199, 226);
            this.sB_Cancel.Name = "sB_Cancel";
            this.sB_Cancel.Size = new System.Drawing.Size(75, 23);
            this.sB_Cancel.TabIndex = 6;
            this.sB_Cancel.Text = "Cancel";
            this.sB_Cancel.Click += new System.EventHandler(this.sB_Cancel_Click);
            // 
            // chE_debugMode
            // 
            this.chE_debugMode.Location = new System.Drawing.Point(12, 12);
            this.chE_debugMode.Name = "chE_debugMode";
            this.chE_debugMode.Properties.Caption = "Включить режим отладки (для разработчика)";
            this.chE_debugMode.Size = new System.Drawing.Size(260, 19);
            this.chE_debugMode.TabIndex = 7;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.chE_debugMode);
            this.Controls.Add(this.sB_Cancel);
            this.Controls.Add(this.sb_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chE_debugMode.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton sb_OK;
        private DevExpress.XtraEditors.SimpleButton sB_Cancel;
        private DevExpress.XtraEditors.CheckEdit chE_debugMode;
    }
}