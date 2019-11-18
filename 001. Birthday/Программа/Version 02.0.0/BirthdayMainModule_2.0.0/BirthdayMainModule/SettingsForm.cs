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
    public partial class SettingsForm : Form
    {
        bool f_cancel = false;   // флаг выхода без сохранения
        bool f_change = false;   // флаг изменения настроек

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void chB_debugMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_debugMode.Checked == true)
                MainForm.f_saveDebug = true;
            else
                MainForm.f_saveDebug = false;

            f_change = true;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            chB_debugMode.Checked = MainForm.f_saveDebug;  // при открытии формы сделать копию текущего значения элемента
            f_cancel = false;   // по умолчанию  
            f_change = false;   // по умолчанию
        }

        private void b_OK_Click(object sender, EventArgs e)
        {
            f_cancel = false;
            Close();            // закрыть форму
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            f_cancel = true;    // нажат cancel
            Close();            // закрыть форму
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (f_cancel && f_change)       // если была нажата кнопка Сancel и было изменение
            {
                chB_debugMode.Checked = !MainForm.f_saveDebug;  // восстановить предыдущие значения элементов
                f_change = false;
            }
        }
    }
}
