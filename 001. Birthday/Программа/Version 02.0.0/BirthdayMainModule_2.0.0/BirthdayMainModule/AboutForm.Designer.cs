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
            this.b_OK = new System.Windows.Forms.Button();
            this.l_Version = new System.Windows.Forms.Label();
            this.lB_Description = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // b_OK
            // 
            this.b_OK.Location = new System.Drawing.Point(110, 226);
            this.b_OK.Name = "b_OK";
            this.b_OK.Size = new System.Drawing.Size(75, 23);
            this.b_OK.TabIndex = 4;
            this.b_OK.Text = "OK";
            this.b_OK.UseVisualStyleBackColor = true;
            this.b_OK.Click += new System.EventHandler(this.b_OK_Click);
            // 
            // l_Version
            // 
            this.l_Version.AutoSize = true;
            this.l_Version.Location = new System.Drawing.Point(12, 9);
            this.l_Version.Name = "l_Version";
            this.l_Version.Size = new System.Drawing.Size(72, 13);
            this.l_Version.TabIndex = 5;
            this.l_Version.Text = "Version 2.0.0.";
            // 
            // lB_Description
            // 
            this.lB_Description.BackColor = System.Drawing.SystemColors.MenuBar;
            this.lB_Description.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lB_Description.FormattingEnabled = true;
            this.lB_Description.Items.AddRange(new object[] {
            "01. Введена проверка ещё одного запущенного",
            " Birthday.",
            "",
            "02. Запись/удаление автозагрузки при ",
            "установке/удалении программы с ",
            "компьютера.",
            "",
            "03. Автоматическая проверка обновления при",
            " перезапуске программы."});
            this.lB_Description.Location = new System.Drawing.Point(12, 34);
            this.lB_Description.Name = "lB_Description";
            this.lB_Description.Size = new System.Drawing.Size(260, 134);
            this.lB_Description.TabIndex = 6;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lB_Description);
            this.Controls.Add(this.l_Version);
            this.Controls.Add(this.b_OK);
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AboutForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_OK;
        private System.Windows.Forms.Label l_Version;
        private System.Windows.Forms.ListBox lB_Description;
    }
}