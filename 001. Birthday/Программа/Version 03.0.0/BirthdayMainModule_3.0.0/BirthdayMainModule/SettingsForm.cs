using Birthday.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Birthday
{
    public partial class SettingsForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public SettingsForm()
        {
            InitializeComponent();

            if (MainForm.F_debugEn)
                chE_debugMode.Visible = true;
            else chE_debugMode.Visible = false;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            chE_debugMode.Checked = Settings.Default.debugMode;      // загрузить настройки
            chE_Birthday.Checked = Settings.Default.Birthday;
            chE_Holiday.Checked = Settings.Default.Holiday;
            cBE_Days.SelectedIndex = Settings.Default.Dayss-1;
            cBE_Hours.SelectedIndex = Settings.Default.Hourss-1;
        }

        private void sb_OK_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.F_debugMode = chE_debugMode.Checked;        // записать флаги
                MainForm.F_birthday = chE_Birthday.Checked;
                MainForm.F_holiday = chE_Holiday.Checked;
                MainForm.F_days = cBE_Days.SelectedIndex+1;
                MainForm.F_hours = cBE_Hours.SelectedIndex+1;

                Settings.Default.debugMode = chE_debugMode.Checked;  // записать настройки
                Settings.Default.Birthday = chE_Birthday.Checked;
                Settings.Default.Holiday = chE_Holiday.Checked;
                Settings.Default.Dayss = cBE_Days.SelectedIndex+1;
                Settings.Default.Hourss = cBE_Hours.SelectedIndex+1;

                Settings.Default.Save();                             // сохранить настройки

                MainForm.CloseRemindForm();
            }
            catch (Exception ex)
            {
                if (MainForm.F_debugMode) MessageBox.Show("sb_OK_Click: " + ex.Message);
                MainForm.Log.Write(ex);
            }

            Close();                                                 // закрыть форму
        }

        private void sB_Cancel_Click(object sender, EventArgs e)
        {
            Close();                                                 // закрыть форму
        }
    }
}
