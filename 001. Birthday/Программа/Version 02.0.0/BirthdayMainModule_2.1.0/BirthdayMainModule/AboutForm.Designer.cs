namespace Birthday
{
    partial class AboutForm
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
            this.l_Version = new System.Windows.Forms.Label();
            this.sB_OK = new DevExpress.XtraEditors.SimpleButton();
            this.lBC_Description = new DevExpress.XtraEditors.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.lBC_Description)).BeginInit();
            this.SuspendLayout();
            // 
            // l_Version
            // 
            this.l_Version.AutoSize = true;
            this.l_Version.Location = new System.Drawing.Point(12, 9);
            this.l_Version.Name = "l_Version";
            this.l_Version.Size = new System.Drawing.Size(75, 13);
            this.l_Version.TabIndex = 5;
            this.l_Version.Text = "Version 2.0.0.";
            // 
            // sB_OK
            // 
            this.sB_OK.Location = new System.Drawing.Point(107, 226);
            this.sB_OK.Name = "sB_OK";
            this.sB_OK.Size = new System.Drawing.Size(75, 23);
            this.sB_OK.TabIndex = 7;
            this.sB_OK.Text = "OK";
            this.sB_OK.Click += new System.EventHandler(this.sB_OK_Click);
            // 
            // lBC_Description
            // 
            this.lBC_Description.Items.AddRange(new object[] {
            "01. Введена проверка ещё одного запущенного",
            " Birthday.",
            "",
            "02. Запись/удаление автозагрузки при ",
            "установке/удалении программы с ",
            "компьютера.",
            "",
            "03. Автоматическая проверка обновления при",
            " перезапуске программы."});
            this.lBC_Description.Location = new System.Drawing.Point(12, 25);
            this.lBC_Description.Name = "lBC_Description";
            this.lBC_Description.Size = new System.Drawing.Size(260, 157);
            this.lBC_Description.TabIndex = 8;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lBC_Description);
            this.Controls.Add(this.sB_OK);
            this.Controls.Add(this.l_Version);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "О программе";
            ((System.ComponentModel.ISupportInitialize)(this.lBC_Description)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label l_Version;
        private DevExpress.XtraEditors.SimpleButton sB_OK;
        private DevExpress.XtraEditors.ListBoxControl lBC_Description;
    }
}