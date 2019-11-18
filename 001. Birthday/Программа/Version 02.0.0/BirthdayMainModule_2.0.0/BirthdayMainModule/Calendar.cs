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

namespace Birthday
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

        /// <summary>
        /// Метод загрузки данных календаря.
        /// </summary>
        /// <param name="users">Количество сотрудников.</param>
        /// <param name="dt">Массив дат рождений сотрудников.</param>
        /// <param name="name">Массив имён сотрудников.</param>
        public void addUsersToCalendar(int users, DateTime[] dt, string[] name, int[] age)
        {
            schedulerControl.Start = DateTime.Today;  // начальное положение календаря

            // разрешить вывод календаря только в виде TimelineView:
            schedulerControl.FullWeekView.Enabled = false;
            schedulerControl.DayView.Enabled = false;
            schedulerControl.WeekView.Enabled = false;
            schedulerControl.MonthView.Enabled = false;
            schedulerControl.WorkWeekView.Enabled = false;
            schedulerControl.TimelineView.Enabled = true;
            schedulerControl.GanttView.Enabled = false;

            // разрешить изменение высоты событий календаря:
            schedulerControl.TimelineView.AppointmentDisplayOptions.AppointmentAutoHeight = true;

            // запретить редактировать календарь:
            schedulerControl.OptionsCustomization.AllowAppointmentCreate = UsedAppointmentType.None;
            schedulerControl.OptionsCustomization.AllowAppointmentEdit = UsedAppointmentType.None;
            schedulerControl.OptionsCustomization.AllowAppointmentDelete = UsedAppointmentType.None;

            // создать массив событий:
            Appointment[] apt = new Appointment[users];

            // заполнить массив событий:
            for (int i = 0; i < users; i++)
            {
                // создать новое событие:
                apt[i] = schedulerStorage.CreateAppointment(AppointmentType.Normal, dt[i], dt[i], name[i]);
                apt[i].AllDay = true;                        // на весь день
                apt[i].LabelKey = 3;                         // цвет - зелёный
                apt[i].Location = Convert.ToString(age[i]);  // возраст
                schedulerStorage.Appointments.Add(apt[i]);   // добавить в календарь
            }
        }

        public void addHolidaysToCalendar(int counter, DateTime[] dt, string[] name)
        {
            schedulerControl.Start = DateTime.Today;  // начальное положение календаря

            // разрешить вывод календаря только в виде TimelineView:
            schedulerControl.FullWeekView.Enabled = false;
            schedulerControl.DayView.Enabled = false;
            schedulerControl.WeekView.Enabled = false;
            schedulerControl.MonthView.Enabled = false;
            schedulerControl.WorkWeekView.Enabled = false;
            schedulerControl.TimelineView.Enabled = true;
            schedulerControl.GanttView.Enabled = false;

            // разрешить изменение высоты событий календаря:
            schedulerControl.TimelineView.AppointmentDisplayOptions.AppointmentAutoHeight = true;

            // запретить редактировать календарь:
            schedulerControl.OptionsCustomization.AllowAppointmentCreate = UsedAppointmentType.None;
            schedulerControl.OptionsCustomization.AllowAppointmentEdit = UsedAppointmentType.None;
            schedulerControl.OptionsCustomization.AllowAppointmentDelete = UsedAppointmentType.None;

            // создать массив событий:
            Appointment[] apt = new Appointment[counter];

            // заполнить массив событий:
            for (int i = 0; i < counter; i++)
            {
                // создать новое событие:
                apt[i] = schedulerStorage.CreateAppointment(AppointmentType.Normal, dt[i], dt[i], name[i]);
                apt[i].AllDay = true;                        // на весь день
                apt[i].LabelKey = 1;                         // цвет - красный
                schedulerStorage.Appointments.Add(apt[i]);   // добавить в календарь
            }
        }
    }
}