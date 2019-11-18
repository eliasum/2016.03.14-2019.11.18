namespace Camcorder
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainerImageView = new System.Windows.Forms.SplitContainer();
            this.splitContainerConfiguration = new System.Windows.Forms.SplitContainer();
            this.deviceListView = new System.Windows.Forms.ListView();
            this.imageListForDeviceList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tSB_CollMax = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tSB_ContinuousShot = new System.Windows.Forms.ToolStripButton();
            this.tSB_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tSB_Add = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tSB_Help = new System.Windows.Forms.ToolStripButton();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.sliderGain = new PylonC.NETSupportLibrary.SliderUserControl();
            this.updateDeviceListTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImageView)).BeginInit();
            this.splitContainerImageView.Panel1.SuspendLayout();
            this.splitContainerImageView.Panel2.SuspendLayout();
            this.splitContainerImageView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerConfiguration)).BeginInit();
            this.splitContainerConfiguration.Panel1.SuspendLayout();
            this.splitContainerConfiguration.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerImageView
            // 
            this.splitContainerImageView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerImageView.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerImageView.Location = new System.Drawing.Point(0, 0);
            this.splitContainerImageView.Name = "splitContainerImageView";
            // 
            // splitContainerImageView.Panel1
            // 
            this.splitContainerImageView.Panel1.Controls.Add(this.splitContainerConfiguration);
            this.splitContainerImageView.Panel1.Controls.Add(this.toolStrip);
            this.splitContainerImageView.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainerImageView.Panel1MinSize = 40;
            // 
            // splitContainerImageView.Panel2
            // 
            this.splitContainerImageView.Panel2.AutoScroll = true;
            this.splitContainerImageView.Panel2.Controls.Add(this.pictureBox);
            this.splitContainerImageView.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainerImageView.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainerImageView.Panel2MinSize = 40;
            this.splitContainerImageView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainerImageView.Size = new System.Drawing.Size(1387, 735);
            this.splitContainerImageView.SplitterDistance = 200;
            this.splitContainerImageView.TabIndex = 0;
            this.splitContainerImageView.TabStop = false;
            // 
            // splitContainerConfiguration
            // 
            this.splitContainerConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerConfiguration.Location = new System.Drawing.Point(0, 0);
            this.splitContainerConfiguration.Name = "splitContainerConfiguration";
            this.splitContainerConfiguration.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerConfiguration.Panel1
            // 
            this.splitContainerConfiguration.Panel1.Controls.Add(this.deviceListView);
            this.splitContainerConfiguration.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainerConfiguration.Panel1MinSize = 40;
            // 
            // splitContainerConfiguration.Panel2
            // 
            this.splitContainerConfiguration.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerConfiguration.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainerConfiguration.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainerConfiguration.Panel2Collapsed = true;
            this.splitContainerConfiguration.Panel2MinSize = 40;
            this.splitContainerConfiguration.Size = new System.Drawing.Size(159, 731);
            this.splitContainerConfiguration.SplitterDistance = 40;
            this.splitContainerConfiguration.TabIndex = 1;
            this.splitContainerConfiguration.TabStop = false;
            // 
            // deviceListView
            // 
            this.deviceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deviceListView.LargeImageList = this.imageListForDeviceList;
            this.deviceListView.Location = new System.Drawing.Point(0, 0);
            this.deviceListView.MultiSelect = false;
            this.deviceListView.Name = "deviceListView";
            this.deviceListView.ShowItemToolTips = true;
            this.deviceListView.Size = new System.Drawing.Size(159, 731);
            this.deviceListView.TabIndex = 0;
            this.deviceListView.UseCompatibleStateImageBehavior = false;
            this.deviceListView.View = System.Windows.Forms.View.Tile;
            this.deviceListView.SelectedIndexChanged += new System.EventHandler(this.deviceListView_SelectedIndexChanged);
            this.deviceListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deviceListView_KeyDown);
            // 
            // imageListForDeviceList
            // 
            this.imageListForDeviceList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListForDeviceList.ImageSize = new System.Drawing.Size(32, 32);
            this.imageListForDeviceList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSB_CollMax,
            this.toolStripSeparator2,
            this.tSB_ContinuousShot,
            this.tSB_Stop,
            this.toolStripSeparator1,
            this.tSB_Add,
            this.toolStripSeparator3,
            this.tSB_Help});
            this.toolStrip.Location = new System.Drawing.Point(159, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(37, 731);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // tSB_CollMax
            // 
            this.tSB_CollMax.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSB_CollMax.Image = ((System.Drawing.Image)(resources.GetObject("tSB_CollMax.Image")));
            this.tSB_CollMax.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSB_CollMax.Name = "tSB_CollMax";
            this.tSB_CollMax.Size = new System.Drawing.Size(34, 36);
            this.tSB_CollMax.Text = "Collapse/Maximize";
            this.tSB_CollMax.Click += new System.EventHandler(this.toolStripButton1_Click_1);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(34, 6);
            // 
            // tSB_ContinuousShot
            // 
            this.tSB_ContinuousShot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSB_ContinuousShot.Image = ((System.Drawing.Image)(resources.GetObject("tSB_ContinuousShot.Image")));
            this.tSB_ContinuousShot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSB_ContinuousShot.Name = "tSB_ContinuousShot";
            this.tSB_ContinuousShot.Size = new System.Drawing.Size(34, 36);
            this.tSB_ContinuousShot.Text = "Continuous Shot";
            this.tSB_ContinuousShot.ToolTipText = "Continuous Shot";
            this.tSB_ContinuousShot.Click += new System.EventHandler(this.toolStripButtonContinuousShot_Click);
            // 
            // tSB_Stop
            // 
            this.tSB_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSB_Stop.Image = ((System.Drawing.Image)(resources.GetObject("tSB_Stop.Image")));
            this.tSB_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSB_Stop.Name = "tSB_Stop";
            this.tSB_Stop.Size = new System.Drawing.Size(34, 36);
            this.tSB_Stop.Text = "Stop Grab";
            this.tSB_Stop.ToolTipText = "Stop Grab";
            this.tSB_Stop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(34, 6);
            // 
            // tSB_Add
            // 
            this.tSB_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSB_Add.Image = ((System.Drawing.Image)(resources.GetObject("tSB_Add.Image")));
            this.tSB_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSB_Add.Name = "tSB_Add";
            this.tSB_Add.Size = new System.Drawing.Size(34, 36);
            this.tSB_Add.Text = "Add window";
            this.tSB_Add.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(34, 6);
            // 
            // tSB_Help
            // 
            this.tSB_Help.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSB_Help.Image = ((System.Drawing.Image)(resources.GetObject("tSB_Help.Image")));
            this.tSB_Help.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSB_Help.Name = "tSB_Help";
            this.tSB_Help.Size = new System.Drawing.Size(34, 36);
            this.tSB_Help.Text = "toolStripButton1";
            this.tSB_Help.ToolTipText = "Help";
            this.tSB_Help.Click += new System.EventHandler(this.tSB_Help_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1179, 731);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Resize += new System.EventHandler(this.pictureBox_Resize);
            // 
            // sliderGain
            // 
            this.sliderGain.Location = new System.Drawing.Point(0, 0);
            this.sliderGain.MinimumSize = new System.Drawing.Size(200, 45);
            this.sliderGain.Name = "sliderGain";
            this.sliderGain.NodeName = "ValueName";
            this.sliderGain.Size = new System.Drawing.Size(225, 128);
            this.sliderGain.TabIndex = 0;
            // 
            // updateDeviceListTimer
            // 
            this.updateDeviceListTimer.Enabled = true;
            this.updateDeviceListTimer.Interval = 5000;
            this.updateDeviceListTimer.Tick += new System.EventHandler(this.updateDeviceListTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1387, 735);
            this.Controls.Add(this.splitContainerImageView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Camcorder: master window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainerImageView.Panel1.ResumeLayout(false);
            this.splitContainerImageView.Panel1.PerformLayout();
            this.splitContainerImageView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImageView)).EndInit();
            this.splitContainerImageView.ResumeLayout(false);
            this.splitContainerConfiguration.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerConfiguration)).EndInit();
            this.splitContainerConfiguration.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerImageView;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tSB_ContinuousShot;
        private System.Windows.Forms.ToolStripButton tSB_Stop;
        private System.Windows.Forms.ListView deviceListView;
        private PylonC.NETSupportLibrary.SliderUserControl sliderGain;

        private PylonC.NETSupportLibrary.SliderUserControl sliderWidth;
        private PylonC.NETSupportLibrary.SliderUserControl sliderExposureTime;
        private PylonC.NETSupportLibrary.SliderUserControl sliderHeight;
        private PylonC.NETSupportLibrary.EnumerationComboBoxUserControl comboBoxPixelFormat;
        private PylonC.NETSupportLibrary.EnumerationComboBoxUserControl comboBoxTestImage;
        private System.Windows.Forms.Timer updateDeviceListTimer;
        private System.Windows.Forms.ImageList imageListForDeviceList;
        private System.Windows.Forms.ToolStripButton tSB_Add;
        private System.Windows.Forms.SplitContainer splitContainerConfiguration;
        private System.Windows.Forms.ToolStripButton tSB_CollMax;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tSB_Help;
    }
}

