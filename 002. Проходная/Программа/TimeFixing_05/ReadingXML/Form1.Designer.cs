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
            this.dGV_XML = new System.Windows.Forms.DataGridView();
            this.AuthorsDataSet = new System.Data.DataSet();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.b_OpenXML = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tb_hours = new System.Windows.Forms.TextBox();
            this.tb_minutes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rTB_Errors = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_XML)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuthorsDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dGV_XML.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_XML.Location = new System.Drawing.Point(12, 30);
            this.dGV_XML.Name = "dataGridView1";
            this.dGV_XML.ReadOnly = true;
            this.dGV_XML.Size = new System.Drawing.Size(889, 402);
            this.dGV_XML.TabIndex = 0;
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
            this.b_OpenXML.Location = new System.Drawing.Point(12, 447);
            this.b_OpenXML.Name = "b_OpenXML";
            this.b_OpenXML.Size = new System.Drawing.Size(152, 23);
            this.b_OpenXML.TabIndex = 3;
            this.b_OpenXML.Text = "Открыть XML";
            this.b_OpenXML.UseVisualStyleBackColor = true;
            this.b_OpenXML.Click += new System.EventHandler(this.b_OpenXML_Click);
            // 
            // tb_hours
            // 
            this.tb_hours.Location = new System.Drawing.Point(12, 489);
            this.tb_hours.Name = "tb_hours";
            this.tb_hours.ReadOnly = true;
            this.tb_hours.Size = new System.Drawing.Size(100, 20);
            this.tb_hours.TabIndex = 22;
            // 
            // tb_minutes
            // 
            this.tb_minutes.Location = new System.Drawing.Point(12, 526);
            this.tb_minutes.Name = "tb_minutes";
            this.tb_minutes.ReadOnly = true;
            this.tb_minutes.Size = new System.Drawing.Size(100, 20);
            this.tb_minutes.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 473);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Часы:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 510);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Минуты:";
            // 
            // richTextBox4
            // 
            this.rTB_Errors.Location = new System.Drawing.Point(907, 30);
            this.rTB_Errors.Name = "richTextBox4";
            this.rTB_Errors.ReadOnly = true;
            this.rTB_Errors.Size = new System.Drawing.Size(139, 402);
            this.rTB_Errors.TabIndex = 30;
            this.rTB_Errors.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Файл XML:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(904, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Список ошибок:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1058, 558);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rTB_Errors);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_minutes);
            this.Controls.Add(this.tb_hours);
            this.Controls.Add(this.b_OpenXML);
            this.Controls.Add(this.dGV_XML);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Фиксация времени через проходную";
            ((System.ComponentModel.ISupportInitialize)(this.dGV_XML)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuthorsDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dGV_XML;
        private System.Data.DataSet AuthorsDataSet;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button b_OpenXML;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox tb_hours;
        private System.Windows.Forms.TextBox tb_minutes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox rTB_Errors;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

