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
            this.chB_debugMode = new System.Windows.Forms.CheckBox();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.b_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chB_debugMode
            // 
            this.chB_debugMode.AutoSize = true;
            this.chB_debugMode.Location = new System.Drawing.Point(12, 12);
            this.chB_debugMode.Name = "chB_debugMode";
            this.chB_debugMode.Size = new System.Drawing.Size(256, 17);
            this.chB_debugMode.TabIndex = 0;
            this.chB_debugMode.Text = "Включить режим отладки (для разработчика)";
            this.chB_debugMode.UseVisualStyleBackColor = true;
            this.chB_debugMode.CheckedChanged += new System.EventHandler(this.chB_debugMode_CheckedChanged);
            // 
            // b_Cancel
            // 
            this.b_Cancel.Location = new System.Drawing.Point(197, 226);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 4;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.UseVisualStyleBackColor = true;
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // b_OK
            // 
            this.b_OK.Location = new System.Drawing.Point(12, 226);
            this.b_OK.Name = "b_OK";
            this.b_OK.Size = new System.Drawing.Size(75, 23);
            this.b_OK.TabIndex = 3;
            this.b_OK.Text = "OK";
            this.b_OK.UseVisualStyleBackColor = true;
            this.b_OK.Click += new System.EventHandler(this.b_OK_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_OK);
            this.Controls.Add(this.chB_debugMode);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chB_debugMode;
        private System.Windows.Forms.Button b_Cancel;
        private System.Windows.Forms.Button b_OK;
    }
}