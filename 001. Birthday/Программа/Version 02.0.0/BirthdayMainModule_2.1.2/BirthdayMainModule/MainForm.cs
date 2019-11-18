﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Drawing;
using System.Threading;
using Birthday.Properties;
using DevExpress.LookAndFeel;
/// <summary>
/// Название: BirthdayMainModule - основной модуль программы Birthday (день рождения).
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
    /// <summary>
    /// class MainForm - основной класс программы.
    /// </summary>
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public static bool f_debugMode;                    // флаги сохранения настроек 
        public static bool f_debugEn;
        public static bool f_birthday;
        public static bool f_holiday;
        public static int f_days;
        public static int f_hours;

        public MainForm()
        {
            InitializeComponent();
            UserLookAndFeel.Default.SkinName = Settings.Default["ApplicationSkinName"].ToString();
            DevExpress.UserSkins.BonusSkins.Register();    // регистрация дополнительных шкур

            f_debugMode = Settings.Default.debugMode;      // загрузка настроек
            f_holiday = Settings.Default.Holiday;
            f_birthday = Settings.Default.Birthday;        
            f_days = Settings.Default.Dayss;
            f_hours = Settings.Default.Hourss;

            /*
            this.BackColor = ColorTranslator.FromHtml("#bbffba");
            tV_Tree.BackColor = ColorTranslator.FromHtml("#fff7ba");
            tB_Search.BackColor = ColorTranslator.FromHtml("#fff7ba");
            */
        }

        bool f_sorting = true;                             // флаг переключения сортировки
        bool f_netConnect = false;                         // флаг наличия подключения 
        string NewL = Environment.NewLine;                 // новая строка
        string userName = null;                            // имя сотрудника
        string[] strOfEvents = new string[500];            // названия событий
        Color[] strOfColors = new Color[500];              // цвета событий

        const int hour = 3600000;                          // час в миллисекундах
        int notFired = 0;                                  // число неуволенных пользователей
        int teams = 0;                                     // число подразделений 
        int jobs = 0;                                      // число профессий
        int counter = 0;                                   // количество праздников
        int countPass = 0;                                 // количество переходных праздников
        int strOfDobs = 0;                                 // строка наличия событий
        static int events = 0;                             // количество событий
        static int cChristian = 7;                         // количество Христианских праздников

        int thisYear = Convert.ToInt32(DateTime.Now.Year.ToString());

        Dictionary<string, usersGetClass> usersGetArray = new Dictionary<string, usersGetClass>();  // словарь пользователей
        Dictionary<string, teamsGetClass> teamsGetArray = new Dictionary<string, teamsGetClass>();  // словарь подразделений
        Dictionary<string, jobsGetClass> jobsGetArray = new Dictionary<string, jobsGetClass>();     // словарь профессий

        // новый экземпляр класса authStartClass для получения информации об авторизации сотрудника по IP адресу:
        authStartClass authStartArray = new authStartClass();

        // Создать массивы:
        // для подразделений:
        string[] namesOfTeams = new string[100];           // названия подразделений
        int[] parentsOfTeams = new int[100];               // родители подразделений
        int[] keysOfTeams = new int[100];                  // ключи подразделений

        // для пользователей:
        string[] arrOfNames = new string[500];             // полные имена пользователей
        string[] arrOfDobs = new string[500];              // даты рождения пользователей
        string[] initials = new string[500];               // инициалы пользователей
        string[] jobStr = new string[500];                 // названия профессий пользователей
        int[] arrOfTeams = new int[500];                   // идентификаторы подразделений пользователей
        int[] arrOfKeys = new int[500];                    // ключи пользователей

        // для профессий:
        string[] arrOfJobs = new string[500];              // массив названий профессий
        int[] arrOfKeyJobs = new int[500];                 // массив ключей профессий

        // для дней Рождений:
        DateTime[] dateOfBirthNow = new DateTime[500];     // массив дат рождений пользователей в текущем году
        string[] dateOfBirth = new string[500];            // массив дат рождений пользователей
        int[] age = new int[300];                          // массив возрастов пользователей

        // для праздников:
        DateTime[] datesOfHolidays = new DateTime[500];    // массив обработанных дат праздников
        int[] monthsOfHolidays = new int[500];             // массив месяцев праздников
        string[] Holidays = new string[500];               // массив названий праздников
        int[] daysOfHolidays = new int[500];               // массив дней праздников

        // для переходящих Христианских праздников:
        DateTime[] dChristian = new DateTime[cChristian];  // массив обработанных дат переходящих христианских праздников
        string[] nChristian = new string[cChristian];      // массив названий переходящих христианских праздников

        // для переходящих праздников:
        DateTime[] dHolidays = new DateTime[500];          // массив обработанных дат переходящих праздников
        string[] nHolidays = new string[500];              // массив названий переходящих праздников

        static string pathToServer = @"http://api.nccp-eng.ru/";             // путь к серверу
        static string pathToServerFolder = @"http://api.nccp-eng.ru/app/";   // путь к папке на сервере

        // путь к моей папке:
        static string pathToMyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        // путь к папке с документами:
        static string pathToMyDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        static string pathToDocsDir = pathToMyDocuments + @"\Birthday";    // путь к папке папка users\<Пользователь>\Documents\Birthday
        static string pathToPhotosDir = pathToDocsDir + @"\Photos";        // путь к папке папка users\<Пользователь>\Documents\Birthday\Photos

        static string pathToHolidays = pathToMyDirectory + @"\dates.txt";  // путь к файлу dates.txt

        DirectoryInfo docsDir = new DirectoryInfo(pathToDocsDir);          // папка users\<Пользователь>\Documents\Birthday
        DirectoryInfo photosDir = new DirectoryInfo(pathToPhotosDir);      // папка users\<Пользователь>\Documents\Birthday\Photos

        WebClient client = new WebClient();
        enum months
        {
            January = 1, February, March, April, May, June,
            July, August, September, October, November, December
        };

        /// <summary>
        /// Метод, реализующий post-запрос к API.
        /// </summary>
        /// <param name="postedData">Запрашиваемые данные.</param>
        /// <param name="postUrl">Запрашиваемый адрес.</param>
        /// <returns>Возвращает ответ API на данные postedData по адресу postUrl.</returns>
        public static HttpWebResponse PostMethod(string postedData, string postUrl)
        {
            HttpWebRequest request = null;

            try
            {
                // создать post-запрос:
                request = (HttpWebRequest)WebRequest.Create(postUrl);
                request.Method = "POST";
                request.Credentials = CredentialCache.DefaultCredentials;

                // задать кодировку:
                UTF8Encoding encoding = new UTF8Encoding();
                var bytes = encoding.GetBytes(postedData);

                // задать значение http-заголовка:
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;

                // записать данные запроса в поток:
                using (var newStream = request.GetRequestStream())
                {
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();
                }
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("PostMethod: " + ex.Message);
                Log.Write(ex);
            }

            return (HttpWebResponse)request.GetResponse();
        }

        /// <summary>
        /// Класс преобразования json для получения данных auth.start
        /// (Авторизует сотрудника по IP адресу).
        /// </summary>
        public class authStartClass
        {
            public string ip { get; set; }
            public int number { get; set; }
            public string login { get; set; }
            public string hash { get; set; }
        }

        /// <summary>
        /// Класс преобразования json для получения данных users.get
        /// (Возвращает данные пользователей по их табельным номерам).
        /// </summary>
        public class usersGetClass
        {
            public int job { get; set; }
            public int team { get; set; }
            public int fired { get; set; }
            public int sex { get; set; }
            public string name1 { get; set; }
            public string name2 { get; set; }
            public string name3 { get; set; }
            public string phone1 { get; set; }
            public string phone2 { get; set; }
            public string email { get; set; }
            public string computer { get; set; }
            public string login { get; set; }
            public string dob { get; set; }
        }

        /// <summary>
        /// Класс преобразования json для получения данных teams.get
        /// (Возвращает список подразделений согласно штатному расписанию).
        /// </summary>
        public class teamsGetClass
        {
            public int parent { get; set; }
            public string name { get; set; }
        }

        /// <summary>
        /// Класс преобразования json для получения данных jobs.get
        /// (Возвращает список профессий согласно штатному расписанию).
        /// </summary>
        public class jobsGetClass
        {
            public string name { get; set; }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MinimizeMainForm();                                      // свернуть окно

            // проверка соединения с сетью:
            if (Internet.CheckConnection())                          // подключение есть       
            {
                f_netConnect = true;
            }
            else                                                     // подключение отсутствует   
            {
                //MessageBox.Show("Отсутствует подключение к сети!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            // Проверка запуска ещё одного экземпляра Birthday:
            if (!InstanceCheck())
            {
                // нажаловаться пользователю и завершить процесс:
                MessageBox.Show("Другой экземпляр Birthday уже открыт!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Exit();                                              // закрыть программу
            }

            usersGet();                     // получить информацию о пользователях
            downloadPhotos();               // получить фотографии пользователей
            authStart();                    // получить информацию об авторизации сотрудника по IP адресу
            teamsGet();                     // получить информацию о подразделениях
            jobsGet();                      // получить информацию о профессиях
            PrepareToBuildTree();           // подготовить информацию для построения дерева сотрудников и подразделений
            BuildTree();                    // вывести дерево подразделений и пользователей
            getHolidays();                  // загрузить праздники из файла
            getPassingСhristianНolidays();  // рассчитать переходящие христианские праздники
            getPassingНolidays();           // рассчитать переходящие праздники
            remind();                       // вычислить события
            ShowEvents();                   // вывести события
        }

        /// <summary>
        /// Метод подготовки информации для построения дерева сотрудников и подразделений.
        /// </summary>
        private void PrepareToBuildTree()
        {
            // найти число подразделений:
            foreach (KeyValuePair<string, teamsGetClass> kvp in teamsGetArray)
            //или кратко foreach (var kvp in array1)
            {
                teams++;                              // число подразделений
            }

            // найти число неуволенных пользователей:
            foreach (KeyValuePair<string, usersGetClass> kvp in usersGetArray)
            //или кратко foreach (var kvp in array)
            {
                int fired = Convert.ToInt32(kvp.Value.fired);
                if (fired == 0)                       // если пользователь не уволен
                {
                    notFired++;                       // число неуволенных пользователей
                }
            }

            // изменить размерности массивов:
            Array.Resize(ref namesOfTeams, teams);    // параметры подразделений
            Array.Resize(ref keysOfTeams, teams);
            Array.Resize(ref parentsOfTeams, teams);

            Array.Resize(ref arrOfNames, notFired);   // параметры сотрудников   
            Array.Resize(ref initials, notFired);
            Array.Resize(ref arrOfTeams, notFired);
            Array.Resize(ref arrOfKeys, notFired);
            Array.Resize(ref jobStr, notFired);

            // записать названия, ключи и родители подразделений в массивы:
            int index = 0;                            // индекс элемента словаря
            foreach (KeyValuePair<string, teamsGetClass> kvp in teamsGetArray)
            //или кратко foreach (var kvp in array1)
            {
                namesOfTeams[index] = Convert.ToString(kvp.Value.name);      // записать название подразделения
                keysOfTeams[index] = Convert.ToInt32(kvp.Key);               // записать ключ подразделения
                parentsOfTeams[index] = Convert.ToInt32(kvp.Value.parent);   // записать родителя подразделения

                //richTextBox1.Text += namesOfTeams[index] + " " + keysOfTeams[index] + NewL;

                index++;
            }

            // инициализация массивов:
            int[] years = new int[notFired];     // год рождения
            int[] months = new int[notFired];    // месяц рождения
            int[] days = new int[notFired];      // день рождения

            // изменить размерности массивов:
            Array.Resize(ref dateOfBirthNow, notFired);
            Array.Resize(ref dateOfBirth, notFired);
            Array.Resize(ref age, notFired);

            // записать названия, ключи и подразделения пользователей в массивы:
            int index1 = 0;                           // индекс элемента словаря
            foreach (KeyValuePair<string, usersGetClass> kvp in usersGetArray)
            //или кратко foreach (var kvp in array)
            {
                int fired = Convert.ToInt32(kvp.Value.fired);
                if (fired == 0)                                              // если пользователь не уволен
                {
                    arrOfNames[index1] = kvp.Value.name1 + " " + kvp.Value.name2 + " " + kvp.Value.name3;   // записать полные имена пользователей в массив arrOfNames
                    arrOfTeams[index1] = Convert.ToInt32(kvp.Value.team);    // записать подразделения пользователей в массив arrOfTeams
                    arrOfKeys[index1] = Convert.ToInt32(kvp.Key);            // записать ключи пользователей в массив arrOfKeys
                    arrOfDobs[index1] = Convert.ToString(kvp.Value.dob);     // записать дни рождения пользователей в массив arrOfDobs

                    index1++;
                }
            }

            try
            {
                // заполнение массивов лет, месяцев, дней и дат рождений:
                for (int i = 0; i < notFired; i++)
                {
                    years[i] = Convert.ToInt32(arrOfDobs[i].Substring(0, 4));
                    months[i] = Convert.ToInt32(arrOfDobs[i].Substring(5, 2));
                    days[i] = Convert.ToInt32(arrOfDobs[i].Substring(8, 2));

                    dateOfBirthNow[i] = new DateTime(thisYear, months[i], days[i]);

                    dateOfBirth[i] = Convert.ToString(years[i]) + "." +
                                     Convert.ToString(months[i]) + "." +
                                     Convert.ToString(days[i]);
                }
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("PrepareToBuildTree: " + ex.Message);
                Log.Write(ex);
            }

            // заполнение массива возрастов:
            for (int i = 0; i < notFired; i++)
            {
                age[i] = thisYear - years[i];
            }

            // записать фамилии, инициалы и возрасты пользователей в массивы:
            int index3 = 0;                                                  // индекс элемента словаря
            foreach (KeyValuePair<string, usersGetClass> kvp in usersGetArray)
            //или кратко foreach (var kvp in array) 
            {
                int fired = Convert.ToInt32(kvp.Value.fired);
                if (fired == 0)                                              // если пользователь не уволен
                {
                    initials[index3] = kvp.Value.name1 + " " + kvp.Value.name2.Substring(0, 1) + ". "
                        + kvp.Value.name3.Substring(0, 1) + ".";

                    index3++;
                }
            }
        }

        /// <summary>
        /// Метод загрузки праздников из файла.
        /// </summary>
        private void getHolidays()
        {
            if (File.Exists(pathToHolidays))  // если файл с перечнем дат существует
            {
                string line;                 // строка файла

                // считать файл и извлечь данные из каждой строки:
                System.IO.StreamReader file = new System.IO.StreamReader(pathToHolidays);
                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        daysOfHolidays[counter] = Convert.ToInt32(line.Substring(0, 2));    // дни праздников
                        monthsOfHolidays[counter] = Convert.ToInt32(line.Substring(3, 2));  // месяцы праздников
                        Holidays[counter] = line.Substring(6, (line.Length - 6));           // названия праздников
                    }
                    catch (Exception ex)
                    {
                        if (f_debugMode) MessageBox.Show("getHolidays: " + ex.Message);
                        Log.Write(ex);
                    }

                    counter++;
                }

                file.Close();                // закрыть файл

                // изменить размерности массивов:
                Array.Resize(ref daysOfHolidays, counter);
                Array.Resize(ref monthsOfHolidays, counter);
                Array.Resize(ref Holidays, counter);
                Array.Resize(ref datesOfHolidays, counter);

                // заполнение массива праздников:
                for (int i = 0; i < counter; i++)
                {
                    datesOfHolidays[i] = new DateTime(thisYear, monthsOfHolidays[i], daysOfHolidays[i]);
                }
            }
        }

        /// <summary>
        /// Метод расчета событий текущих и будущих дней рождения сотрудников и праздников.
        /// </summary> 
        public void remind()
        {
            remindTimer.Interval = hour * f_hours;           // напоминание каждые f_hours часов
            remindTimer.Enabled = true;

            string[] arrOfHolidaysToday = new string[20];    // праздники сегодня
            string[] arrOfHolidaysSoon = new string[20];     // ближайшие праздники

            int numbOfDobsLately = 0;                        // количество недавних дней рождения
            int numbOfDobs = 0;                              // количество дней рождения сегодня
            int numbOfDobsSoon = 0;                          // количество ближайших дней рождения

            int numbOfHolidays = 0;                          // количество праздников сегодня
            int numbOfHolidaysSoon = 0;                      // количество ближайших праздников
                      
            // очистить массивы:                                                  
            Array.Clear(strOfEvents, 0, events);             
            Array.Clear(strOfColors, 0, events);             

            // изменить размерности массивов:
            Array.Resize(ref strOfEvents, 500);
            Array.Resize(ref strOfColors, 500);

            events = 0;                                      // количество событий
            strOfDobs = 0;

            /////////////////////////////////////////////////////////////////////////////////////////
            // получить профессии пользователей:
            int index = 0;
            foreach (KeyValuePair<string, usersGetClass> kvp in usersGetArray)
            //или кратко foreach (var kvp in array)
            {
                int fired = Convert.ToInt32(kvp.Value.fired);
                if (fired == 0)                              // если пользователь не уволен
                {
                    int job = kvp.Value.job;

                    // записать название профессии в строку:
                    for (int i = 0; i < jobs; i++)
                    {
                        if (arrOfKeyJobs[i] == job)          // если ключ профессии равен идентификатору профессии
                        {
                            jobStr[index] = arrOfJobs[i];    // записать в строку название профессии
                            index++;
                        }
                    }
                }
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            // получить и записать события:
            DateTime ndt = DateTime.Today;                   // DateTime.Today || new DateTime(2017, 03, 19)

            string[,] s1 = new string[8, 100];               // массивы для записи инфо о днях рождения
            string[,] s2 = new string[8, 100];
            string[,] s3 = new string[8, 100];

            if (f_birthday)                                  // если выводить дни рождения
            {
                // найти недавние (f_days дней) дни рождения и их количество:
                for (int j = 1; j <= f_days; j++)           
                {
                    for (int i = 0; i < notFired; i++)
                    {
                        if (dateOfBirthNow[i].AddDays(j) == ndt)
                        {
                            s1[0, numbOfDobsLately] = arrOfNames[i];
                            s1[1, numbOfDobsLately] = " (";
                            s1[2, numbOfDobsLately] = Convert.ToString(age[i]);
                            s1[3, numbOfDobsLately] = "), ";
                            s1[4, numbOfDobsLately] = dateOfBirth[i];
                            s1[5, numbOfDobsLately] = "\n";
                            s1[6, numbOfDobsLately] = jobStr[i];
                            s1[7, numbOfDobsLately] = "\n\n";

                            numbOfDobsLately++;
                        }
                    }
                }

                // записать недавние дни рождения в массивы:
                if (numbOfDobsLately > 0)                                 // если есть недавние дни рождения
                {
                    strOfEvents[events] = "\n\n\n\nНедавние дни рождения: \n\n";  // заголовок
                    strOfColors[events] = Color.Blue;                     // цвет заголовка
                    events++;

                    for (int j = 0; j < numbOfDobsLately; j++)
                    {
                        strOfEvents[events] = s1[0, j];                   // ФИО
                        strOfColors[events] = Color.Red;
                        events++;

                        for (int i = 1; i < 8; i++)
                        {
                            strOfEvents[events] = s1[i, j];               // данные
                            strOfColors[events] = Color.Black;
                            events++;
                        }
                    }
                }

                // найти дни рождения сегодня и их количество:
                for (int i = 0; i < notFired; i++)
                {
                    if (dateOfBirthNow[i] == ndt)
                    {
                        s2[0, numbOfDobs] = arrOfNames[i];
                        s2[1, numbOfDobs] = " (";
                        s2[2, numbOfDobs] = Convert.ToString(age[i]);
                        s2[3, numbOfDobs] = "), ";
                        s2[4, numbOfDobs] = dateOfBirth[i];
                        s2[5, numbOfDobs] = "\n";
                        s2[6, numbOfDobs] = jobStr[i];
                        s2[7, numbOfDobs] = "\n\n";

                        numbOfDobs++;
                    }
                }

                // записать дни рождения сегодня в массивы:
                if (numbOfDobs > 0)                                       // если есть дни рождения сегодня
                {
                    strOfEvents[events] = "Сегодня день рождения: \n\n";  // заголовок
                    strOfColors[events] = Color.Blue;                     // цвет заголовка
                    events++;

                    for (int j = 0; j < numbOfDobs; j++)
                    {
                        strOfEvents[events] = s2[0, j];                   // ФИО
                        strOfColors[events] = Color.Red;
                        events++;

                        for (int i = 1; i < 8; i++)
                        {
                            strOfEvents[events] = s2[i, j];               // данные
                            strOfColors[events] = Color.Black;
                            events++;
                        }
                    }
                }

                // найти ближайшие (f_days дней) дни рождения и их количество:
                for (int j = 1; j <= f_days; j++)
                {
                    for (int i = 0; i < notFired; i++)
                    {
                        if (dateOfBirthNow[i] == ndt.AddDays(j))
                        {
                            s3[0, numbOfDobsSoon] = arrOfNames[i];
                            s3[1, numbOfDobsSoon] = " (";
                            s3[2, numbOfDobsSoon] = Convert.ToString(age[i]);
                            s3[3, numbOfDobsSoon] = "), ";
                            s3[4, numbOfDobsSoon] = dateOfBirth[i];
                            s3[5, numbOfDobsSoon] = "\n";
                            s3[6, numbOfDobsSoon] = jobStr[i];
                            s3[7, numbOfDobsSoon] = "\n\n";

                            numbOfDobsSoon++;
                        }
                    }
                }

                // записать ближайшие (f_days дней) дни рождения в массивы:
                if (numbOfDobsSoon > 0)                                   // если есть ближайшие дни рождения
                {
                    switch (f_days)                                       // заголовок
                    {
                        case 1:
                            strOfEvents[events] = "Дни рождения в ближайший день: \n\n";
                            break;

                        case 2:
                        case 3:
                        case 4:
                            strOfEvents[events] = "Дни рождения в ближайшие " + f_days + " дня: \n\n";
                            break;

                        case 5:
                        case 6:
                        case 7:
                            strOfEvents[events] = "Дни рождения в ближайшие " + f_days + " дней: \n\n";
                            break;
                    }

                    strOfColors[events] = Color.Blue;                     // цвет заголовка
                    events++;

                    for (int j = 0; j < numbOfDobsSoon; j++)
                    {
                        strOfEvents[events] = s3[0, j];                   // ФИО
                        strOfColors[events] = Color.Red;
                        events++;

                        for (int i = 1; i < 8; i++)
                        {
                            strOfEvents[events] = s3[i, j];               // данные
                            strOfColors[events] = Color.Black;
                            events++;
                        }
                    }
                }
            }

            if (f_holiday)
            {
                // найти праздники сегодня и их количество:
                for (int i = 0; i < counter; i++)
                {
                    if (datesOfHolidays[i] == ndt)
                    {
                        arrOfHolidaysToday[numbOfHolidays] = Holidays[i];
                        numbOfHolidays++;
                    }
                }

                // найти переходящие праздники сегодня и их количество:
                for (int i = 0; i < countPass; i++)
                {
                    if (dHolidays[i] == ndt)
                    {
                        arrOfHolidaysToday[numbOfHolidays] = nHolidays[i];
                        numbOfHolidays++;
                    }
                }

                // найти переходящие Христианские праздники сегодня и их количество:
                for (int i = 0; i < cChristian; i++)
                {
                    if (dChristian[i] == ndt)
                    {
                        arrOfHolidaysToday[numbOfHolidays] = nChristian[i];
                        numbOfHolidays++;
                    }
                }

                // записать праздники сегодня в массивы:
                if (numbOfHolidays > 0)                                      // если есть праздники сегодня
                {
                    strOfEvents[events] = "Сегодня даты: \n\n";              // заголовок
                    strOfColors[events] = Color.Blue;                        // цвет заголовка
                    events++;

                    for (int j = 0; j < numbOfHolidays; j++)
                    {
                        strOfEvents[events] = arrOfHolidaysToday[j] + "\n";  // данные
                        strOfColors[events] = Color.Black;
                        events++;
                    }

                    strOfEvents[events - 1] += "\n";
                }

                // найти ближайшие (f_days дней) праздники и их количество:
                for (int j = 1; j <= f_days; j++)
                {
                    for (int i = 0; i < counter; i++)
                    {
                        if (datesOfHolidays[i] == ndt.AddDays(j))
                        {
                            arrOfHolidaysSoon[numbOfHolidaysSoon] = Holidays[i];
                            numbOfHolidaysSoon++;
                        }
                    }
                }

                // найти ближайшие (f_days дней) переходящие праздники и их количество:
                for (int j = 1; j <= f_days; j++)
                {
                    for (int i = 0; i < countPass; i++)
                    {
                        if (dHolidays[i] == ndt.AddDays(j))
                        {
                            arrOfHolidaysSoon[numbOfHolidaysSoon] = nHolidays[i];
                            numbOfHolidaysSoon++;
                        }
                    }
                }

                // найти ближайшие (f_days дней) Христианские праздники и их количество:
                for (int j = 1; j <= f_days; j++)
                {
                    for (int i = 0; i < cChristian; i++)
                    {
                        if (dChristian[i] == ndt.AddDays(j))
                        {
                            arrOfHolidaysSoon[numbOfHolidaysSoon] = nChristian[i];
                            numbOfHolidaysSoon++;
                        }
                    }
                }

                // записать ближайшие (f_days дней) праздники в массивы:
                if (numbOfHolidaysSoon > 0)                               // если есть ближайшие дни рождения
                {
                    switch (f_days)                                       // заголовок
                    {
                        case 1:
                            strOfEvents[events] = "Даты в ближайший день: \n\n";
                            break;

                        case 2:
                        case 3:
                        case 4:
                            strOfEvents[events] = "Даты в ближайшие " + f_days + " дня: \n\n";
                            break;

                        case 5:
                        case 6:
                        case 7:
                            strOfEvents[events] = "Даты в ближайшие " + f_days + " дней: \n\n";
                            break;
                    }

                    strOfColors[events] = Color.Blue;                       // цвет заголовка
                    events++;

                    for (int j = 0; j < numbOfHolidaysSoon; j++)
                    {
                        strOfEvents[events] = arrOfHolidaysSoon[j] + "\n";  // данные
                        strOfColors[events] = Color.Black;
                        events++;
                    }
                }
            }

            // изменить размерности массивов:
            Array.Resize(ref strOfEvents, events);
            Array.Resize(ref strOfColors, events);

            Array.Resize(ref arrOfHolidaysToday, numbOfHolidays);
            Array.Resize(ref arrOfHolidaysSoon, numbOfHolidaysSoon);

            strOfDobs = numbOfDobsLately + numbOfDobs + numbOfDobsSoon + numbOfHolidays + numbOfHolidaysSoon;
        }

        public void ShowEvents()
        {
            // вывести сообщение:
            if (notifyIcon1.Visible == true) ShowBalloon();
            if (notifyIcon1.Visible == false) ShowRemindForm();
        }

        public void ShowBalloon()
        {
            remind();                                                         // вычислить события

            // если есть что выводить, то вывести сообщение:
            if (strOfDobs != 0)
            {
                notifyIcon1.ShowBalloonTip(15000, "Внимание!", "Есть события, кликни на меня.", ToolTipIcon.Info);
            }
            else
            {
                notifyIcon1.ShowBalloonTip(15000, "Внимание!", "Событий нет.", ToolTipIcon.Info);
            }
        }

        public void ShowRemindForm()
        {
            remind();                                                         // вычислить события

            // если есть что выводить, то вывести сообщение:
            if (strOfDobs != 0)
            {
                RemindForm remindForm = new RemindForm();                     // создать форму
                
                // если форма открыта, закрыть её:
                if (Application.OpenForms["RemindForm"] != null)
                {
                    remindForm.Close();                                       // закрыть форму
                }
                else
                {
                    remindForm.Show();                                        // открыть форму
                    remindForm.ShowRemind(strOfEvents, strOfColors, events);  // вывести сообщение
                }
            }
        }

        /// <summary>
        /// Метод закрытия формы remindForm
        /// </summary>
        public static void CloseRemindForm()
        {
            // Закрыть remindForm:
            FormCollection FCollection = Application.OpenForms;

            try
            {
                foreach (Form f in FCollection)
                {
                    if (f.Name == "RemindForm")
                    {
                        f.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("Exit: " + ex.Message);
                Log.Write(ex);
            }
        }

        /// <summary>
        /// Метод вывода дерева подразделений и пользователей.
        /// </summary>
        private void BuildTree()
        {
            if (f_sorting) f_sorting = false;
            else f_sorting = true;

            DataTable dt = new DataTable();                     // создание таблицы данных в памяти
            BindingSource bindingSource = new BindingSource();  // создание источника данных
            bindingSource.DataSource = dt;

            if (f_sorting)
            {
                // создание столбцов таблицы данных в памяти:
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("Value", typeof(string));
                dt.Columns.Add("Parent_Id", typeof(int));

                //  создание строк таблицы данных в памяти, заполненных инфо о сотрудниках:
                int index2 = 0;
                foreach (string s in arrOfNames)             // перебор по строкам массива arrOfNames1[]
                {
                    dt.Rows.Add(arrOfTeams[index2], s, 0);   // добавить строку с инфо о пользователе в таблицу
                    index2++;
                }

                FillTreeView(tV_Tree, dt);                   // заполнить дерево из таблицы
            }
            else
            {
                // создание столбцов таблицы данных в памяти:
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("Value", typeof(string));
                dt.Columns.Add("Parent_Id", typeof(int));

                //  создание строк таблицы данных в памяти, заполненных инфо о сотрудниках и подразделениях:
                dt.Rows.Add(1, "НЗХК-Инжиниринг", 0);        // добавить строку корневого узла в таблицу

                int notEmpty = 0;

                foreach (KeyValuePair<string, teamsGetClass> kvp in teamsGetArray)  // перебор по элементам словаря teamsGetArray
                                                                                    //или кратко foreach (var kvp in array1)
                {
                    string teamName = Convert.ToString(kvp.Value.name);      // название подразделения
                    int teamKey = Convert.ToInt32(kvp.Key);                  // ключ подразделения
                    int teamParent = Convert.ToInt32(kvp.Value.parent);      // родитель подразделения

                    //richTextBox1.Text += teamKey + " " + teamName + " " + teamParent + NewL;

                    notEmpty = 0;                                            // подразделение не пустое

                    for (int j = 0; j < notFired; j++)                       // перебор всех пользователей
                    {
                        if (teamKey == arrOfTeams[j])                        // если в подразделении есть сотрудники 
                        {
                            notEmpty++;                                      // подразделение не пустое
                        }
                    }

                    for (int j = 0; j < teams; j++)                          // перебор всех подразделений
                    {
                        if (teamKey == parentsOfTeams[j])                    // если в подразделении есть подразделения 
                        {
                            notEmpty++;                                      // подразделение не пустое
                        }
                    }

                    if ((notEmpty != 0) && (teamKey != 100))                 // если подразделение не пустое и не запрещенное
                    {
                        dt.Rows.Add(teamKey, teamName, teamParent);          // добавить строку с инфо о подразделении в таблицу
                    }

                    for (int j = 0; j < notFired; j++)                       // перебор всех пользователей
                    {
                        if (arrOfTeams[j] == teamKey)                        // если номер подразделения пользователя = ключу подразделения
                        {
                            dt.Rows.Add(arrOfKeys[j], arrOfNames[j], teamKey);  // добавить строку с инфо о пользователе в таблицу
                        }
                    }
                }
                FillTreeView(tV_Tree, dt);                                   // заполнить дерево из таблицы
            }
            //notifyIcon1.Visible = false;                                   // иконка в трее не видна
        }

        /// <summary>
        ///  Заполнение дерева из БД
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="dt"></param>
        private void FillTreeView(TreeView treeView, DataTable dt)
        {
            treeView.Nodes.Clear();
            DataRow[] rows = dt.Select("Parent_Id = 0");
            foreach (DataRow row in rows)
            {
                TreeNode node = new TreeNode(row["Value"].ToString());
                node.Tag = row["Id"];
                treeView.Nodes.Add(node);
                FillNode(node, dt);
            }
        }

        /// <summary>
        ///  Добавление к ноде чилдов
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="dt"></param>
        private void FillNode(TreeNode parentNode, DataTable dt)
        {
            DataRow[] rows = dt.Select(string.Format("Parent_Id = {0}", parentNode.Tag));
            foreach (DataRow row in rows)
            {
                TreeNode treeNode = new TreeNode(row["Value"].ToString());
                treeNode.Tag = row["Id"];
                parentNode.Nodes.Add(treeNode);
                FillNode(treeNode, dt);
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            treeViewClick();
        }

        public void treeViewClick()
        {
            try
            {
                if (tV_Tree.SelectedNode.Text != "НЗХК-Инжиниринг")    // если не корневой узел
                {
                    if (tV_Tree.SelectedNode.FirstNode == null)        // если нет дочерних узлов
                    {
                        UserForm UserForm = new UserForm();            // создать форму

                        // передать данные о сотруднике в форму UserForm:
                        foreach (KeyValuePair<string, usersGetClass> kvp in usersGetArray)
                        //или кратко foreach (var kvp in array)
                        {
                            int fired = Convert.ToInt32(kvp.Value.fired);
                            if (fired == 0)                            // если пользователь не уволен
                            {
                                string name1 = kvp.Value.name1;
                                string name2 = kvp.Value.name2;
                                string name3 = kvp.Value.name3;
                                int team = kvp.Value.team;
                                int job = kvp.Value.job;
                                string phone1 = kvp.Value.phone1;
                                string phone2 = kvp.Value.phone2;
                                string dob = kvp.Value.dob;
                                string computer = kvp.Value.computer;
                                string login = kvp.Value.login;
                                string email = kvp.Value.email;
                                string Key = kvp.Key;

                                string teamS = null;                   // строка для названия подразделения
                                string jobS = null;                    // строка для названия профессии

                                // записать название подразделения в строку:
                                for (int i = 0; i < teams; i++)
                                {
                                    if (keysOfTeams[i] == team)        // если ключ подразделения равен идентификатору подразделения
                                        teamS = namesOfTeams[i];       // записать в строку название подразделения
                                }

                                // записать название профессии в строку:
                                for (int i = 0; i < jobs; i++)
                                {
                                    if (arrOfKeyJobs[i] == job)        // если ключ профессии равен идентификатору профессии
                                        jobS = arrOfJobs[i];           // записать в строку название профессии
                                }

                                string pathToPhoto = pathToPhotosDir + @"\" + Key + ".jpg";

                                // сокрытие логина в Active Directory:
                                if (userName != name2 + " " + name3 + " " + name1)
                                    login = "***";

                                // если выбран сотрудник:
                                if (tV_Tree.SelectedNode.Text == name1 + " " + name2 + " " + name3)
                                {
                                    if (File.Exists(pathToPhoto))      // если файл фото существует
                                    {
                                        UserForm.showUserInfo(name1, name2, name3, teamS, jobS, phone1, phone2, dob, computer, login, email, pathToPhoto);
                                    }
                                    else
                                    {
                                        UserForm.showUserInfo(name1, name2, name3, teamS, jobS, phone1, phone2, dob, computer, login, email, null);
                                    }
                                }
                            }
                        }
                        UserForm.ShowDialog();                         // открыть форму
                    }
                }
            }

            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("treeViewClick: " + ex.Message);
                Log.Write(ex);
            }
        }

        /// <summary>
        /// Нажатие на иконку в трее мышью.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)   // если нажата лкм
            {
                if (this.WindowState == FormWindowState.Minimized)    // если окно свёрнуто 
                {
                    this.WindowState = FormWindowState.Normal;        // восстановить окно
                    this.ShowInTaskbar = true;                        // вернуть отображение окна в панели задач Windows
                    notifyIcon1.Visible = false;                      // иконка в трее не видна
                    ShowEvents();                                     // вывести напоминание о текущих и будущих днях рождения сотрудников
                    Activate();                                       // активировать форму
                }
                else                                                  // если окно активно
                {
                    MinimizeMainForm();                               // свернуть окно                  
                }
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)  // если нажата пкм
            {
                contextMenuStrip1.Show(Cursor.Position);              // вывести контекстное меню
            }
        }

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {

            // если окно свёрнуто  
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Text = "Кликните на иконку, чтобы развернуть окно программы.";
            }
            else
            {
                notifyIcon1.Text = "Кликните на иконку, чтобы свернуть окно программы.";
            }
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Minimized) // если окно свёрнуто
                {
                    this.ShowInTaskbar = false;                    // убрать отображение окна в панели задач Windows
                    notifyIcon1.Visible = true;                    // иконка в трее видна
                }

                if (notifyIcon1.Visible == false)                  // если иконка в трее не видна
                {
                    t_MainForm.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("MainForm_Deactivate: " + ex.Message);
                Log.Write(ex);
            }
        }

        private void contextMenuStrip1_MouseLeave(object sender, EventArgs e)
        {
            contextMenuStrip1.Close();                         // убрать контекстное меню
        }

        /// <summary>
        /// Метод поиска в дереве.
        /// </summary>
        /// <param name="SearchText" - строка поиска.></param>
        /// <param name="StartNode" - начальный узел.></param>
        /// <returns>Если найдено, возвращает узел, если ничего не найдено - возвращает null.</returns>
        private TreeNode SearchNode(string SearchText, TreeNode StartNode)
        {
            TreeNode node = null;
            while (StartNode != null)
            {
                if (StartNode.Text.ToLower().Contains(SearchText.ToLower()))
                {
                    node = StartNode;                                   // чето нашли, выходим
                    break;
                };
                if (StartNode.Nodes.Count != 0)                         // у узла есть дочерние элементы
                {
                    node = SearchNode(SearchText, StartNode.Nodes[0]);  // ищем рекурсивно в дочерних
                    if (node != null)
                    {
                        break;                                          // чето нашли
                    };
                };
                StartNode = StartNode.NextNode;
            };
            return node;                                                // вернули результат поиска
        }

        private void tB_Search_TextChanged(object sender, EventArgs e)
        {
            SearchInTree();  // поиск в дереве
        }

        /// <summary>
        /// Метод поиска в дереве.
        /// </summary>
        private void SearchInTree()
        {
            string SearchText = tB_Search.Text;
            if (SearchText == "")
            {
                tV_Tree.CollapseAll();
                return;
            };

            TreeNode SelectedNode = SearchNode(SearchText, tV_Tree.Nodes[0]);  // пытаемся найти в поле Text

            /*
            try
            {
                // цвета узла по умолчанию:
                tV_Tree.SelectedNode.BackColor = System.Drawing.Color.Empty;
                tV_Tree.SelectedNode.ForeColor = System.Drawing.Color.Black;
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("SearchInTree: " + ex.Message);
                MessageBox.Show("SearchInTree: " + ex.Message);
                Log.Write(ex);
            }
            */

            if (SelectedNode != null)
            {
                // нашли, выделяем...
                tV_Tree.SelectedNode = SelectedNode;
                tV_Tree.SelectedNode.Expand();
                tV_Tree.Select();

                /*
                try
                {
                    // цвета выделенного при поиске узла:
                    tV_Tree.SelectedNode.BackColor = System.Drawing.ColorTranslator.FromHtml("#3399FF");
                    tV_Tree.SelectedNode.ForeColor = System.Drawing.Color.White;
                }
                catch (Exception ex)
                {
                    if (f_debugMode) MessageBox.Show("SearchInTree: " + ex.Message);
                    Log.Write(ex);
                }
                */

                tB_Search.Focus();                                              // фокус на строке поиска
            };
        }

        private void календарьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calendar();
        }

        /// <summary>
        /// Метод получения информации о пльзователях.
        /// </summary>
        private void usersGet()
        {
            string uJson = null;
            string pathUsersGet = pathToDocsDir + @"\usersGet.json";

            if (f_netConnect)
            {
                // Получить информацию о пользователях:
                string usersGetMeth = "method=users.get";
                string usersGetResponse = null;

                try
                {
                    // выполнить post-запрос:
                    var uResponse = PostMethod(usersGetMeth, pathToServer);
                    if (uResponse != null)
                    {
                        var strreader = new StreamReader(uResponse.GetResponseStream(), Encoding.UTF8);
                        usersGetResponse = strreader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    if (f_debugMode) MessageBox.Show("usersGet: " + ex.Message);
                    Log.Write(ex);
                }

                try
                {
                    // записать полученный json в файл:
                    StreamWriter uSW = new StreamWriter(new FileStream(pathUsersGet, FileMode.Create, FileAccess.Write));
                    uSW.Write(usersGetResponse);
                    uSW.Close();
                }
                catch (Exception ex)
                {
                    if (f_debugMode) MessageBox.Show("usersGet: " + ex.Message);
                    Log.Write(ex);
                }

                uJson = usersGetResponse;
            }
            else
            {
                // проверка наличия файла usersGet.json в текущей папке:
                if (File.Exists(pathUsersGet))                       // если файл существует
                {
                    uJson = File.ReadAllText(pathUsersGet, Encoding.Default).Replace("\n", " ");
                }
                else
                {
                    MessageBox.Show("Файл usersGet.json не найден, программа будет закрыта!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Exit();                                          // закрыть программу
                }
            }

            try
            {
                // парсить полученный json с информацией о пользователях:
                usersGetArray = JsonConvert.DeserializeObject<Dictionary<string, Birthday.MainForm.usersGetClass>>(uJson);

                // отсортировать пользователей по фамилии в алфавитном порядке 
                usersGetArray = usersGetArray.OrderBy(pair => pair.Value.name1).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("usersGet: " + ex.Message);
                Log.Write(ex);
            }
        }

        /// <summary>
        /// Метод получения локального IP адреса.
        /// </summary>
        /// <returns>Локальный IP адрес.</returns>
        private static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIP = null;

            try
            {
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip;
                        return localIP;
                    }
                }
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("GetLocalIPAddress: " + ex.Message);
                Log.Write(ex);
            }

            return null;
        }

        /// <summary>
        /// Метод получения информации об авторизации сотрудника по IP адресу.
        /// </summary>
        private void authStart()
        {
            string aJson = null;
            string pathAuthStart = pathToDocsDir + @"\authStart.json";

            if (f_netConnect)
            {
                // Получить ip-адрес
                IPAddress entIP = GetLocalIPAddress();

                //Получить табельный номер сотрудника по IP адресу:
                string authStartMeth = "method=auth.start&ip=";
                string param = authStartMeth + entIP;
                string authResponse = null;

                try
                {
                    // выполнить post-запрос:
                    var aResponse = PostMethod(param, pathToServer);
                    if (aResponse != null)
                    {
                        var strreader = new StreamReader(aResponse.GetResponseStream(), Encoding.UTF8);
                        authResponse = strreader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    if (f_debugMode) MessageBox.Show("authStart: " + ex.Message);
                    Log.Write(ex);
                }

                try
                {
                    // записать полученный json в файл:
                    StreamWriter aSW = new StreamWriter(new FileStream(pathAuthStart, FileMode.Create, FileAccess.Write));
                    aSW.Write(authResponse);
                    aSW.Close();
                }
                catch (Exception ex)
                {
                    if (f_debugMode) MessageBox.Show("authStart: " + ex.Message);
                    Log.Write(ex);
                }

                aJson = authResponse;
            }
            else
            {
                // проверка наличия файла authStart.json в текущей папке:
                if (File.Exists(pathAuthStart))                      // если файл не существует
                {
                    aJson = File.ReadAllText(pathAuthStart, Encoding.Default).Replace("\n", " ");
                }
                else
                {
                    MessageBox.Show("Файл authStart.json не найден, программа будет закрыта!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Exit();                                          // закрыть программу
                }
            }

            try
            {
                // парсить полученный json с информацией об авторизации сотрудника по IP адресу:
                authStartArray = JsonConvert.DeserializeObject<Birthday.MainForm.authStartClass>(aJson);
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("authStart: " + ex.Message);
                Log.Write(ex);
            }

            // проверка наличия в системе по табельному номеру:
            int index = authStartArray.number;                       // табельный номер
            string s = Convert.ToString(index);

            try
            {
                userName = usersGetArray[s].name2 + " " + usersGetArray[s].name3 + " " + usersGetArray[s].name1;
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("authStart: " + ex.Message);
                Log.Write(ex);
            }

            // сокрытие настроек:
            if (userName == "Илья Михайлович Минин")
            {
                f_debugEn = true;
            }
            else
            {
                f_debugEn = false;
            }
        }

        /// <summary>
        /// Метод получения информации о подразделениях.
        /// </summary>
        private void teamsGet()
        {
            string tJson = null;
            string pathTeamsGet = pathToDocsDir + @"\teamsGet.json";

            if (f_netConnect)
            {
                // Получить информацию о подразделениях:
                string teamsGetMeth = "method=teams.get";
                string teamsGetResponse = null;

                try
                {
                    // выполнить post-запрос:
                    var tResponse = PostMethod(teamsGetMeth, pathToServer);
                    if (tResponse != null)
                    {
                        var strreader = new StreamReader(tResponse.GetResponseStream(), Encoding.UTF8);
                        teamsGetResponse = strreader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    if (f_debugMode) MessageBox.Show("teamsGet: " + ex.Message);
                    Log.Write(ex);
                }

                try
                {
                    // записать полученный json в файл:
                    StreamWriter tSW = new StreamWriter(new FileStream(pathTeamsGet, FileMode.Create, FileAccess.Write));
                    tSW.Write(teamsGetResponse);
                    tSW.Close();
                }
                catch (Exception ex)
                {
                    if (f_debugMode) MessageBox.Show("teamsGet: " + ex.Message);
                    Log.Write(ex);
                }

                tJson = teamsGetResponse;
            }
            else
            {
                // проверка наличия файла teamsGet.json в текущей папке:
                if (File.Exists(pathTeamsGet))                       // если файл существует
                {
                    tJson = File.ReadAllText(pathTeamsGet, Encoding.Default).Replace("\n", " ");
                }
                else
                {
                    MessageBox.Show("Файл teamsGet.json не найден, программа будет закрыта!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Exit();                                          // закрыть программу
                }
            }

            try
            {
                // парсить полученный json с информацией о подразделениях:
                teamsGetArray = JsonConvert.DeserializeObject<Dictionary<string, Birthday.MainForm.teamsGetClass>>(tJson);

                // отсортировать подразделения в алфавитном порядке:
                teamsGetArray = teamsGetArray.OrderBy(pair => pair.Value.name).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("teamsGet: " + ex.Message);
                Log.Write(ex);
            }
        }

        /// <summary>
        /// Метод получения информации о профессиях.
        /// </summary>
        private void jobsGet()
        {
            string jJson = null;
            string pathJobsGet = pathToDocsDir + @"\jobsGet.json";

            if (f_netConnect)
            {
                // Получить информацию о профессиях:
                string jobsGetMeth = "method=jobs.get";
                string jobsGetResponse = null;

                try
                {
                    // выполнить post-запрос:
                    var jResponse = PostMethod(jobsGetMeth, pathToServer);
                    if (jResponse != null)
                    {
                        var strreader = new StreamReader(jResponse.GetResponseStream(), Encoding.UTF8);
                        jobsGetResponse = strreader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    if (f_debugMode) MessageBox.Show("jobsGet: " + ex.Message);
                    Log.Write(ex);
                }

                try
                {
                    // записать полученный json в файл:
                    StreamWriter jSW = new StreamWriter(new FileStream(pathJobsGet, FileMode.Create, FileAccess.Write));
                    jSW.Write(jobsGetResponse);
                    jSW.Close();
                }
                catch (Exception ex)
                {
                    if (f_debugMode) MessageBox.Show("jobsGet: " + ex.Message);
                    Log.Write(ex);
                }

                jJson = jobsGetResponse;
            }
            else
            {
                // проверка наличия файла teamsGet.json в текущей папке:
                if (File.Exists(pathJobsGet))                        // если файл существует
                {
                    jJson = File.ReadAllText(pathJobsGet, Encoding.Default).Replace("\n", " ");
                }
                else
                {
                    MessageBox.Show("Файл jobsGet.json не найден, программа будет закрыта!", "Birthday", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Exit();                                          // закрыть программу
                }
            }

            try
            {
                // парсить полученный json с информацией о пользователях:
                jobsGetArray = JsonConvert.DeserializeObject<Dictionary<string, Birthday.MainForm.jobsGetClass>>(jJson);

                // отсортировать в алфавитном порядке 
                jobsGetArray = jobsGetArray.OrderBy(pair => pair.Value.name).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            catch (Exception ex)
            {
                if (f_debugMode) MessageBox.Show("jobsGet: " + ex.Message);
                Log.Write(ex);
            }

            // записать названия и ключи профессий в массивы, а так же найти число профессий:
            foreach (KeyValuePair<string, jobsGetClass> kvp in jobsGetArray)
            //или кратко foreach (var kvp in array)
            {
                arrOfJobs[jobs] = kvp.Value.name;
                arrOfKeyJobs[jobs] = Convert.ToInt32(kvp.Key);
                jobs++;
            }

            Array.Resize(ref arrOfJobs, jobs);             // изменить размерность массива arrOfJobs
            Array.Resize(ref arrOfKeyJobs, jobs);          // изменить размерность массива arrOfKeyJobs
        }

        /// <summary>
        /// Метод получения фотографий пользователей.
        /// </summary>
        private void downloadPhotos()
        {
            if (f_netConnect)
            {
                photosDir.Create();                                                                     // создать папку для фото

                foreach (KeyValuePair<string, usersGetClass> kvp in usersGetArray)
                //или кратко foreach (var kvp in array)
                {
                    int fired = Convert.ToInt32(kvp.Value.fired);
                    if (fired == 0)                                                               // если пользователь не уволен
                    {
                        try
                        {
                            string Key = kvp.Key;
                            string pathToPhoto = @"http://api.nccp-eng.ru/photo/" + Key + ".jpg";     // путь к фото
                            client.DownloadFile(pathToPhoto, pathToPhotosDir + @"\" + Key + ".jpg");  // скачать файл
                            client.Dispose();                                                         // отключить вэбклиента
                        }
                        catch (Exception ex)
                        {
                            if (f_debugMode) MessageBox.Show("downloadPhotos: " + ex.Message);
                            Log.Write(ex);
                        }
                    }
                }
            }
        }

        public void Calendar()
        {
            CalendarForm CalendarForm = new CalendarForm();                            // создать форму

            CalendarForm.addUsersToCalendar(notFired, dateOfBirthNow, initials, age);  // загрузить данные пользователей в календарь
            CalendarForm.addHolidaysToCalendar(counter, datesOfHolidays, Holidays);    // загрузить данные праздников в календарь
            CalendarForm.addHolidaysToCalendar(countPass, dHolidays, nHolidays);       // загрузить данные переходящих праздников в календарь
            CalendarForm.addHolidaysToCalendar(cChristian, dChristian, nChristian);    // загрузить данные переходящих христианских праздников в календарь

            CalendarForm.ShowDialog();                                                 // открыть форму
        }

        /// <summary>
        /// Метод проверки наличия установленного GoogleChrome и запуска нужной страницы в интернете.
        /// </summary>
        /// <param name="url"></param>
        private void SetupChecks(string url)
        {
            RegistryKey google = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Google");

            if (google != null)
            {
                RegistryKey GoogleChrome = google.OpenSubKey("Chrome");

                if (GoogleChrome != null)
                {
                    // открыть в Google Chrome
                    Process.Start("chrome", url);
                }
            }
            else
                MessageBox.Show("У Вас не установлен браузер Google Chrome.");
        }

        private void пЗОToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupChecks(@"http://pzo.nccp-eng.ru/");
        }

        private void сУПToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupChecks(@"http://sup.nccp-eng.ru/");
        }

        private void метрологToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupChecks(@"http://metrolog.nccp-eng.ru/");
        }

        private void tB_Search_Click(object sender, EventArgs e)
        {
            SearchInTree();                                  // поиск в дереве
        }

        private void tB_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            SearchInTree();                                  // поиск в дереве

            char c = e.KeyChar;

            // можно вводить только кириллицу, Backspace (Возврат на один символ), Space (Пробел) и (): 
            e.Handled = !((c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я')
            || c == 'Ё' || c == 'ё' || c == 8 || c == 32 || c == 40 || c == 41);

            if (e.Handled)  // если введено что то кроме разрешенных символов
            {
                toolTip1.ToolTipIcon = ToolTipIcon.Warning;
                toolTip1.ToolTipTitle = "Внимание!";
                toolTip1.IsBalloon = true;
                toolTip1.BackColor = Color.Yellow;
                toolTip1.ForeColor = Color.Red;
                toolTip1.SetToolTip(tB_Search, "Возможен ввод только русских букв.");
            }
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
                                                pathToServerFolder
                                                });
                        return checkStatus;
                    }
                    catch (Exception ex)
                    {
                        if (f_debugMode) MessageBox.Show("CheckConnection: " + ex.Message);
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

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm SettingsForm = new SettingsForm();            // создать форму
            SettingsForm.ShowDialog();                                 // открыть форму
        }

        /// <summary>
        /// Метод получения переходящих Христианских праздников
        /// </summary>
        private void getPassingСhristianНolidays()
        {
            // Алгоритм расчета Православной Пасхи (дня Воскресения Христова):
            int Y = thisYear;
            int a = Y % 19;    // золотое число G — порядок года в 19-летнем цикле полнолуний
            int b = Y % 4;
            int c = Y % 7;
            int d = (19 * a + 15) % 30;
            int e = (2 * b + 4 * c + 6 * d + 6) % 7;
            int z = d + e;

            int Month = 0;     // месяц Пасхи
            int Day = 0;       // день Пасхи

            /*
            if ((d + e) > 10)
            {
                Month = 4;
                Day = d + e - 9;
            }
            else
            {
                Month = 3;
                Day = 22 + d + e;
            }
            */

            Month = (z + 25) / 35 + 3;
            Day = z + 22 - 31 * (Month / 4);

            // запись Христианских праздников в массивы:
            DateTime dtEasterSunday = new DateTime(Y, Month, Day);    // дата Пасхи в текущем году по старому стилю (Юлианский календарь)
            dtEasterSunday = dtEasterSunday.AddDays(13);              // дата Пасхи в текущем году по новому стилю (Григорианский календарь)
            dChristian[0] = dtEasterSunday;
            nChristian[0] = "Православная Пасха. Светлое Христово Воскресение";

            DateTime dtShroveSunday = new DateTime();
            dtShroveSunday = dtEasterSunday.AddDays(-49);             // дата Прощёного Воскресенья в текущем году
            dChristian[1] = dtShroveSunday;
            nChristian[1] = "Прощёное воскресение";

            DateTime dtBeginningOfGreatLent = new DateTime();
            dtBeginningOfGreatLent = dtEasterSunday.AddDays(-48);     // дата начала Великого Поста (Четыредесятницы) в текущем году
            dChristian[2] = dtBeginningOfGreatLent;
            nChristian[2] = "Начало Великого Поста (Четыредесятницы)";

            DateTime dtPalmSunday = new DateTime();
            dtPalmSunday = dtEasterSunday.AddDays(-7);                // дата Вербного Воскресенья в текущем году
            dChristian[3] = dtPalmSunday;
            nChristian[3] = "Вербное воскресение";

            DateTime dtAscensionOfChrist = new DateTime();
            dtAscensionOfChrist = dtEasterSunday.AddDays(39);         // дата Вознесения Господня в текущем году
            dChristian[4] = dtAscensionOfChrist;
            nChristian[4] = "Вознесение Господне";

            DateTime dtHolyTrinityPentecost = new DateTime();
            dtHolyTrinityPentecost = dtEasterSunday.AddDays(49);      // дата дня Пятидесятницы (день Святой Троицы)
            dChristian[5] = dtHolyTrinityPentecost;
            nChristian[5] = "Пятидесятница (день Святой Троицы)";

            DateTime dtBeginningOfApostlesFast = new DateTime();
            dtBeginningOfApostlesFast = dtEasterSunday.AddDays(57);   // дата начала Петрова поста
            dChristian[6] = dtBeginningOfApostlesFast;
            nChristian[6] = "Начало Петрова Поста";
        }

        /// <summary>
        /// Метод получения переходящих праздников
        /// </summary>
        private void getPassingНolidays()
        {
            int[] DaysInMonths = new int[12];            // массив количеств дней в месяцах

            // записать количества дней в месяцах:
            for (int i = 0; i < 12; i++)
            {
                DaysInMonths[i] = DateTime.DaysInMonth(thisYear, i + 1);
            }

            /////////////////////////////////////ФЕВРАЛЬ///////////////////////////////////////////
            // цикл по дням февраля:
            int february_sunday = 0;                     // переменная воскресенье февраля 

            for (int i = 1; i <= DaysInMonths[1]; i++)   // DaysInMonths[1] - февраль
            {
                DateTime dt = new DateTime(thisYear, (int)months.February, i);

                if (dt.ToString("dddd") == "воскресенье")
                {
                    february_sunday++;

                    // Вс2/02 День Аэрофлота:
                    if (february_sunday == 2)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День Аэрофлота";
                        countPass++;
                    }
                }
            }

            ////////////////////////////////////////МАРТ///////////////////////////////////////////
            // найти количество воскресений марта:
            int count_march_sunday = 0;                  // количество воскресений марта

            for (int i = 1; i <= DaysInMonths[2]; i++)   // DaysInMonths[2] - март
            {
                DateTime dt = new DateTime(thisYear, (int)months.March, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    count_march_sunday++;
                }
            }

            // цикл по дням марта:
            int march_sunday = 0;                        // порядковый номер воскресенья марта

            for (int i = 1; i <= DaysInMonths[2]; i++)   // DaysInMonths[2] - март
            {
                DateTime dt = new DateTime(thisYear, (int)months.March, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    march_sunday++;

                    // Вс2/03/2000 День работников геодезии и картографии:
                    if (march_sunday == 2)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "2000 День работников геодезии и картографии";
                        countPass++;
                    }

                    // Вс3/03 День работников торговли, бытового обслуживания населения и жилищно-коммунального хозяйства:
                    if (march_sunday == 3)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День работников торговли, бытового обслуживания населения и жилищно-коммунального хозяйства";
                        countPass++;
                    }

                    // Вс5/03 В 03.00 Переход на Европейское летнее время (часы переводятся на 1 час вперед):
                    if (march_sunday == count_march_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "В 03.00 Переход на Европейское летнее время (часы переводятся на 1 час вперед)";
                        countPass++;
                    }
                }
            }

            ////////////////////////////////////////АПРЕЛЬ/////////////////////////////////////////
            // найти количество воскресений апреля:
            int count_april_sunday = 0;                  // количество воскресений апреля

            for (int i = 1; i <= DaysInMonths[3]; i++)   // DaysInMonths[3] - апрель
            {
                DateTime dt = new DateTime(thisYear, (int)months.April, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    count_april_sunday++;
                }
            }

            // цикл по дням апреля:
            int april_sunday = 0;                        // порядковый номер воскресенья апреля

            for (int i = 1; i <= DaysInMonths[3]; i++)   // DaysInMonths[3] - апрель
            {
                DateTime dt = new DateTime(thisYear, (int)months.April, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    april_sunday++;

                    // Вс1/04 День геолога:
                    if (april_sunday == 1)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День геолога";
                        countPass++;
                    }

                    // Вс2/04 День войск противовоздушной обороны страны:
                    if (april_sunday == 2)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День войск противовоздушной обороны страны";
                        countPass++;
                    }

                    // Вс5/04 Всемирный день породненных городов:
                    if (april_sunday == count_april_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "Всемирный день породненных городов";
                        countPass++;
                    }
                }
            }

            //////////////////////////////////////////МАЙ/////////////////////////////////////////
            // найти количество воскресений мая:
            int count_may_sunday = 0;                    // количество воскресений мая

            for (int i = 1; i <= DaysInMonths[4]; i++)   // DaysInMonths[4] - май
            {
                DateTime dt = new DateTime(thisYear, (int)months.May, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    count_may_sunday++;
                }
            }

            // цикл по дням мая:
            int may_sunday = 0;                          // порядковый номер воскресенья мая

            for (int i = 1; i <= DaysInMonths[4]; i++)   // DaysInMonths[4] - май
            {
                DateTime dt = new DateTime(thisYear, (int)months.May, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    may_sunday++;

                    // Вс5/05 День химика:
                    if (may_sunday == count_may_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День химика";
                        countPass++;
                    }
                }
            }

            /////////////////////////////////////////ИЮНЬ/////////////////////////////////////////
            // найти количество суббот и воскресений июня:
            int count_june_saturday = 0;                 // количество суббот июня
            int count_june_sunday = 0;                   // количество воскресений июня

            for (int i = 1; i <= DaysInMonths[5]; i++)   // DaysInMonths[5] - июнь
            {
                DateTime dt = new DateTime(thisYear, (int)months.June, i);

                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    count_june_saturday++;
                }

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    count_june_sunday++;
                }
            }

            // цикл по дням июня:
            int june_saturday = 0;                       // порядковый номер субботы июня
            int june_sunday = 0;                         // порядковый номер воскресенья июня

            for (int i = 1; i <= DaysInMonths[5]; i++)   // DaysInMonths[5] - июнь
            {
                DateTime dt = new DateTime(thisYear, (int)months.June, i);

                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    june_saturday++;

                    // Сб5/06 День изобретателя и рационализатора:
                    if (june_saturday == count_june_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День изобретателя и рационализатора";
                        countPass++;
                    }
                }

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    june_sunday++;

                    // Вс1/06 День мелиоратора:
                    if (june_sunday == 1)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День мелиоратора";
                        countPass++;
                    }

                    // Вс2/06 День работников легкой промышленности:
                    if (june_sunday == 2)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День работников легкой промышленности";
                        countPass++;
                    }

                    // Вс3/06 День медицинского работника:
                    if (june_sunday == 3)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День медицинского работника";
                        countPass++;
                    }
                }
            }

            /////////////////////////////////////////ИЮЛЬ/////////////////////////////////////////
            // найти количество пятниц, суббот и воскресений июля:
            int count_july_friday = 0;                   // количество пятниц июля
            int count_july_saturday = 0;                 // количество суббот июля
            int count_july_sunday = 0;                   // количество воскресений июля

            for (int i = 1; i <= DaysInMonths[6]; i++)   // DaysInMonths[6] - июль
            {
                DateTime dt = new DateTime(thisYear, (int)months.July, i);

                if (dt.DayOfWeek == DayOfWeek.Friday)
                {
                    count_july_friday++;
                }

                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    count_july_saturday++;
                }

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    count_july_sunday++;
                }
            }

            // цикл по дням июля:
            int july_friday = 0;                         // порядковый номер пятницы июля
            int july_saturday = 0;                       // порядковый номер субботы июля
            int july_sunday = 0;                         // порядковый номер воскресенья июля

            for (int i = 1; i <= DaysInMonths[6]; i++)   // DaysInMonths[6] - июль
            {
                DateTime dt = new DateTime(thisYear, (int)months.July, i);

                if (dt.DayOfWeek == DayOfWeek.Friday)
                {
                    july_friday++;

                    // Пт5/07 Международный день сисадмина:
                    if (july_friday == count_july_friday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "Международный день сисадмина";
                        countPass++;
                    }
                }

                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    july_saturday++;

                    // Сб1/07 Международный день кооперации:
                    if (july_saturday == 1)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "Международный день кооперации";
                        countPass++;
                    }
                }

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    july_sunday++;

                    // Вс1/07 День работников морского и речного флота:
                    if (july_sunday == 1)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День работников морского и речного флота";
                        countPass++;
                    }

                    // Вс2/07 День рыбака:
                    if (july_sunday == 2)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День рыбака";
                        countPass++;
                    }

                    // Вс3/07 День металлурга:
                    if (july_sunday == 3)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День металлурга";
                        countPass++;
                    }

                    // Вс5/07 День Военно-Морского Флота:
                    if (july_sunday == count_july_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День Военно-Морского Флота";
                        countPass++;
                    }
                }
            }

            /////////////////////////////////////////АВГУСТ////////////////////////////////////////
            // найти количество воскресений августа:
            int count_august_sunday = 0;                 // количество воскресений августа

            for (int i = 1; i <= DaysInMonths[7]; i++)   // DaysInMonths[7] - август
            {
                DateTime dt = new DateTime(thisYear, (int)months.August, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    count_august_sunday++;
                }
            }

            // цикл по дням августа:
            int august_sunday = 0;                       // порядковый номер воскресенья августа

            for (int i = 1; i <= DaysInMonths[7]; i++)   // DaysInMonths[7] - август
            {
                DateTime dt = new DateTime(thisYear, (int)months.August, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    august_sunday++;

                    // Вс1/08 День железнодорожника:
                    if (august_sunday == 1)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День железнодорожника";
                        countPass++;
                    }

                    // Вс2/08 День строителя:
                    if (august_sunday == 2)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День строителя";
                        countPass++;
                    }

                    // Вс3/08 День Воздушного Флота России:
                    if (august_sunday == 3)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День Воздушного Флота России";
                        countPass++;
                    }

                    // Вс5/08 День шахтера:
                    if (august_sunday == count_august_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День шахтера";
                        countPass++;
                    }
                }
            }

            ///////////////////////////////////////СЕНТЯБРЬ///////////////////////////////////////
            // найти количество воскресений сентября:
            int count_september_sunday = 0;              // количество воскресений сентября

            for (int i = 1; i <= DaysInMonths[8]; i++)   // DaysInMonths[8] - сентябрь
            {
                DateTime dt = new DateTime(thisYear, (int)months.September, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    count_september_sunday++;
                }
            }

            // цикл по дням сентября:
            int september_sunday = 0;                    // порядковый номер воскресенья сентября

            for (int i = 1; i <= DaysInMonths[8]; i++)   // DaysInMonths[8] - сентябрь
            {
                DateTime dt = new DateTime(thisYear, (int)months.September, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    september_sunday++;

                    // Вс1/09 День работников нефтяной и газовой промышленности:
                    if (september_sunday == 1)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День работников нефтяной и газовой промышленности";
                        countPass++;
                    }

                    // Вс2/09 День танкистов:
                    if (september_sunday == 2)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День танкистов";
                        countPass++;
                    }

                    // Вс3/09 День работников леса:
                    if (september_sunday == 3)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День работников леса";
                        countPass++;
                    }

                    // Вс5/09 День машиностроителя:
                    if (september_sunday == count_september_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День машиностроителя";
                        countPass++;
                    }
                }
            }

            ////////////////////////////////////////ОКТЯБРЬ///////////////////////////////////////
            // найти количество воскресений октября:
            int count_october_sunday = 0;                // количество воскресений октября

            for (int i = 1; i <= DaysInMonths[9]; i++)   // DaysInMonths[9] - октябрь
            {
                DateTime dt = new DateTime(thisYear, (int)months.October, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    count_october_sunday++;
                }
            }

            // цикл по дням октября:
            int october_monday = 0;                      // порядковый номер понедельника октября
            int october_sunday = 0;                      // порядковый номер воскресенья октября

            for (int i = 1; i <= DaysInMonths[9]; i++)   // DaysInMonths[9] - октябрь
            {
                DateTime dt = new DateTime(thisYear, (int)months.October, i);

                if (dt.DayOfWeek == DayOfWeek.Monday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    october_monday++;

                    // Пн1/10 Международный день врача:
                    if (october_monday == 1)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "Международный день врача";
                        countPass++;
                    }
                }

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    october_sunday++;

                    // Вс2/10 День работников сельского хозяйства и перерабатывающей промышленности:
                    if (october_sunday == 2)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День работников сельского хозяйства и перерабатывающей промышленности";
                        countPass++;
                    }

                    // Вс5/10 В 03.00 Переход на Европейское зимнее время (часы переводятся на 1 час назад):
                    if (october_sunday == count_october_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "В 03.00 Переход на Европейское зимнее время (часы переводятся на 1 час назад)";
                        countPass++;
                    }

                    // Вс5/10 День работников автомобильного транспорта и дорожного хозяйства:
                    if (october_sunday == count_october_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День работников автомобильного транспорта и дорожного хозяйства";
                        countPass++;
                    }

                    // Вс5/10 День инженера-механика:
                    if (october_sunday == count_october_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День инженера-механика";
                        countPass++;
                    }
                }
            }

            /////////////////////////////////////////НОЯБРЬ///////////////////////////////////////
            // найти количество воскресений ноября:
            int count_november_sunday = 0;               // количество воскресений ноября

            for (int i = 1; i <= DaysInMonths[10]; i++)  // DaysInMonths[10] - ноябрь
            {
                DateTime dt = new DateTime(thisYear, (int)months.November, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    count_november_sunday++;
                }
            }

            // цикл по дням ноября:
            int november_sunday = 0;                     // порядковый номер воскресенья ноября

            for (int i = 1; i <= DaysInMonths[10]; i++)  // DaysInMonths[10] - ноябрь
            {
                DateTime dt = new DateTime(thisYear, (int)months.November, i);

                if (dt.DayOfWeek == DayOfWeek.Sunday)    // if (dt.ToString("dddd") == "воскресенье")
                {
                    november_sunday++;

                    // Вс3/11 День Ракетных войск и артиллерии:
                    if (november_sunday == 3)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День Ракетных войск и артиллерии";
                        countPass++;
                    }

                    // Вс5/11 День матери:
                    if (november_sunday == count_november_sunday)
                    {
                        dHolidays[countPass] = dt;
                        nHolidays[countPass] = "День матери";
                        countPass++;
                    }
                }
            }

            // изменить размерность массивов:
            Array.Resize(ref dHolidays, countPass);
            Array.Resize(ref nHolidays, countPass);
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown)
            {
                // свернуть в трей при попытке закрытия формы:
                e.Cancel = true;
                MinimizeMainForm();                               // свернуть окно
            }
        }

        /*
        Вы не можете запретить запуск нового экземпляра, но этот самый новый экземпляр может проверить
        наличие другого экземпляра и выйти. Например, используя именованный Mutex.
        */
        // держим в переменной, чтобы сохранить владение им до конца пробега программы:
        static Mutex InstanceCheckMutex;
        static bool InstanceCheck()
        {
            bool isNew;
            InstanceCheckMutex = new Mutex(true, "Birthday", out isNew);
            return isNew;
        }

        private void bBI_Sorting__ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BuildTree();           // вывести дерево подразделений и пользователей
            SearchInTree();        // поиск в дереве
        }

        private void bBI_Settings__ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();            // создать форму
            settingsForm.ShowDialog();                                 // открыть форму  
        }

        private void bBI_PZO__ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetupChecks(@"http://pzo.nccp-eng.ru/");
        }

        private void bBI_SUP__ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetupChecks(@"http://sup.nccp-eng.ru/");
        }

        private void bBI_Metrolog__ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetupChecks(@"http://metrolog.nccp-eng.ru/");
        }

        private void bBI_Exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Exit();                                          // закрыть программу
        }

        public void Exit()                                   // закрыть программу
        {
            notifyIcon1.Visible = false;                     // иконка в трее не видна

            // Сначала закрыть remindForm, а потом и всё приложение:
            CloseRemindForm();
            Application.Exit();
        }

        private void bBI_Calendar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Calendar();
        }

        private void bBI_About_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutForm aboutForm = new AboutForm();                          // создать форму
            aboutForm.ShowDialog();                                         // открыть форму  
        }

        private void bBI_Skins_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SkinsForm skinsForm = new SkinsForm();                          // создать форму
            skinsForm.ShowDialog();                                         // открыть форму
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Exit();                                                         // закрыть программу
        }

        private void t_MainForm_Tick(object sender, EventArgs e)
        {
            MinimizeMainForm();
        }

        public void MinimizeMainForm()
        {
            this.WindowState = FormWindowState.Minimized;            // свернуть окно
            this.ShowInTaskbar = false;                              // убрать отображение окна в панели задач Windows
            notifyIcon1.Visible = true;                              // иконка в трее видна
            t_MainForm.Enabled = false;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            t_MainForm.Enabled = false;
        }

        private void bBI_Reference_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            F_Error f_Error = new F_Error();                           // создать форму
            f_Error.ShowDialog();                                      // открыть форму  
        }

        /// <summary>
        /// Метод запуска отсчёта времени до следующего напоминания.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remindTimer_Tick(object sender, EventArgs e)
        {
            ShowEvents();
            remindTimer.Enabled = true;
        }

        private void notifyIcon1_BalloonTipClicked_1(object sender, EventArgs e)
        {
            ShowRemindForm();
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}