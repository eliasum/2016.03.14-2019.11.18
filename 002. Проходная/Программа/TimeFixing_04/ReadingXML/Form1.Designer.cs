namespace TimeFixing
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.AuthorsDataSet = new System.Data.DataSet();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.b_OpenXML = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tB_in = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tB_out = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tb_hours = new System.Windows.Forms.TextBox();
            this.tb_minutes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuthorsDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(506, 402);
            this.dataGridView1.TabIndex = 0;
            // 
            // AuthorsDataSet
            // 
            this.AuthorsDataSet.DataSetName = "NewDataSet";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // b_OpenXML
            // 
            this.b_OpenXML.Location = new System.Drawing.Point(12, 420);
            this.b_OpenXML.Name = "b_OpenXML";
            this.b_OpenXML.Size = new System.Drawing.Size(152, 23);
            this.b_OpenXML.TabIndex = 3;
            this.b_OpenXML.Text = "Открыть XML";
            this.b_OpenXML.UseVisualStyleBackColor = true;
            this.b_OpenXML.Click += new System.EventHandler(this.b_OpenXML_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 445);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Вход";
            // 
            // tB_in
            // 
            this.tB_in.Location = new System.Drawing.Point(12, 461);
            this.tB_in.Name = "tB_in";
            this.tB_in.Size = new System.Drawing.Size(100, 20);
            this.tB_in.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 482);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Выход";
            // 
            // tB_out
            // 
            this.tB_out.Location = new System.Drawing.Point(12, 498);
            this.tB_out.Name = "tB_out";
            this.tB_out.Size = new System.Drawing.Size(100, 20);
            this.tB_out.TabIndex = 19;
            // 
            // tb_hours
            // 
            this.tb_hours.Location = new System.Drawing.Point(182, 461);
            this.tb_hours.Name = "tb_hours";
            this.tb_hours.Size = new System.Drawing.Size(100, 20);
            this.tb_hours.TabIndex = 22;
            // 
            // tb_minutes
            // 
            this.tb_minutes.Location = new System.Drawing.Point(182, 498);
            this.tb_minutes.Name = "tb_minutes";
            this.tb_minutes.Size = new System.Drawing.Size(100, 20);
            this.tb_minutes.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(179, 445);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Часы";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(179, 482);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Минуты";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(524, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(146, 402);
            this.richTextBox1.TabIndex = 27;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(676, 12);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(146, 402);
            this.richTextBox2.TabIndex = 28;
            this.richTextBox2.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(828, 12);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(274, 402);
            this.richTextBox3.TabIndex = 29;
            this.richTextBox3.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1481, 533);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_minutes);
            this.Controls.Add(this.tb_hours);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tB_out);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tB_in);
            this.Controls.Add(this.b_OpenXML);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MainForm";
            this.Text = "Фиксация времени через проходную";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuthorsDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Data.DataSet AuthorsDataSet;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button b_OpenXML;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tB_in;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tB_out;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox tb_hours;
        private System.Windows.Forms.TextBox tb_minutes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
    }
}

