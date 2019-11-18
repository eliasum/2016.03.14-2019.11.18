namespace BirthdayMainModule
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.l_UserName = new System.Windows.Forms.Label();
            this.b_Sorting = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // l_UserName
            // 
            this.l_UserName.AutoSize = true;
            this.l_UserName.Location = new System.Drawing.Point(12, 9);
            this.l_UserName.Name = "l_UserName";
            this.l_UserName.Size = new System.Drawing.Size(115, 13);
            this.l_UserName.TabIndex = 7;
            this.l_UserName.Text = "Здравствуйте, Гость!";
            // 
            // b_Sorting
            // 
            this.b_Sorting.Location = new System.Drawing.Point(478, 35);
            this.b_Sorting.Name = "b_Sorting";
            this.b_Sorting.Size = new System.Drawing.Size(141, 23);
            this.b_Sorting.TabIndex = 9;
            this.b_Sorting.Text = "Изменить сортировку";
            this.b_Sorting.UseVisualStyleBackColor = true;
            this.b_Sorting.Click += new System.EventHandler(this.b_Sorting_Click);
            // 
            // treeView1
            // 
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(15, 35);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 2;
            this.treeView1.Size = new System.Drawing.Size(444, 374);
            this.treeView1.TabIndex = 10;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "0331.ico");
            this.imageList1.Images.SetKeyName(1, "0037.ico");
            this.imageList1.Images.SetKeyName(2, "0070.ico");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 639);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.b_Sorting);
            this.Controls.Add(this.l_UserName);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Birthday";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label l_UserName;
        private System.Windows.Forms.Button b_Sorting;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
    }
}

