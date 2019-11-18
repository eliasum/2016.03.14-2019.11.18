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
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.IO;

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
            VersionTracker();                                        // отследить версию
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
                reg.Close();                          // закрытие вложенного раздела
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

        private void b_Version_Click(object sender, EventArgs e)
        {
            string strVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            label1.Text = strVersion;

            string strMajor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
            string strMinor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
            string strBuild = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
            string strRevision = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString();

            textBox1.Text = strMajor + Environment.NewLine + strMinor + Environment.NewLine + strBuild + Environment.NewLine + strRevision;
        }

        public class VersionChecker  // Класс сравнения версий программ
        {
            public bool NewVersionExists(string localVersion, string versionFromServer)
            {
                Version verLocal = new Version(localVersion);
                Version verWeb = new Version(versionFromServer);
                return verLocal < verWeb;
            }
            /*
		    public Version(string version) - Инициализирует новый экземпляр класса Version,
		    используя указанную строку.
		    Параметры:
            version - Строка, содержащая основной и дополнительный номера версии, 
		    номер сборки и номер редакции, в которой каждое число отделено точкой (.).
		    */
        }

        public void VersionTracker()  // метод слежения за версиями
        {
            WebClient client = new WebClient();

            // Путь к файлу version.txt с текущей версией, который на локальной машине:
            string localVersionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "version.txt");
            /*
            string Path.Combine() объединяет две строки AppDomain.CurrentDomain.BaseDirectory 
            и "version.txt" в путь.
            */
            // скачать актуальную версию в переменную string ServerVersion: 
            string ServerVersion = client.DownloadString("file://192.168.213.51/www/api/app/version.txt");

            // Путь к папке с программой BirthdayMainModule.exe:
            string MyDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // проверка наличия исполняемого файла в текущей папке:
            if (!File.Exists("BirthdayMainModule.exe"))
            {
                Uri ui = new Uri("file://192.168.213.51/www/api/app/BirthdayMainModule.exe");
                client.DownloadFile(ui, MyDirectory + "\\BirthdayMainModule.exe");

                ui = new Uri("file://192.168.213.51/www/api/app/version.txt");
                client.DownloadFile(ui, MyDirectory + "\\version.txt");
            }

            // алгоритм получения версии файла на локальной машине:
            string localVersion = null;

            if (File.Exists(localVersionFile))                            // если файл version.txt существует
            {
                using (StreamReader sr = File.OpenText(localVersionFile)) // открываем файл для чтения
                {
                    localVersion = sr.ReadLine();                         // Предположим у тебя одна строка в файле
                }

                if (!string.IsNullOrWhiteSpace(localVersion))             // если считанная строка что то содержит
                {
                    // Сравнение версий файлов:
                    VersionChecker verChecker = new VersionChecker();

                    if (verChecker.NewVersionExists(localVersion, ServerVersion))
                    {
                        /*
                        // диалог с пользователем:
                        DialogResult dial = MessageBox.Show("Доступна новая версия программы, обновить?",
                        "Автоматическое обновление", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (dial == DialogResult.Yes)  // если обновить
                        {*/
                            Uri ui = new Uri("file://192.168.213.51/www/api/app/BirthdayMainModule.exe");  // перезаписать исполняемый 
                            client.DownloadFile(ui, MyDirectory + "\\BirthdayMainModule.exe");             // модуль BirthdayMainModule.exe

                            ui = new Uri("file://192.168.213.51/www/api/app/version.txt");                 // перезаписать файл
                            client.DownloadFile(ui, MyDirectory + "\\version.txt");                        // версии

                            Process.Start("BirthdayMainModule.exe");                                       // запустить BirthdayMainModule.exe
                            Environment.Exit(0);                                                           // закрыть программу
                        //}
                        client.Dispose();
                    }
                    else
                    {
                        //MessageBox.Show("У вас самая последняя версия!");

                        Process.Start("BirthdayMainModule.exe");                                       // запустить BirthdayMainModule.exe
                        Environment.Exit(0);                                                           // закрыть программу
                    }
                }
                else
                {
                    MessageBox.Show("Номер версии не найден!");
                }
            }
            else
                MessageBox.Show("Файл версии не найден!");
        }
    }
}