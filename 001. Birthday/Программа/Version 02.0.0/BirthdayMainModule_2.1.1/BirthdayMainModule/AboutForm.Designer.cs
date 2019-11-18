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
            this.sB_OK = new DevExpress.XtraEditors.SimpleButton();
            this.lBC_Description = new DevExpress.XtraEditors.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.lBC_Description)).BeginInit();
            this.SuspendLayout();
            // 
            // sB_OK
            // 
            this.sB_OK.Location = new System.Drawing.Point(96, 188);
            this.sB_OK.Name = "sB_OK";
            this.sB_OK.Size = new System.Drawing.Size(75, 23);
            this.sB_OK.TabIndex = 7;
            this.sB_OK.Text = "OK";
            this.sB_OK.Click += new System.EventHandler(this.sB_OK_Click);
            // 
            // lBC_Description
            // 
            this.lBC_Description.Appearance.BackColor = System.Drawing.Color.White;
            this.lBC_Description.Appearance.Options.UseBackColor = true;
            this.lBC_Description.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.lBC_Description.HighlightedItemStyle = DevExpress.XtraEditors.HighlightStyle.Skinned;
            this.lBC_Description.Items.AddRange(new object[] {
            "Если у Вас есть какие либо предложения и ",
            "вопросы, обращайтесь к программисту",
            "Лаборатории информационных технологий",
            "Минину Илье Михайловичу по электронной ",
            "почте mim81104@c31.nccp.ru или по ",
            "внутреннему телефону ✆ 11-61."});
            this.lBC_Description.Location = new System.Drawing.Point(12, 25);
            this.lBC_Description.Name = "lBC_Description";
            this.lBC_Description.ShowFocusRect = false;
            this.lBC_Description.Size = new System.Drawing.Size(247, 157);
            this.lBC_Description.TabIndex = 8;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 228);
            this.Controls.Add(this.lBC_Description);
            this.Controls.Add(this.sB_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "О программе";
            ((System.ComponentModel.ISupportInitialize)(this.lBC_Description)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton sB_OK;
        private DevExpress.XtraEditors.ListBoxControl lBC_Description;
    }
}