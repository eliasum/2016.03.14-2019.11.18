namespace Birthday
{
    partial class CalendarForm
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
            DevExpress.XtraScheduler.TimeRuler timeRuler1 = new DevExpress.XtraScheduler.TimeRuler();
            DevExpress.XtraScheduler.TimeRuler timeRuler2 = new DevExpress.XtraScheduler.TimeRuler();
            DevExpress.XtraScheduler.TimeScaleYear timeScaleYear1 = new DevExpress.XtraScheduler.TimeScaleYear();
            DevExpress.XtraScheduler.TimeScaleQuarter timeScaleQuarter1 = new DevExpress.XtraScheduler.TimeScaleQuarter();
            DevExpress.XtraScheduler.TimeScaleMonth timeScaleMonth1 = new DevExpress.XtraScheduler.TimeScaleMonth();
            DevExpress.XtraScheduler.TimeScaleWeek timeScaleWeek1 = new DevExpress.XtraScheduler.TimeScaleWeek();
            DevExpress.XtraScheduler.TimeScaleDay timeScaleDay1 = new DevExpress.XtraScheduler.TimeScaleDay();
            DevExpress.XtraScheduler.TimeScaleHour timeScaleHour1 = new DevExpress.XtraScheduler.TimeScaleHour();
            DevExpress.XtraScheduler.TimeScale15Minutes timeScale15Minutes1 = new DevExpress.XtraScheduler.TimeScale15Minutes();
            DevExpress.XtraScheduler.TimeRuler timeRuler3 = new DevExpress.XtraScheduler.TimeRuler();
            this.dateNavigator1 = new DevExpress.XtraScheduler.DateNavigator();
            this.schedulerControl = new DevExpress.XtraScheduler.SchedulerControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.schedulerStorage = new DevExpress.XtraScheduler.SchedulerStorage();
            this.sB_OK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.dateNavigator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateNavigator1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage)).BeginInit();
            this.SuspendLayout();
            // 
            // dateNavigator1
            // 
            this.dateNavigator1.CalendarAppearance.DayCellSpecial.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.dateNavigator1.CalendarAppearance.DayCellSpecial.Options.UseFont = true;
            this.dateNavigator1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateNavigator1.DateTime = new System.DateTime(2016, 4, 18, 0, 0, 0, 0);
            this.dateNavigator1.EditValue = new System.DateTime(2016, 4, 18, 0, 0, 0, 0);
            this.dateNavigator1.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dateNavigator1.Location = new System.Drawing.Point(1099, 12);
            this.dateNavigator1.Name = "dateNavigator1";
            this.dateNavigator1.SchedulerControl = this.schedulerControl;
            this.dateNavigator1.Size = new System.Drawing.Size(213, 243);
            this.dateNavigator1.TabIndex = 17;
            // 
            // schedulerControl
            // 
            this.schedulerControl.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Timeline;
            this.schedulerControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.schedulerControl.GroupType = DevExpress.XtraScheduler.SchedulerGroupType.Date;
            this.schedulerControl.Location = new System.Drawing.Point(12, 12);
            this.schedulerControl.MenuManager = this.barManager1;
            this.schedulerControl.Name = "schedulerControl";
            this.schedulerControl.Size = new System.Drawing.Size(1081, 374);
            this.schedulerControl.Start = new System.DateTime(2016, 4, 1, 0, 0, 0, 0);
            this.schedulerControl.Storage = this.schedulerStorage;
            this.schedulerControl.TabIndex = 16;
            this.schedulerControl.Text = "schedulerControl1";
            this.schedulerControl.Views.DayView.NavigationButtonVisibility = DevExpress.XtraScheduler.NavigationButtonVisibility.Never;
            this.schedulerControl.Views.DayView.TimeRulers.Add(timeRuler1);
            this.schedulerControl.Views.FullWeekView.Enabled = true;
            this.schedulerControl.Views.FullWeekView.TimeRulers.Add(timeRuler2);
            this.schedulerControl.Views.MonthView.MenuCaption = "&Месяц";
            this.schedulerControl.Views.MonthView.NavigationButtonVisibility = DevExpress.XtraScheduler.NavigationButtonVisibility.Never;
            this.schedulerControl.Views.TimelineView.NavigationButtonVisibility = DevExpress.XtraScheduler.NavigationButtonVisibility.Never;
            timeScaleYear1.Enabled = false;
            timeScaleQuarter1.Enabled = false;
            timeScaleMonth1.DisplayFormat = "MMMM, yyyy";
            timeScaleMonth1.MenuCaption = "&Месяц, &Год";
            timeScaleWeek1.Enabled = false;
            timeScaleDay1.DisplayFormat = "dd, dddd ";
            timeScaleDay1.Width = 180;
            timeScaleHour1.Enabled = false;
            timeScale15Minutes1.Enabled = false;
            this.schedulerControl.Views.TimelineView.Scales.Add(timeScaleYear1);
            this.schedulerControl.Views.TimelineView.Scales.Add(timeScaleQuarter1);
            this.schedulerControl.Views.TimelineView.Scales.Add(timeScaleMonth1);
            this.schedulerControl.Views.TimelineView.Scales.Add(timeScaleWeek1);
            this.schedulerControl.Views.TimelineView.Scales.Add(timeScaleDay1);
            this.schedulerControl.Views.TimelineView.Scales.Add(timeScaleHour1);
            this.schedulerControl.Views.TimelineView.Scales.Add(timeScale15Minutes1);
            this.schedulerControl.Views.TimelineView.SelectionBar.Visible = false;
            this.schedulerControl.Views.TimelineView.TimeIndicatorDisplayOptions.Visibility = DevExpress.XtraScheduler.TimeIndicatorVisibility.Never;
            this.schedulerControl.Views.WorkWeekView.DayCount = 5;
            this.schedulerControl.Views.WorkWeekView.TimeRulers.Add(timeRuler3);
            // 
            // barManager1
            // 
            this.barManager1.AllowItemAnimatedHighlighting = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.CloseButtonAffectAllTabs = false;
            this.barManager1.DockingEnabled = false;
            this.barManager1.Form = this;
            this.barManager1.MaxItemId = 0;
            // 
            // sB_OK
            // 
            this.sB_OK.Location = new System.Drawing.Point(1237, 364);
            this.sB_OK.Name = "sB_OK";
            this.sB_OK.Size = new System.Drawing.Size(75, 23);
            this.sB_OK.TabIndex = 18;
            this.sB_OK.Text = "OK";
            this.sB_OK.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // CalendarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 399);
            this.Controls.Add(this.sB_OK);
            this.Controls.Add(this.dateNavigator1);
            this.Controls.Add(this.schedulerControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalendarForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Календарь";
            ((System.ComponentModel.ISupportInitialize)(this.dateNavigator1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateNavigator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraScheduler.DateNavigator dateNavigator1;
        private DevExpress.XtraScheduler.SchedulerControl schedulerControl;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraScheduler.SchedulerStorage schedulerStorage;
        private DevExpress.XtraEditors.SimpleButton sB_OK;
    }
}