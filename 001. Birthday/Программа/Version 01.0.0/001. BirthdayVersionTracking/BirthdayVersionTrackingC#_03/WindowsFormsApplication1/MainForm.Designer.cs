namespace BirthdayVersionTracking
{
    partial class MainForm
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
            this.B_Delete = new System.Windows.Forms.Button();
            this.b_Version = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.b_Load = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // B_Delete
            // 
            this.B_Delete.Location = new System.Drawing.Point(34, 24);
            this.B_Delete.Name = "B_Delete";
            this.B_Delete.Size = new System.Drawing.Size(178, 23);
            this.B_Delete.TabIndex = 0;
            this.B_Delete.Text = "Извлеч из автозапуска";
            this.B_Delete.UseVisualStyleBackColor = true;
            this.B_Delete.Click += new System.EventHandler(this.B_Delete_Click);
            // 
            // b_Version
            // 
            this.b_Version.Location = new System.Drawing.Point(34, 63);
            this.b_Version.Name = "b_Version";
            this.b_Version.Size = new System.Drawing.Size(117, 23);
            this.b_Version.TabIndex = 1;
            this.b_Version.Text = "Показать версию";
            this.b_Version.UseVisualStyleBackColor = true;
            this.b_Version.Click += new System.EventHandler(this.b_Version_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Версия";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(34, 110);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(178, 157);
            this.textBox1.TabIndex = 3;
            // 
            // b_Load
            // 
            this.b_Load.Location = new System.Drawing.Point(274, 125);
            this.b_Load.Name = "b_Load";
            this.b_Load.Size = new System.Drawing.Size(75, 23);
            this.b_Load.TabIndex = 4;
            this.b_Load.Text = "Запустить";
            this.b_Load.UseVisualStyleBackColor = true;
            this.b_Load.Click += new System.EventHandler(this.b_Load_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 316);
            this.Controls.Add(this.b_Load);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_Version);
            this.Controls.Add(this.B_Delete);
            this.Name = "MainForm";
            this.Text = "Birthday";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_Delete;
        private System.Windows.Forms.Button b_Version;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button b_Load;
    }
}

