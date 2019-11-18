using System.Windows.Forms;

namespace Birthday
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
            DevExpress.Utils.Animation.PushTransition pushTransition1 = new DevExpress.Utils.Animation.PushTransition();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.календарьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пЗОToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сУПToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.метрологToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.workspaceManager1 = new DevExpress.Utils.WorkspaceManager();
            this.skinRibbonGalleryBarItem1 = new DevExpress.XtraBars.SkinRibbonGalleryBarItem();
            this.t_MainForm = new System.Windows.Forms.Timer(this.components);
            this.remindTimer = new System.Windows.Forms.Timer(this.components);
            this.tV_Tree = new System.Windows.Forms.TreeView();
            this.tB_Search = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bSI_MainMenu = new DevExpress.XtraBars.BarSubItem();
            this.bBI_Sorting = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_Skins = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_Settings = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_PZO = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_SUP = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_Metrolog = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_Exit = new DevExpress.XtraBars.BarButtonItem();
            this.bSI_ReferenceMenu = new DevExpress.XtraBars.BarSubItem();
            this.bBI_Reference = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_About = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_Calendar = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonControl2 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.ribbonControl3 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl3)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "0331.ico");
            this.imageList1.Images.SetKeyName(1, "0037.ico");
            this.imageList1.Images.SetKeyName(2, "0070.ico");
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked_1);
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseMove);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem,
            this.календарьToolStripMenuItem,
            this.пЗОToolStripMenuItem,
            this.сУПToolStripMenuItem,
            this.метрологToolStripMenuItem,
            this.toolStripMenuItem2,
            this.справкаToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(150, 164);
            this.contextMenuStrip1.MouseLeave += new System.EventHandler(this.contextMenuStrip1_MouseLeave);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // календарьToolStripMenuItem
            // 
            this.календарьToolStripMenuItem.Name = "календарьToolStripMenuItem";
            this.календарьToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.календарьToolStripMenuItem.Text = "Календарь";
            this.календарьToolStripMenuItem.Click += new System.EventHandler(this.календарьToolStripMenuItem_Click);
            // 
            // пЗОToolStripMenuItem
            // 
            this.пЗОToolStripMenuItem.Name = "пЗОToolStripMenuItem";
            this.пЗОToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.пЗОToolStripMenuItem.Text = "ПЗО";
            this.пЗОToolStripMenuItem.Click += new System.EventHandler(this.пЗОToolStripMenuItem_Click);
            // 
            // сУПToolStripMenuItem
            // 
            this.сУПToolStripMenuItem.Name = "сУПToolStripMenuItem";
            this.сУПToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.сУПToolStripMenuItem.Text = "СУП";
            this.сУПToolStripMenuItem.Click += new System.EventHandler(this.сУПToolStripMenuItem_Click);
            // 
            // метрологToolStripMenuItem
            // 
            this.метрологToolStripMenuItem.Name = "метрологToolStripMenuItem";
            this.метрологToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.метрологToolStripMenuItem.Text = "Метролог";
            this.метрологToolStripMenuItem.Click += new System.EventHandler(this.метрологToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(146, 6);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Действие";
            this.barSubItem1.Id = 1;
            this.barSubItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem1.ImageOptions.Image")));
            this.barSubItem1.Name = "barSubItem1";
            // 
            // popupMenu1
            // 
            this.popupMenu1.Name = "popupMenu1";
            // 
            // workspaceManager1
            // 
            this.workspaceManager1.TargetControl = this;
            this.workspaceManager1.TransitionType = pushTransition1;
            // 
            // skinRibbonGalleryBarItem1
            // 
            this.skinRibbonGalleryBarItem1.Caption = "skinRibbonGalleryBarItem1";
            this.skinRibbonGalleryBarItem1.Id = 19;
            this.skinRibbonGalleryBarItem1.Name = "skinRibbonGalleryBarItem1";
            // 
            // t_MainForm
            // 
            this.t_MainForm.Interval = 300000;
            this.t_MainForm.Tick += new System.EventHandler(this.t_MainForm_Tick);
            // 
            // remindTimer
            // 
            this.remindTimer.Tick += new System.EventHandler(this.remindTimer_Tick);
            // 
            // tV_Tree
            // 
            this.tV_Tree.BackColor = System.Drawing.SystemColors.Info;
            this.tV_Tree.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tV_Tree.FullRowSelect = true;
            this.tV_Tree.HotTracking = true;
            this.tV_Tree.ImageIndex = 0;
            this.tV_Tree.ImageList = this.imageList1;
            this.tV_Tree.Location = new System.Drawing.Point(15, 107);
            this.tV_Tree.Name = "tV_Tree";
            this.tV_Tree.SelectedImageIndex = 2;
            this.tV_Tree.ShowNodeToolTips = true;
            this.tV_Tree.Size = new System.Drawing.Size(358, 374);
            this.tV_Tree.TabIndex = 10;
            this.tV_Tree.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // tB_Search
            // 
            this.tB_Search.BackColor = System.Drawing.SystemColors.Info;
            this.tB_Search.Location = new System.Drawing.Point(15, 80);
            this.tB_Search.Name = "tB_Search";
            this.tB_Search.Size = new System.Drawing.Size(358, 21);
            this.tB_Search.TabIndex = 11;
            this.tB_Search.Click += new System.EventHandler(this.tB_Search_Click);
            this.tB_Search.TextChanged += new System.EventHandler(this.tB_Search_TextChanged);
            this.tB_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_Search_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 55);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(111, 19);
            this.label1.TabIndex = 13;
            this.label1.Text = "Поиск в дереве:";
            // 
            // bSI_MainMenu
            // 
            this.bSI_MainMenu.AccessibleName = "";
            this.bSI_MainMenu.Caption = "Главное меню";
            this.bSI_MainMenu.Id = 1;
            this.bSI_MainMenu.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bSI_MainMenu.ImageOptions.Image")));
            this.bSI_MainMenu.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bSI_MainMenu.ImageOptions.LargeImage")));
            this.bSI_MainMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Sorting),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Skins),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Settings),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_PZO),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_SUP),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Metrolog),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Exit)});
            this.bSI_MainMenu.MenuCaption = "iii";
            this.bSI_MainMenu.Name = "bSI_MainMenu";
            this.bSI_MainMenu.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            // 
            // bBI_Sorting
            // 
            this.bBI_Sorting.Caption = "Изменить сортировку";
            this.bBI_Sorting.Id = 16;
            this.bBI_Sorting.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBI_Sorting.ImageOptions.Image")));
            this.bBI_Sorting.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBI_Sorting.ImageOptions.LargeImage")));
            this.bBI_Sorting.Name = "bBI_Sorting";
            this.bBI_Sorting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Sorting__ItemClick);
            // 
            // bBI_Skins
            // 
            this.bBI_Skins.Caption = "Темы оформления";
            this.bBI_Skins.Id = 1;
            this.bBI_Skins.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBI_Skins.ImageOptions.Image")));
            this.bBI_Skins.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBI_Skins.ImageOptions.LargeImage")));
            this.bBI_Skins.Name = "bBI_Skins";
            this.bBI_Skins.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Skins_ItemClick);
            // 
            // bBI_Settings
            // 
            this.bBI_Settings.Caption = "Настройки";
            this.bBI_Settings.Id = 16;
            this.bBI_Settings.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBI_Settings.ImageOptions.Image")));
            this.bBI_Settings.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBI_Settings.ImageOptions.LargeImage")));
            this.bBI_Settings.Name = "bBI_Settings";
            this.bBI_Settings.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Settings__ItemClick);
            // 
            // bBI_PZO
            // 
            this.bBI_PZO.Caption = "ПЗО";
            this.bBI_PZO.Id = 20;
            this.bBI_PZO.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBI_PZO.ImageOptions.Image")));
            this.bBI_PZO.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBI_PZO.ImageOptions.LargeImage")));
            this.bBI_PZO.Name = "bBI_PZO";
            this.bBI_PZO.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_PZO__ItemClick);
            // 
            // bBI_SUP
            // 
            this.bBI_SUP.Caption = "СУП";
            this.bBI_SUP.Id = 21;
            this.bBI_SUP.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBI_SUP.ImageOptions.Image")));
            this.bBI_SUP.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBI_SUP.ImageOptions.LargeImage")));
            this.bBI_SUP.Name = "bBI_SUP";
            this.bBI_SUP.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_SUP__ItemClick);
            // 
            // bBI_Metrolog
            // 
            this.bBI_Metrolog.Caption = "Метролог";
            this.bBI_Metrolog.Id = 22;
            this.bBI_Metrolog.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBI_Metrolog.ImageOptions.Image")));
            this.bBI_Metrolog.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBI_Metrolog.ImageOptions.LargeImage")));
            this.bBI_Metrolog.Name = "bBI_Metrolog";
            this.bBI_Metrolog.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Metrolog__ItemClick);
            // 
            // bBI_Exit
            // 
            this.bBI_Exit.AllowDrawArrowInMenu = false;
            this.bBI_Exit.Caption = "Выход";
            this.bBI_Exit.Id = 25;
            this.bBI_Exit.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBI_Exit.ImageOptions.Image")));
            this.bBI_Exit.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBI_Exit.ImageOptions.LargeImage")));
            this.bBI_Exit.Name = "bBI_Exit";
            this.bBI_Exit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Exit_ItemClick);
            // 
            // bSI_ReferenceMenu
            // 
            this.bSI_ReferenceMenu.Caption = "Справочное меню";
            this.bSI_ReferenceMenu.Id = 13;
            this.bSI_ReferenceMenu.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bSI_ReferenceMenu.ImageOptions.Image")));
            this.bSI_ReferenceMenu.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bSI_ReferenceMenu.ImageOptions.LargeImage")));
            this.bSI_ReferenceMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Reference),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_About)});
            this.bSI_ReferenceMenu.Name = "bSI_ReferenceMenu";
            // 
            // bBI_Reference
            // 
            this.bBI_Reference.Caption = "Справка";
            this.bBI_Reference.Id = 17;
            this.bBI_Reference.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBI_Reference.ImageOptions.Image")));
            this.bBI_Reference.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBI_Reference.ImageOptions.LargeImage")));
            this.bBI_Reference.Name = "bBI_Reference";
            this.bBI_Reference.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Reference_ItemClick);
            // 
            // bBI_About
            // 
            this.bBI_About.Caption = "О программе";
            this.bBI_About.Id = 18;
            this.bBI_About.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBI_About.ImageOptions.Image")));
            this.bBI_About.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBI_About.ImageOptions.LargeImage")));
            this.bBI_About.Name = "bBI_About";
            this.bBI_About.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_About_ItemClick);
            // 
            // bBI_Calendar
            // 
            this.bBI_Calendar.Caption = "Календарь";
            this.bBI_Calendar.Id = 15;
            this.bBI_Calendar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBI_Calendar.ImageOptions.Image")));
            this.bBI_Calendar.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBI_Calendar.ImageOptions.LargeImage")));
            this.bBI_Calendar.Name = "bBI_Calendar";
            this.bBI_Calendar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Calendar_ItemClick);
            // 
            // ribbonControl2
            // 
            this.ribbonControl2.BackColor = System.Drawing.Color.Yellow;
            this.ribbonControl2.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.Blue;
            this.ribbonControl2.ExpandCollapseItem.Id = 0;
            this.ribbonControl2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ribbonControl2.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl2.ExpandCollapseItem,
            this.bSI_MainMenu,
            this.bBI_Settings,
            this.bBI_PZO,
            this.bBI_SUP,
            this.bBI_Metrolog,
            this.bBI_Exit,
            this.bSI_ReferenceMenu,
            this.bBI_Calendar,
            this.bBI_Sorting,
            this.bBI_Reference,
            this.bBI_About,
            this.bBI_Skins});
            this.ribbonControl2.Location = new System.Drawing.Point(0, 27);
            this.ribbonControl2.MaxItemId = 25;
            this.ribbonControl2.Name = "ribbonControl2";
            this.ribbonControl2.QuickToolbarItemLinks.Add(this.bSI_MainMenu);
            this.ribbonControl2.QuickToolbarItemLinks.Add(this.bBI_Calendar);
            this.ribbonControl2.QuickToolbarItemLinks.Add(this.bSI_ReferenceMenu);
            this.ribbonControl2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ribbonControl2.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl2.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl2.ShowItemCaptionsInQAT = true;
            this.ribbonControl2.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.ShowOnMultiplePages;
            this.ribbonControl2.ShowToolbarCustomizeItem = false;
            this.ribbonControl2.Size = new System.Drawing.Size(400, 25);
            this.ribbonControl2.Toolbar.ShowCustomizeItem = false;
            this.ribbonControl2.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
            // 
            // ribbonControl3
            // 
            this.ribbonControl3.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.Blue;
            this.ribbonControl3.ExpandCollapseItem.AllowDrawArrow = false;
            this.ribbonControl3.ExpandCollapseItem.AllowDrawArrowInMenu = false;
            this.ribbonControl3.ExpandCollapseItem.AllowRightClickInMenu = false;
            this.ribbonControl3.ExpandCollapseItem.Id = 0;
            this.ribbonControl3.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl3.ExpandCollapseItem});
            this.ribbonControl3.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl3.MaxItemId = 9;
            this.ribbonControl3.Name = "ribbonControl3";
            this.ribbonControl3.RibbonCaptionAlignment = DevExpress.XtraBars.Ribbon.RibbonCaptionAlignment.Center;
            this.ribbonControl3.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
            this.ribbonControl3.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl3.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl3.ShowQatLocationSelector = false;
            this.ribbonControl3.ShowToolbarCustomizeItem = false;
            this.ribbonControl3.Size = new System.Drawing.Size(400, 27);
            this.ribbonControl3.Toolbar.ShowCustomizeItem = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 500);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tB_Search);
            this.Controls.Add(this.tV_Tree);
            this.Controls.Add(this.ribbonControl2);
            this.Controls.Add(this.ribbonControl3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Ribbon = this.ribbonControl3;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Дни Рождения";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem календарьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пЗОToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сУПToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem метрологToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private ToolTip toolTip1;
        protected DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.Utils.WorkspaceManager workspaceManager1;
        private DevExpress.XtraBars.SkinRibbonGalleryBarItem skinRibbonGalleryBarItem1;
        private Timer t_MainForm;
        private Timer remindTimer;
        private Label label1;
        private TextBox tB_Search;
        private TreeView tV_Tree;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl2;
        private DevExpress.XtraBars.BarSubItem bSI_MainMenu;
        private DevExpress.XtraBars.BarButtonItem bBI_Sorting;
        private DevExpress.XtraBars.BarButtonItem bBI_Skins;
        private DevExpress.XtraBars.BarButtonItem bBI_Settings;
        private DevExpress.XtraBars.BarButtonItem bBI_PZO;
        private DevExpress.XtraBars.BarButtonItem bBI_SUP;
        private DevExpress.XtraBars.BarButtonItem bBI_Metrolog;
        private DevExpress.XtraBars.BarButtonItem bBI_Exit;
        private DevExpress.XtraBars.BarSubItem bSI_ReferenceMenu;
        private DevExpress.XtraBars.BarButtonItem bBI_Reference;
        private DevExpress.XtraBars.BarButtonItem bBI_About;
        private DevExpress.XtraBars.BarButtonItem bBI_Calendar;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl3;
    }
}

