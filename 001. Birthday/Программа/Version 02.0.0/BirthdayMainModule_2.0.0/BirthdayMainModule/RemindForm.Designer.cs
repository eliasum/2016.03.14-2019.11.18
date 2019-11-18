namespace Birthday
{
    partial class RemindForm
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
            this.rTB_dates = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rTB_dates
            // 
            this.rTB_dates.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rTB_dates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rTB_dates.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rTB_dates.Location = new System.Drawing.Point(0, 0);
            this.rTB_dates.Name = "rTB_dates";
            this.rTB_dates.ReadOnly = true;
            this.rTB_dates.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rTB_dates.Size = new System.Drawing.Size(363, 305);
            this.rTB_dates.TabIndex = 1;
            this.rTB_dates.Text = "";
            this.rTB_dates.Click += new System.EventHandler(this.rTB_dates_Click);
            this.rTB_dates.DoubleClick += new System.EventHandler(this.rTB_dates_DoubleClick);
            // 
            // RemindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 305);
            this.ControlBox = false;
            this.Controls.Add(this.rTB_dates);
            this.Name = "RemindForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Внимание!";
            this.Click += new System.EventHandler(this.RemindForm_Click);
            this.DoubleClick += new System.EventHandler(this.RemindForm_DoubleClick);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox rTB_dates;
    }
}