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
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
/// <summary>
/// Название: BirthdayVersionTracking - модуль слежения за версиями программы Birthday (день рождения).
/// Дата начала разработки 14.03.2016 г.
/// Автор - Минин Илья Михайлович.
/// Исходные данные:
/// 
/// Небольшой модуль слежения за версиями.
/// При запуске проверяет себя в автозагрузке. Если нет - прописывает себя туда.
/// Сравнивает версии файлов программы на сервере и в локальной папке пользователя. Если на сервере новее - 
/// закачиваем и изменяем.
/// Запускаем основной модуль, закрываем себя.
/// 
/// Основной модуль.
/// Пытается провести логин пользователя. В случае ошибки пытается сделать фиксы системы и повторить
/// попытку.
/// Получает информацию о сотрудниках и подразделениях.
/// Строит список сотрудников (в идеале предусмотреть настройку, чтобы построение было с учетом
/// подразделений и без него.
/// При клике на сотрудника появляется окно с информацией о нём.
/// Парсит даты рождения сотрудников, выводит их в календарь. Предупреждает пользователя о предстоящих 
/// днях рождения + праздниках всплывающим окном.
/// Программа висит в трее, правым кликом по иконке вызывается меню с пунктами:
/// - календарь
/// - ПЗУ, СУП, Метролог (открывает гугл хром с соответствующей страницей).
/// (проверить, возможно ли такое сделать впринципе)
/// Подумать о возможности расширения программы до полноценного мессенджера.
/// </summary>
namespace BirthdayVersionTracking
{
    public partial class MainForm : Form
    {
        private object Label1;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void b_Load_Click(object sender, EventArgs e)
        {
            //Thread.Sleep(60000);                                     // задержка 60 с
            if (Internet.CheckConnection())                          // проверка соединения с сетью
            {
                SetAutorunValue(true);                               // добавить в автозагрузку          
                VersionTracker();                                    // отследить версию
            }
            else
            {
                MessageBox.Show("Отсутствует подключение к сети!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // проверка наличия исполняемого файла в текущей папке:
                if (!File.Exists("BirthdayMainModule.exe"))
                {
                    MessageBox.Show("Исполняемый файл не найден!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Environment.Exit(0);
                }
                else
                {
                    Process.Start("BirthdayMainModule.exe");
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Установка/сброс значения строкового параметра ключа реестра.
        /// </summary>
        /// <param name="autorun">Параметр установки/сброса автозагрузки.
        /// autorun=true - установить, autorun=false - сбросить.</param>
        /// <returns>Возвращает результат выполнения метода - true или false.</returns>
        public bool SetAutorunValue(bool autorun)                    
        {
            const string name = "Birthday";
            string ExePath = Application.ExecutablePath;  // путь к исполняемому файлу, включая исполняемое имя

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

        /// <summary>
        /// Класс сравнения версий программ
        /// </summary>
        public class VersionChecker  
        {
            /// <summary>
            /// Метод проверки существования новой версии программы.
            /// </summary>
            /// <param name="localVersion">Версия на локальной машине.</param>
            /// <param name="versionFromServer">Версия на сервере.</param>
            /// <returns>Возвращает true, если версия на сервере новее, иначе false.</returns>
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

        WebClient client = new WebClient();
        // Путь к папке с программой BirthdayMainModule.exe:
        string MyDirectory = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Метод слежения за версиями.
        /// </summary>
        public void VersionTracker()  
        {
            // проверка наличия исполняемого файла в текущей папке:
            if (!File.Exists("BirthdayMainModule.exe"))
            {
                //MessageBox.Show("Исполняемый файл не найден!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                DownloadFromServer();  // Скачать актуальную версию программы на компьютер
            }

            // Проверка запуска ещё одного экземпляра BirthdayMainModule:
            if (Process.GetProcessesByName("BirthdayMainModule").Length != 0)
            {
                //MessageBox.Show("Другой экземпляр открыт!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Information);
                {
                    foreach (Process currentProcess in Process.GetProcessesByName("BirthdayMainModule"))
                        currentProcess.Kill();
                }
            }

            // Путь к файлу version.txt с текущей версией, который на локальной машине:
            string localVersionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "version.txt");
            /*
            string Path.Combine() объединяет две строки AppDomain.CurrentDomain.BaseDirectory 
            и "version.txt" в путь.
            */

            // скачать актуальную версию в переменную string ServerVersion: 
            string ServerVersion = client.DownloadString("File://192.168.213.51/www/api/app/version.txt");

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
                        //MessageBox.Show("У вас устаревшая версия!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DownloadFromServer();  // Скачать актуальную версию программы на компьютер
                    }
                    else
                    {
                        //MessageBox.Show("У вас самая последняя версия!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start("BirthdayMainModule.exe");                                       // запустить BirthdayMainModule.exe
                        Environment.Exit(0);                                                           // закрыть программу
                    }
                }
                else
                {
                    //MessageBox.Show("Номер версии не найден!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    DownloadFromServer();      // Скачать актуальную версию программы на компьютер
                }
            }
            else
            {
                //MessageBox.Show("Файл версии не найден!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                DownloadFromServer();          // Скачать актуальную версию программы на компьютер
            }
        }

        /// <summary>
        /// Скачать актуальную версию программы на компьютер.
        /// </summary>
        public void DownloadFromServer()
        {
            Uri ui = new Uri("file://192.168.213.51/www/api/app/BirthdayMainModule.exe");  // перезаписать исполняемый 
            client.DownloadFile(ui, MyDirectory + "\\BirthdayMainModule.exe");             // модуль BirthdayMainModule.exe

            ui = new Uri("file://192.168.213.51/www/api/app/version.txt");                 // перезаписать файл
            client.DownloadFile(ui, MyDirectory + "\\version.txt");                        // версии
            client.Dispose();

            Process.Start("BirthdayMainModule");                                           // запустить BirthdayMainModule.exe
            Environment.Exit(0);
        }

        /// <summary>
        /// Класс для проверки соединения с интернетом.
        /// </summary>
        public static class Internet
        {
            [DllImport("wininet.dll")]
            static extern bool InternetGetConnectedState(ref InternetConnectionState lpdwFlags, int dwReserved);

            [Flags]
            enum InternetConnectionState : int
            {
                INTERNET_CONNECTION_MODEM = 0x1,
                INTERNET_CONNECTION_LAN = 0x2,
                INTERNET_CONNECTION_PROXY = 0x4,
                INTERNET_RAS_INSTALLED = 0x10,
                INTERNET_CONNECTION_OFFLINE = 0x20,
                INTERNET_CONNECTION_CONFIGURED = 0x40
            }

            static object _syncObj = new object();

            /// <summary>
            /// Проверить, есть ли соединение с интернетом.
            /// </summary>
            /// <returns></returns>
            public static Boolean CheckConnection()
            {
                lock (_syncObj)
                {
                    try
                    {
                        InternetConnectionState flags = InternetConnectionState.INTERNET_CONNECTION_CONFIGURED | 0;
                        bool checkStatus = InternetGetConnectedState(ref flags, 0);

                        if (checkStatus)
                            return PingServer(new string[]
                                                {
                                                @"google.com",
                                                @"microsoft.com",
                                                @"file://192.168.213.51/www/api/app/"
                                                });
                        return checkStatus;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            /// <summary>
            /// Пингует сервера, при первом получении ответа от любого сервера возвращает true 
            /// </summary>
            /// <param name="serverList">Список серверов</param>
            /// <returns></returns>
            public static bool PingServer(string[] serverList)
            {
                bool haveAnInternetConnection = false;
                Ping ping = new Ping();
                for (int i = 0; i < serverList.Length; i++)
                {
                    PingReply pingReply = ping.Send(serverList[i]);
                    haveAnInternetConnection = (pingReply.Status == IPStatus.Success);
                    if (haveAnInternetConnection)
                        break;
                }

                return haveAnInternetConnection;
            }
        }
    }
}