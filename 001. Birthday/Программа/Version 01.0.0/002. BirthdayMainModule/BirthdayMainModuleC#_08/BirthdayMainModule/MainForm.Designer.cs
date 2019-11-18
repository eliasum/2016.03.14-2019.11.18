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
            DevExpress.Utils.Animation.PushTransition pushTransition1 = new DevExpress.Utils.Animation.PushTransition();
            this.l_UserName = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.workspaceManager1 = new DevExpress.Utils.WorkspaceManager();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.bSI_Action = new DevExpress.XtraBars.BarSubItem();
            this.bBI_MinimizeToTray = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_Settings = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_Calendar = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_PZO = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_SUP = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_Metrolog = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_Reference = new DevExpress.XtraBars.BarButtonItem();
            this.bBI_About = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.barMdiChildrenListItem1 = new DevExpress.XtraBars.BarMdiChildrenListItem();
            this.barMdiChildrenListItem2 = new DevExpress.XtraBars.BarMdiChildrenListItem();
            this.barWorkspaceMenuItem1 = new DevExpress.XtraBars.BarWorkspaceMenuItem();
            this.barDockingMenuItem1 = new DevExpress.XtraBars.BarDockingMenuItem();
            this.barDockingMenuItem2 = new DevExpress.XtraBars.BarDockingMenuItem();
            this.barDockingMenuItem3 = new DevExpress.XtraBars.BarDockingMenuItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
            this.bBI_Sorting = new DevExpress.XtraBars.BarButtonItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.свернутьразвернутьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.календарьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пЗОToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сУПToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.метрологToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.tB_Search = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // l_UserName
            // 
            this.l_UserName.AutoSize = true;
            this.l_UserName.Location = new System.Drawing.Point(12, 28);
            this.l_UserName.Name = "l_UserName";
            this.l_UserName.Size = new System.Drawing.Size(115, 13);
            this.l_UserName.TabIndex = 7;
            this.l_UserName.Text = "Здравствуйте, Гость!";
            // 
            // treeView1
            // 
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(15, 70);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 2;
            this.treeView1.Size = new System.Drawing.Size(358, 374);
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
            // workspaceManager1
            // 
            this.workspaceManager1.TargetControl = this;
            this.workspaceManager1.TransitionType = pushTransition1;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.bSI_Action,
            this.barSubItem2,
            this.barSubItem3,
            this.barMdiChildrenListItem1,
            this.barMdiChildrenListItem2,
            this.barWorkspaceMenuItem1,
            this.barDockingMenuItem1,
            this.barDockingMenuItem2,
            this.barDockingMenuItem3,
            this.bBI_MinimizeToTray,
            this.barStaticItem1,
            this.barHeaderItem1,
            this.barStaticItem2,
            this.barSubItem4,
            this.bBI_Settings,
            this.bBI_Calendar,
            this.bBI_Reference,
            this.bBI_About,
            this.bBI_PZO,
            this.bBI_SUP,
            this.bBI_Metrolog,
            this.bBI_Sorting});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 24;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(407, 25);
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.bSI_Action);
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.bBI_Sorting);
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            // 
            // bSI_Action
            // 
            this.bSI_Action.Caption = "Действие";
            this.bSI_Action.Glyph = ((System.Drawing.Image)(resources.GetObject("bSI_Action.Glyph")));
            this.bSI_Action.Id = 1;
            this.bSI_Action.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_MinimizeToTray),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Settings),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Calendar),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_PZO),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_SUP),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Metrolog),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Reference),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_About)});
            this.bSI_Action.Name = "bSI_Action";
            // 
            // bBI_MinimizeToTray
            // 
            this.bBI_MinimizeToTray.Caption = "Свернуть в трей";
            this.bBI_MinimizeToTray.Id = 11;
            this.bBI_MinimizeToTray.ImageIndex = 0;
            this.bBI_MinimizeToTray.ImageUri.Uri = "SendBackward";
            this.bBI_MinimizeToTray.Name = "bBI_MinimizeToTray";
            this.bBI_MinimizeToTray.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_MinimizeToTray_ItemClick);
            // 
            // bBI_Settings
            // 
            this.bBI_Settings.Caption = "Настройки";
            this.bBI_Settings.Id = 16;
            this.bBI_Settings.ImageUri.Uri = "Customization";
            this.bBI_Settings.Name = "bBI_Settings";
            this.bBI_Settings.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Settings_ItemClick);
            // 
            // bBI_Calendar
            // 
            this.bBI_Calendar.Caption = "Календарь";
            this.bBI_Calendar.Id = 17;
            this.bBI_Calendar.ImageUri.Uri = "Today";
            this.bBI_Calendar.Name = "bBI_Calendar";
            this.bBI_Calendar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Calendar_ItemClick);
            // 
            // bBI_PZO
            // 
            this.bBI_PZO.Caption = "ПЗО";
            this.bBI_PZO.Id = 20;
            this.bBI_PZO.ImageUri.Uri = "Pie";
            this.bBI_PZO.Name = "bBI_PZO";
            this.bBI_PZO.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_PZO_ItemClick);
            // 
            // bBI_SUP
            // 
            this.bBI_SUP.Caption = "СУП";
            this.bBI_SUP.Id = 21;
            this.bBI_SUP.ImageUri.Uri = "Summary";
            this.bBI_SUP.Name = "bBI_SUP";
            this.bBI_SUP.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_SUP_ItemClick);
            // 
            // bBI_Metrolog
            // 
            this.bBI_Metrolog.Caption = "Метролог";
            this.bBI_Metrolog.Id = 22;
            this.bBI_Metrolog.ImageUri.Uri = "Replace";
            this.bBI_Metrolog.Name = "bBI_Metrolog";
            this.bBI_Metrolog.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Metrolog_ItemClick);
            // 
            // bBI_Reference
            // 
            this.bBI_Reference.Caption = "Справка";
            this.bBI_Reference.Id = 18;
            this.bBI_Reference.ImageUri.Uri = "Paste";
            this.bBI_Reference.Name = "bBI_Reference";
            // 
            // bBI_About
            // 
            this.bBI_About.Caption = "О программе";
            this.bBI_About.Id = 19;
            this.bBI_About.ImageUri.Uri = "Home";
            this.bBI_About.Name = "bBI_About";
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "barSubItem";
            this.barSubItem2.Id = 3;
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barSubItem3
            // 
            this.barSubItem3.Caption = "barSubItem3";
            this.barSubItem3.Id = 4;
            this.barSubItem3.Name = "barSubItem3";
            // 
            // barMdiChildrenListItem1
            // 
            this.barMdiChildrenListItem1.Caption = "barMdiChildrenListItem1";
            this.barMdiChildrenListItem1.Id = 5;
            this.barMdiChildrenListItem1.Name = "barMdiChildrenListItem1";
            // 
            // barMdiChildrenListItem2
            // 
            this.barMdiChildrenListItem2.Id = 6;
            this.barMdiChildrenListItem2.Name = "barMdiChildrenListItem2";
            // 
            // barWorkspaceMenuItem1
            // 
            this.barWorkspaceMenuItem1.Caption = "barWorkspaceMenuItem1";
            this.barWorkspaceMenuItem1.Id = 7;
            this.barWorkspaceMenuItem1.Name = "barWorkspaceMenuItem1";
            this.barWorkspaceMenuItem1.WorkspaceManager = this.workspaceManager1;
            // 
            // barDockingMenuItem1
            // 
            this.barDockingMenuItem1.Caption = "barDockingMenuItem1";
            this.barDockingMenuItem1.Id = 8;
            this.barDockingMenuItem1.Name = "barDockingMenuItem1";
            // 
            // barDockingMenuItem2
            // 
            this.barDockingMenuItem2.Caption = "barDockingMenuItem2";
            this.barDockingMenuItem2.Id = 9;
            this.barDockingMenuItem2.Name = "barDockingMenuItem2";
            // 
            // barDockingMenuItem3
            // 
            this.barDockingMenuItem3.Caption = "barDockingMenuItem3";
            this.barDockingMenuItem3.Id = 10;
            this.barDockingMenuItem3.Name = "barDockingMenuItem3";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "-";
            this.barStaticItem1.Id = 12;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "barHeaderItem1";
            this.barHeaderItem1.Id = 13;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "barStaticItem2";
            this.barStaticItem2.Id = 14;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barSubItem4
            // 
            this.barSubItem4.Caption = "barSubItem4";
            this.barSubItem4.Id = 15;
            this.barSubItem4.Name = "barSubItem4";
            // 
            // bBI_Sorting
            // 
            this.bBI_Sorting.Caption = "Изменить сортировку";
            this.bBI_Sorting.Id = 23;
            this.bBI_Sorting.ImageUri.Uri = "SortAsc";
            this.bBI_Sorting.Name = "bBI_Sorting";
            this.bBI_Sorting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBI_Sorting_ItemClick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseMove);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.свернутьразвернутьToolStripMenuItem,
            this.toolStripMenuItem1,
            this.настройкиToolStripMenuItem,
            this.календарьToolStripMenuItem,
            this.пЗОToolStripMenuItem,
            this.сУПToolStripMenuItem,
            this.метрологToolStripMenuItem,
            this.toolStripMenuItem2,
            this.справкаToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(150, 192);
            this.contextMenuStrip1.MouseLeave += new System.EventHandler(this.contextMenuStrip1_MouseLeave);
            // 
            // свернутьразвернутьToolStripMenuItem
            // 
            this.свернутьразвернутьToolStripMenuItem.Name = "свернутьразвернутьToolStripMenuItem";
            this.свернутьразвернутьToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.свернутьразвернутьToolStripMenuItem.Text = "Развернуть";
            this.свернутьразвернутьToolStripMenuItem.Click += new System.EventHandler(this.свернутьразвернутьToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 6);
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
            this.barSubItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barSubItem1.Glyph")));
            this.barSubItem1.Id = 1;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_MinimizeToTray),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Settings),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Calendar),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_PZO),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_SUP),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Metrolog),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_Reference),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBI_About)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // tB_Search
            // 
            this.tB_Search.Location = new System.Drawing.Point(15, 44);
            this.tB_Search.Name = "tB_Search";
            this.tB_Search.Size = new System.Drawing.Size(358, 20);
            this.tB_Search.TabIndex = 11;
            this.tB_Search.Click += new System.EventHandler(this.tB_Search_Click);
            this.tB_Search.TextChanged += new System.EventHandler(this.tB_Search_TextChanged);
            this.tB_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_Search_KeyPress);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 475);
            this.Controls.Add(this.tB_Search);
            this.Controls.Add(this.ribbonControl1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.l_UserName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Birthday";
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate_1);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label l_UserName;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.Utils.WorkspaceManager workspaceManager1;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarSubItem bSI_Action;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private DevExpress.XtraBars.BarMdiChildrenListItem barMdiChildrenListItem1;
        private DevExpress.XtraBars.BarMdiChildrenListItem barMdiChildrenListItem2;
        private DevExpress.XtraBars.BarWorkspaceMenuItem barWorkspaceMenuItem1;
        private DevExpress.XtraBars.BarDockingMenuItem barDockingMenuItem1;
        private DevExpress.XtraBars.BarDockingMenuItem barDockingMenuItem2;
        private DevExpress.XtraBars.BarDockingMenuItem barDockingMenuItem3;
        private DevExpress.XtraBars.BarButtonItem bBI_MinimizeToTray;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarButtonItem bBI_Settings;
        private DevExpress.XtraBars.BarButtonItem bBI_Calendar;
        private DevExpress.XtraBars.BarButtonItem bBI_Reference;
        private DevExpress.XtraBars.BarButtonItem bBI_About;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem4;
        private DevExpress.XtraBars.BarButtonItem bBI_PZO;
        private DevExpress.XtraBars.BarButtonItem bBI_SUP;
        private DevExpress.XtraBars.BarButtonItem bBI_Metrolog;
        private DevExpress.XtraBars.BarButtonItem bBI_Sorting;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem свернутьразвернутьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem календарьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пЗОToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сУПToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem метрологToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.TextBox tB_Search;
    }
}

