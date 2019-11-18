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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cBE_Hours = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cBE_Days = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.chE_Holiday = new DevExpress.XtraEditors.CheckEdit();
            this.chE_Birthday = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chE_debugMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cBE_Hours.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cBE_Days.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chE_Holiday.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chE_Birthday.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // sb_OK
            // 
            this.sb_OK.Location = new System.Drawing.Point(7, 151);
            this.sb_OK.Name = "sb_OK";
            this.sb_OK.Size = new System.Drawing.Size(75, 23);
            this.sb_OK.TabIndex = 5;
            this.sb_OK.Text = "OK";
            this.sb_OK.Click += new System.EventHandler(this.sb_OK_Click);
            // 
            // sB_Cancel
            // 
            this.sB_Cancel.Location = new System.Drawing.Point(194, 151);
            this.sB_Cancel.Name = "sB_Cancel";
            this.sB_Cancel.Size = new System.Drawing.Size(75, 23);
            this.sB_Cancel.TabIndex = 6;
            this.sB_Cancel.Text = "Cancel";
            this.sB_Cancel.Click += new System.EventHandler(this.sB_Cancel_Click);
            // 
            // chE_debugMode
            // 
            this.chE_debugMode.Location = new System.Drawing.Point(7, 126);
            this.chE_debugMode.Name = "chE_debugMode";
            this.chE_debugMode.Properties.Caption = "Включить режим отладки (для разработчика)";
            this.chE_debugMode.Size = new System.Drawing.Size(260, 19);
            this.chE_debugMode.TabIndex = 7;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cBE_Hours);
            this.groupControl1.Controls.Add(this.cBE_Days);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.chE_Holiday);
            this.groupControl1.Controls.Add(this.chE_Birthday);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(255, 108);
            this.groupControl1.TabIndex = 9;
            this.groupControl1.Text = "Оповещения";
            // 
            // cBE_Hours
            // 
            this.cBE_Hours.EditValue = "3";
            this.cBE_Hours.Location = new System.Drawing.Point(144, 76);
            this.cBE_Hours.Name = "cBE_Hours";
            this.cBE_Hours.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cBE_Hours.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.cBE_Hours.Size = new System.Drawing.Size(48, 20);
            this.cBE_Hours.TabIndex = 11;
            // 
            // cBE_Days
            // 
            this.cBE_Days.EditValue = "3";
            this.cBE_Days.Location = new System.Drawing.Point(23, 76);
            this.cBE_Days.Name = "cBE_Days";
            this.cBE_Days.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cBE_Days.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.cBE_Days.Size = new System.Drawing.Size(48, 20);
            this.cBE_Days.TabIndex = 10;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(198, 78);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(10, 13);
            this.labelControl3.TabIndex = 12;
            this.labelControl3.Text = "ч.";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(77, 78);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(61, 13);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "дн. каждые";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(5, 78);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(12, 13);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "За";
            // 
            // chE_Holiday
            // 
            this.chE_Holiday.EditValue = true;
            this.chE_Holiday.Location = new System.Drawing.Point(5, 48);
            this.chE_Holiday.Name = "chE_Holiday";
            this.chE_Holiday.Properties.Caption = "Оповещать о праздниках";
            this.chE_Holiday.Size = new System.Drawing.Size(180, 19);
            this.chE_Holiday.TabIndex = 1;
            // 
            // chE_Birthday
            // 
            this.chE_Birthday.EditValue = true;
            this.chE_Birthday.Location = new System.Drawing.Point(5, 23);
            this.chE_Birthday.Name = "chE_Birthday";
            this.chE_Birthday.Properties.Caption = "Оповещать о днях Рождения";
            this.chE_Birthday.Size = new System.Drawing.Size(180, 19);
            this.chE_Birthday.TabIndex = 0;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 186);
            this.Controls.Add(this.groupControl1);
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
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cBE_Hours.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cBE_Days.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chE_Holiday.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chE_Birthday.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton sb_OK;
        private DevExpress.XtraEditors.SimpleButton sB_Cancel;
        private DevExpress.XtraEditors.CheckEdit chE_debugMode;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckEdit chE_Holiday;
        private DevExpress.XtraEditors.CheckEdit chE_Birthday;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cBE_Days;
        private DevExpress.XtraEditors.ComboBoxEdit cBE_Hours;
    }
}