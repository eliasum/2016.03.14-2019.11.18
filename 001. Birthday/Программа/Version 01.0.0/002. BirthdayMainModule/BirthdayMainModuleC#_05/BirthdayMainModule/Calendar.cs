using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraScheduler;

namespace BirthdayMainModule
{
    public partial class CalendarForm : Form
    {
        public CalendarForm()
        {
            InitializeComponent();
        }

        private void b_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void showCalendar(int users, DateTime[] dt, string[] name)
        {
            schedulerControl.Start = DateTime.Today;

            schedulerControl.FullWeekView.Enabled = false;
            schedulerControl.DayView.Enabled = false;
            schedulerControl.WeekView.Enabled = false;
            schedulerControl.MonthView.Enabled = false;
            schedulerControl.WorkWeekView.Enabled = false;
            schedulerControl.TimelineView.Enabled = true;
            schedulerControl.GanttView.Enabled = false;
            
            Appointment[] apt = new Appointment[users];

            for (int i = 0; i < users; i++)
            {
                apt[i] = schedulerStorage.CreateAppointment(AppointmentType.Normal, dt[i], dt[i], name[i]);
                apt[i].AllDay = true;
                apt[i].LabelKey = 3;
                schedulerStorage.Appointments.Add(apt[i]);
            }
        }
    }
}