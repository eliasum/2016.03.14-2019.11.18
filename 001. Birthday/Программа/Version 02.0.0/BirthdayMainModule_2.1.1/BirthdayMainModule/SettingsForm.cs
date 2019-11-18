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
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            chE_debugMode.Checked = Settings.Default.debugMode;  // загрузить настройки в chE_debugMode
        }

        private void sb_OK_Click(object sender, EventArgs e)
        {
            MainForm.f_debugMode = chE_debugMode.Checked;        // записать chE_debugMode в f_debugMode

            Settings.Default.debugMode = chE_debugMode.Checked;  // записать chE_debugMode в настройки
            Settings.Default.Save();                             // сохранить настройки

            Close();                                             // закрыть форму
        }

        private void sB_Cancel_Click(object sender, EventArgs e)
        {
            Close();                                             // закрыть форму
        }
    }
}
