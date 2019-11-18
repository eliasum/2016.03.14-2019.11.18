﻿namespace TimeFixing
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
            this.tB_summ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tB_in = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tB_out = new System.Windows.Forms.TextBox();
            this.b_saveOrderedXml = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tb_hours = new System.Windows.Forms.TextBox();
            this.tb_minutes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuthorsDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(892, 402);
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
            // tB_summ
            // 
            this.tB_summ.Location = new System.Drawing.Point(12, 467);
            this.tB_summ.Name = "tB_summ";
            this.tB_summ.Size = new System.Drawing.Size(100, 20);
            this.tB_summ.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 451);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "summ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 492);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Вход";
            // 
            // tB_in
            // 
            this.tB_in.Location = new System.Drawing.Point(12, 508);
            this.tB_in.Name = "tB_in";
            this.tB_in.Size = new System.Drawing.Size(100, 20);
            this.tB_in.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 529);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Выход";
            // 
            // tB_out
            // 
            this.tB_out.Location = new System.Drawing.Point(12, 545);
            this.tB_out.Name = "tB_out";
            this.tB_out.Size = new System.Drawing.Size(100, 20);
            this.tB_out.TabIndex = 19;
            // 
            // b_saveOrderedXml
            // 
            this.b_saveOrderedXml.Location = new System.Drawing.Point(182, 420);
            this.b_saveOrderedXml.Name = "b_saveOrderedXml";
            this.b_saveOrderedXml.Size = new System.Drawing.Size(177, 23);
            this.b_saveOrderedXml.TabIndex = 21;
            this.b_saveOrderedXml.Text = "Сохранить XML";
            this.b_saveOrderedXml.UseVisualStyleBackColor = true;
            this.b_saveOrderedXml.Click += new System.EventHandler(this.b_saveOrderedXml_Click);
            // 
            // tb_hours
            // 
            this.tb_hours.Location = new System.Drawing.Point(136, 467);
            this.tb_hours.Name = "tb_hours";
            this.tb_hours.Size = new System.Drawing.Size(100, 20);
            this.tb_hours.TabIndex = 22;
            // 
            // tb_minutes
            // 
            this.tb_minutes.Location = new System.Drawing.Point(259, 467);
            this.tb_minutes.Name = "tb_minutes";
            this.tb_minutes.Size = new System.Drawing.Size(100, 20);
            this.tb_minutes.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(133, 451);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Часы";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(256, 451);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Минуты";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 577);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_minutes);
            this.Controls.Add(this.tb_hours);
            this.Controls.Add(this.b_saveOrderedXml);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tB_out);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tB_in);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tB_summ);
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
        private System.Windows.Forms.TextBox tB_summ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tB_in;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tB_out;
        private System.Windows.Forms.Button b_saveOrderedXml;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox tb_hours;
        private System.Windows.Forms.TextBox tb_minutes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

