using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Threading;
using System.Text;
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
namespace Birthday
{
    public partial class MainForm : Form
    {
        WebClient client = new WebClient();

        // путь к моей директории:
        static string pathToMyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        string pathToMyFile = Path.Combine(pathToMyDirectory, "BirthdayMainModule");   // путь к исполняемому файлу основного модуля
        string pathToMyVersion = pathToMyDirectory + @"\version.txt";                  // путь к файлу версии на компьютере

        static string pathToServerFolder = @"http://api.nccp-eng.ru/app/";             // путь к папке на сервере @"file://192.168.213.51/www/api/app/" @"http://api.nccp-eng.ru/app/"

        string pathToExeOnServer = pathToServerFolder + @"\BirthdayMainModule.exe";    // путь к исполняемому файлу основного модуля на сервере
        string pathToVersionOnServer = pathToServerFolder + @"\version.txt";           // путь к файлу версии на сервере

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetAutorunValue(true);                                   // добавить в автозагрузку 
/*
            // проверка наличия .NET Framework 4.5.2 на компе:
            Process Process = null;
            string pathChecker = Path.Combine(pathToMyDirectory, "Checker");
            string pathFramework = Path.Combine(pathToMyDirectory, "NDP452-KB2901907-x86-x64-AllOS-ENU");

            try
            {
                Process = Process.Start(pathChecker);                // запустить проверку
                Thread.Sleep(1000);                                  // задержка 300 мс

                if (Process.ExitCode != 777)                         // если проверка неудачная
                {
                    try
                    {
                        MessageBox.Show(".NET Framework 4.5.2 не найден. Будет произведена установка .NET Framework 4.5.2.", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Process.Start(pathFramework);                // запустить установку
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Файл установки .NET Framework 4.5.2 не найден. Программа будет закрыта.", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Log.Write(ex);
                    }

                    Environment.Exit(0);                             // закрыть программу 
                }
            }
            catch                                                    // если Checker.exe потерян
            {
                try
                {
                    MessageBox.Show(".NET Framework 4.5.2 не найден. Будет произведена установка .NET Framework 4.5.2.", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Process.Start(pathFramework);                    // запустить установку
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Файл установки .NET Framework 4.5.2 не найден. Программа будет закрыта.", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Log.Write(ex);
                }

                Environment.Exit(0);                                 // закрыть программу 
            }
*/
            // проверка соединения с сетью:
            if (Internet.CheckConnection())                          // подключение есть       
            {
                VersionTracker();                                    // отследить версию
            }
            else                                                     // подключение отсутствует   
            {
                //MessageBox.Show("Отсутствует подключение к сети!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // проверка наличия исполняемого файла в текущей папке:
                if (!File.Exists("BirthdayMainModule.exe"))          // если файл не существует
                {
                    MessageBox.Show("Исполняемый файл не найден!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Environment.Exit(0);                             // закрыть программу
                }
                else
                {
                    Process.Start(pathToMyFile);                     // запустить BirthdayMainModule.exe
                    Environment.Exit(0);                             // закрыть программу
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
            
            // открытие существующего вложенного раздела с доступом на запись:
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\");
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

            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
            
            /*
            try
            {
                if (autorun)
                {
                    Microsoft.Win32.RegistryKey Key =
                        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);
                    Key.SetValue(name, ExePath);
                    Key.Close();
                }
                else
                {
                    Microsoft.Win32.RegistryKey key =
                        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    key.DeleteValue(name, false);
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
            */
            return true;
        }

        private void B_Delete_Click(object sender, EventArgs e)
        {
            SetAutorunValue(false);                   // удалить из автозагрузки
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

        /// <summary>
        /// Метод скачки актуальной версии программы на компьютер.
        /// </summary>
        public void DownloadFromServer()
        {
            try
            {
                Uri uri1 = new Uri(pathToExeOnServer);                                      // перезаписать исполняемый 
                client.DownloadFile(uri1, pathToMyFile);                                    // модуль BirthdayMainModule.exe

                Uri uri2 = new Uri(pathToVersionOnServer);                                  // перезаписать файл
                client.DownloadFile(uri2, pathToMyVersion);                                 // версии

                client.Dispose();                                                           // отключить вэбклиента
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            try
            {
                Process.Start(pathToMyFile);                                                // запустить BirthdayMainModule.exe
            }
            catch (Exception ex)
            {
                MessageBox.Show("Отсутствует файл BirthdayMainModule.exe. Программа будет закрыта.", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Log.Write(ex);
            }

            Environment.Exit(0);                                                            // закрыть программу
        } 

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
                        
            // скачать актуальную версию в переменную string ServerVersion: 
            string ServerVersion = null;
            try
            {
                ServerVersion = client.DownloadString(pathToVersionOnServer);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Номер версии на сервере не найден. Программа будет закрыта.", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Log.Write(ex);
                Environment.Exit(0);                                      // закрыть программу
            }

            // алгоритм получения версии файла на локальной машине:
            string localVersion = null;
            if (File.Exists(pathToMyVersion))                             // если файл version.txt существует
            {
                using (StreamReader sr = File.OpenText(pathToMyVersion))  // открываем файл для чтения
                {
                    localVersion = sr.ReadLine();                         // Предположим у тебя одна строка в файле
                }

                if (localVersion != null)                                 // если считанная строка что то содержит
                {
                    // Сравнение версий файлов:
                    VersionChecker verChecker = new VersionChecker();

                    if (verChecker.NewVersionExists(localVersion, ServerVersion))
                    {
                        //MessageBox.Show("У вас устаревшая версия!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DownloadFromServer();         // Скачать актуальную версию программы на компьютер
                    }
                    else
                    {
                        //MessageBox.Show("У вас самая последняя версия!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start(pathToMyFile);  // запустить BirthdayMainModule.exe
                        Environment.Exit(0);          // закрыть программу
                    }
                }
                else
                {
                    //MessageBox.Show("Номер версии не найден!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    DownloadFromServer();             // Скачать актуальную версию программы на компьютер
                }
            }
            else
            {
                //MessageBox.Show("Файл версии не найден!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                DownloadFromServer();                 // Скачать актуальную версию программы на компьютер
            }
        }

        public static bool CheckConnection_()
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send("www.cyberforum.ru");
            if (reply.Status != IPStatus.Success)
            {
                MessageBox.Show("облом");
                return false;
            }
            else return true;
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
                                                @"google.ru",
                                                @"microsoft.com/ru-ru/",
                                                @"www.cyberforum.ru"
                                                });
                        return checkStatus;
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
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

        public class Log
        {
            private static object sync = new object();
            public static void Write(Exception ex)
            {
                try
                {
                    // Путь .\\Log
                    string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                    if (!Directory.Exists(pathToLog))
                        Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
                    string filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log",
                    AppDomain.CurrentDomain.FriendlyName, DateTime.Now));
                    string fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{1}.{2}()] {3}\r\n",
                    DateTime.Now, ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message);
                    lock (sync)
                    {
                        File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
                    }
                }
                catch
                {
                    // Перехватываем все и ничего не делаем
                }
            }
        }
    }
}