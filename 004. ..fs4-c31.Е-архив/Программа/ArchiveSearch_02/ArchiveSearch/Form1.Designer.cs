namespace ArchiveSearch
{
    partial class Form1
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.b_saveToTxt = new System.Windows.Forms.Button();
            this.b_exit = new System.Windows.Forms.Button();
            this.b_stop = new System.Windows.Forms.Button();
            this.b_start = new System.Windows.Forms.Button();
            this.b_saveToXlsx = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(512, 381);
            this.listBox1.TabIndex = 2;
            // 
            // b_saveToTxt
            // 
            this.b_saveToTxt.Location = new System.Drawing.Point(12, 399);
            this.b_saveToTxt.Name = "b_saveToTxt";
            this.b_saveToTxt.Size = new System.Drawing.Size(113, 23);
            this.b_saveToTxt.TabIndex = 3;
            this.b_saveToTxt.Text = "Сохранить в .txt";
            this.b_saveToTxt.UseVisualStyleBackColor = true;
            this.b_saveToTxt.Click += new System.EventHandler(this.b_saveToTxt_Click);
            // 
            // b_exit
            // 
            this.b_exit.Location = new System.Drawing.Point(449, 399);
            this.b_exit.Name = "b_exit";
            this.b_exit.Size = new System.Drawing.Size(75, 23);
            this.b_exit.TabIndex = 6;
            this.b_exit.Tag = "3";
            this.b_exit.Text = "Выход";
            this.b_exit.UseVisualStyleBackColor = true;
            this.b_exit.Click += new System.EventHandler(this.b_exit_Click);
            // 
            // b_stop
            // 
            this.b_stop.Location = new System.Drawing.Point(320, 399);
            this.b_stop.Name = "b_stop";
            this.b_stop.Size = new System.Drawing.Size(75, 23);
            this.b_stop.TabIndex = 5;
            this.b_stop.Tag = "2";
            this.b_stop.Text = "Стоп";
            this.b_stop.UseVisualStyleBackColor = true;
            this.b_stop.Click += new System.EventHandler(this.b_stop_Click);
            // 
            // b_start
            // 
            this.b_start.Location = new System.Drawing.Point(185, 399);
            this.b_start.Name = "b_start";
            this.b_start.Size = new System.Drawing.Size(75, 23);
            this.b_start.TabIndex = 4;
            this.b_start.Tag = "1";
            this.b_start.Text = "Старт";
            this.b_start.UseVisualStyleBackColor = true;
            this.b_start.Click += new System.EventHandler(this.b_start_Click);
            // 
            // b_saveToXlsx
            // 
            this.b_saveToXlsx.Location = new System.Drawing.Point(12, 428);
            this.b_saveToXlsx.Name = "b_saveToXlsx";
            this.b_saveToXlsx.Size = new System.Drawing.Size(113, 23);
            this.b_saveToXlsx.TabIndex = 7;
            this.b_saveToXlsx.Tag = "1";
            this.b_saveToXlsx.Text = "Сохранить в .xlsx";
            this.b_saveToXlsx.UseVisualStyleBackColor = true;
            this.b_saveToXlsx.Click += new System.EventHandler(this.b_saveToXlsx_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(557, 172);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 8;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(557, 216);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 9;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 616);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.b_saveToXlsx);
            this.Controls.Add(this.b_exit);
            this.Controls.Add(this.b_stop);
            this.Controls.Add(this.b_start);
            this.Controls.Add(this.b_saveToTxt);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button b_saveToTxt;
        private System.Windows.Forms.Button b_exit;
        private System.Windows.Forms.Button b_stop;
        private System.Windows.Forms.Button b_start;
        private System.Windows.Forms.Button b_saveToXlsx;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}

