using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;

namespace BirthdayVersionTracking
{
    public partial class MainForm : Form
    {
        private object Label1;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Activated(object sender, EventArgs e)  // активизирует форму и присваивает ей фокус
        {
            SetAutorunValue(true);                                   // добавить в автозагрузку
        }

        public bool SetAutorunValue(bool autorun)                    // установка/сброс значения строкового параметра ключа реестра
        {
            const string name = "Birthday";
            string ExePath = System.Windows.Forms.Application.ExecutablePath;  // путь к исполняемому файлу, включая исполняемое имя

            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");  // открытие существующего вложенного раздела с доступом на запись
            /*
            HKEY_CURRENT_USER - раздел реестра в котором находятся настройки параметров
            среды текущего пользователя(сеанса).
            На первом уровне расположены ветви (Hive Keys). На втором уровне располагаются
            разделы или ключи реестра (Registry Keys), на третьем – подразделы (Subkeys) и
            на четвертом и далее – параметры (Values).
            */
            try
            {
                if (autorun)
                {
                    if (reg.GetValue(name) == null)   // проверка отсутствия в автозагрузке
                        reg.SetValue(name, ExePath);  // записать "Birthday" типа REG_SZ — текстовая строка
                }
                else
                {
                    if (reg.GetValue(name) != null)   // проверка наличия в автозагрузке
                        reg.DeleteValue(name);        // удалить "Birthday" типа REG_SZ — текстовая строка
                }
              reg.Close();                            // закрытие вложенного раздела
            }

            catch
            {
              return false;
            }

            return true;
        }

        private void B_Delete_Click(object sender, EventArgs e)
        {
            SetAutorunValue(false);                   // удалить из автозагрузки
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            label1.Text = strVersion;

            string strMajor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
            string strMinor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
            string strBuild = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
            string strRevision = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString();

            textBox1.Text = strMajor + Environment.NewLine + strMinor + Environment.NewLine + strBuild + Environment.NewLine + strRevision;

        }
    }
}
