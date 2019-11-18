using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;
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
namespace BirthdayMainModule
{
    /// <summary>
    /// class MainForm - основной класс программы.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        string NewL = Environment.NewLine;
        bool fl = false;  // флаг переключения сортировки

        int notFired = 0;  // число неуволенных пользователей
        int teams = 0;     // число подразделений
 
        int thisYear = Convert.ToInt32(DateTime.Now.Year.ToString());

        Dictionary<string, usersGetClass> usersGetArray  = new Dictionary<string, usersGetClass>();  // словарь пользователей
        Dictionary<string, teamsGetClass> teamsGetArray  = new Dictionary<string, teamsGetClass>();  // словарь подразделений
        Dictionary<string, jobsGetClass>  jobsGetArray   = new Dictionary<string, jobsGetClass>();   // словарь профессий

        // новый экземпляр класса authStartClass для получения информации об авторизации сотрудника по IP адресу:
        authStartClass authStartArray = new authStartClass();   

        // Создать массивы имён, ключей и родителей подразделений: 
        string[] namesOfTeams = new string[100];
        int[] keysOfTeams = new int[100];
        int[] parentsOfTeams = new int[100];

        // Создать массивы фамилий, подразделений, ключей дней рождений пользователей: 
        string[] arrOfNames = new string[300]; 
        string[] Initials = new string[300];
        int[] arrOfTeams = new int[300];
        int[] arrOfKeys = new int[300];
        string[] arrOfDobs = new string[300];

        DateTime[] date = new DateTime[300];  // массив дат

        /// <summary>
        /// Метод, реализующий post-запрос к API.
        /// </summary>
        /// <param name="postedData">Запрашиваемые данные.</param>
        /// <param name="postUrl">Запрашиваемый адрес.</param>
        /// <returns>Возвращает ответ API на данные postedData по адресу postUrl.</returns>
        public static HttpWebResponse PostMethod(string postedData, string postUrl)
        {
            // создать post-запрос:
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
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
            usersGet();   // получить информацию о пользователях
            authStart();  // получить информацию об авторизации сотрудника по IP адресу
            teamsGet();   // получить информацию о подразделениях
            BuildTree();  // вывести дерево подразделений и пользователей
        }

        /// <summary>
        /// Метод вывода дерева подразделений и пользователей.
        /// </summary>
        private void BuildTree()
        {
            if (fl) fl = false;
            else fl = true;
            ///////////////Подготовить и записать в массивы инфо о сотрудниках и подразделениях/////////////
            DataTable dt = new DataTable();                     // создание таблицы данных в памяти
            BindingSource bindingSource = new BindingSource();  // создание источника данных
            bindingSource.DataSource = dt;

            // найти число подразделений:
            foreach (KeyValuePair<string, teamsGetClass> kvp in teamsGetArray)
            //или кратко foreach (var kvp in array1)
            {
                teams++;  // число подразделений
            }

            // найти число неуволенных пользователей:
            foreach (KeyValuePair<string, usersGetClass> kvp in usersGetArray)
            //или кратко foreach (var kvp in array)
            {
                int fired = Convert.ToInt32(kvp.Value.fired);
                if (fired == 0)
                {
                    notFired++;  // число неуволенных пользователей
                }
            }

            Array.Resize(ref namesOfTeams, teams);    // изменить размерность массива namesOfTeams  
            Array.Resize(ref keysOfTeams, teams);     // изменить размерность массива keysOfTeams
            Array.Resize(ref parentsOfTeams, teams);  // изменить размерность массива parentsOfTeams

            Array.Resize(ref arrOfNames, notFired);   // изменить размерность массива arrOfNames
            Array.Resize(ref Initials, notFired);
            Array.Resize(ref arrOfTeams, notFired);   // изменить размерность массива arrOfTeams
            Array.Resize(ref arrOfKeys, notFired);    // изменить размерность массива arrOfKeys

            // записать названия, ключи и родители подразделений в массивы:
            int index = 0;                            // индекс элемента словаря
            foreach (KeyValuePair<string, teamsGetClass> kvp in teamsGetArray)
            //или кратко foreach (var kvp in array1)
            {
                namesOfTeams[index] = Convert.ToString(kvp.Value.name);      // записать название подразделения
                keysOfTeams[index] = Convert.ToInt32(kvp.Key);               // записать ключ подразделения
                parentsOfTeams[index] = Convert.ToInt32(kvp.Value.parent);   // записать родителя подразделения

                index++;
            }

            // массивы лет, месяцев и дней:
            int[] years = new int[notFired];
            int[] months = new int[notFired];
            int[] days = new int[notFired];
            int[] age = new int[notFired]; 

            Array.Resize(ref date, notFired);    // изменить размерность массива date

            // записать названия, ключи и подразделения пользователей в массивы:
            int index1 = 0;                           // индекс элемента словаря
            foreach (KeyValuePair<string, usersGetClass> kvp in usersGetArray)
            //или кратко foreach (var kvp in array)
            {
                int fired = Convert.ToInt32(kvp.Value.fired);
                if (fired == 0)                                              // если пользователь не уволен
                {
                    arrOfNames[index1] = kvp.Value.name1 +" "+ kvp.Value.name2 + " " + kvp.Value.name3;   // записать полные имена пользователей в массив arrOfNames
                    arrOfTeams[index1] = Convert.ToInt32(kvp.Value.team);    // записать подразделения пользователей в массив arrOfTeams
                    arrOfKeys[index1] = Convert.ToInt32(kvp.Key);            // записать ключи пользователей в массив arrOfKeys
                    arrOfDobs[index1] = Convert.ToString(kvp.Value.dob);     // записать дни рождения пользователей в массив arrOfDobs

                    index1++;
                }
            }

            try {
                // заполнение массивов лет, месяцев, дней и дат рождений:
                for (int i = 0; i < notFired; i++)
                {
                    years[i] = Convert.ToInt32(arrOfDobs[i].Substring(0, 4));
                    months[i] = Convert.ToInt32(arrOfDobs[i].Substring(5, 2));
                    days[i] = Convert.ToInt32(arrOfDobs[i].Substring(8, 2));

                    date[i] = new DateTime(thisYear, months[i], days[i]);
                }
            }
            catch{}
            /*
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            */

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
                    Initials[index3] = kvp.Value.name1 + " " + kvp.Value.name2.Substring(0, 1) + ". " 
                        + kvp.Value.name3.Substring(0, 1)+ "." + " (" + Convert.ToString(age[index3]) + ")";

                    index3++;
                }
            }


            ////////////////////////////////////////////////////////////////////////////////////////////////
            if (fl)
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

                FillTreeView2(treeView1, bindingSource);     // заполнить дерево из таблицы
            }
            else
            {
                // создание столбцов таблицы данных в памяти:
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("Value", typeof(string));
                dt.Columns.Add("Parent_Id", typeof(int));

                //  создание строк таблицы данных в памяти, заполненных инфо о сотрудниках и подразделениях:
                dt.Rows.Add(1, "НЗХК-Инжиниринг", 0);        // добавить строку корневого узла в таблицу

                foreach (KeyValuePair<string, teamsGetClass> kvp in teamsGetArray)  // перебор по элементам словаря teamsGetArray
                //или кратко foreach (var kvp in array1)
                {
                    string teamName = Convert.ToString(kvp.Value.name);      // название подразделения
                    int teamKey = Convert.ToInt32(kvp.Key);                  // ключ подразделения
                    int teamParent = Convert.ToInt32(kvp.Value.parent);      // родитель подразделения

                    if (teamKey != 100)                                      // если не "Не использовать"
                    {
                        dt.Rows.Add(teamKey, teamName, teamParent);          // добавить строку с инфо о подразделении в таблицу

                        for (int j = 0; j < notFired; j++)                   // перебор всех пользователей
                        {
                            if (arrOfTeams[j] == teamKey)                    // если номер подразделения пользователя = ключу подразделения
                            {
                                dt.Rows.Add(arrOfKeys[j], arrOfNames[j], teamKey);  // добавить строку с инфо о пользователе в таблицу
                            }
                        }
                    }
                }

                FillTreeView2(treeView1, bindingSource);                     // заполнить дерево из таблицы
            }
            notifyIcon1.Visible = false;                                     // иконка в трее не видна
        }

        /// <summary>
        ///  Заполнение дерева из БД.
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="bindingSource"></param>

        private void FillTreeView2(TreeView treeView, BindingSource bindingSource)
        {
            treeView.Nodes.Clear();
            bindingSource.Position = 0;
            for (int i = 0; i < bindingSource.Count; i++)
            {
                //Application.DoEvents();
                DataRow dr = (bindingSource.Current as DataRowView).Row;

                int id = Convert.ToInt32(dr["id"]);
                int parent_id = Convert.ToInt32(dr["parent_id"]);
                string name = dr["value"].ToString();

                if (parent_id == 0)
                {
                    treeView.Nodes.Add(FillNode2(id, name));
                }
                else
                {
                    TreeNode[] treeNodes = treeView.Nodes.Find(parent_id.ToString(), true);
                    if (treeNodes.Length != 0)
                        (treeNodes.GetValue(0) as TreeNode).Nodes.Add(FillNode2(id, name));
                }
                bindingSource.MoveNext();
            }
        }

        /// <summary>
        /// Создание нового узла.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>TreeNode</returns>
        private TreeNode FillNode2(int id, string name)
        {
            TreeNode treeNode = new TreeNode();
            treeNode.Tag = id;
            treeNode.Name = id.ToString();
            treeNode.Text = name;
            return treeNode;
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode.Text != "НЗХК-Инжиниринг")  // если не корневой узел
                {
                    if (treeView1.SelectedNode.FirstNode == null)      // если нет дочерних узлов
                    {
                        UserForm UserForm = new UserForm();            // создать форму

                        jobsGet();                                     // получить информацию о профессиях

                        string[] arrOfJobs = new string[500];          // массив названий профессий
                        int[] arrOfKeyJobs = new int[500];             // массив ключей профессий

                        // записать названия и ключи профессий в массивы, а так же найти число профессий:
                        int jobs = 0;                                  // индекс элементов словаря
                        foreach (KeyValuePair<string, jobsGetClass> kvp in jobsGetArray)
                        //или кратко foreach (var kvp in array)
                        {
                            arrOfJobs[jobs] = kvp.Value.name;
                            arrOfKeyJobs[jobs] = Convert.ToInt32(kvp.Key);
                            jobs++;
                        }
                        
                        Array.Resize(ref arrOfJobs, jobs);             // изменить размерность массива arrOfJobs
                        Array.Resize(ref arrOfKeyJobs, jobs);          // изменить размерность массива arrOfKeyJobs

                        // передать данные о сотруднике в форму UserForm:
                        foreach (KeyValuePair<string, usersGetClass> kvp in usersGetArray)
                        //или кратко foreach (var kvp in array)
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
                            
                            string teamS = null;              // строка для названия подразделения
                            string jobS = null;               // строка для названия профессии

                            // записать название подразделения в строку:
                            for (int i=0; i< teams; i++)
                            {
                                if (keysOfTeams[i] == team)   // если ключ подразделения равен идентификатору подразделения
                                    teamS = namesOfTeams[i];  // записать в строку название подразделения
                            }

                            // записать название профессии в строку:
                            for (int i = 0; i < jobs; i++)
                            {
                                if (arrOfKeyJobs[i] == job)   // если ключ профессии равен идентификатору профессии
                                    jobS = arrOfJobs[i];      // записать в строку название профессии
                            }

                            string pathToPhoto= "http://api.nccp-eng.ru/photo/" + Key + ".jpg";

                            // если выбран сотрудник:
                            if (treeView1.SelectedNode.Text == name1 + " " + name2 + " " + name3)
                             {
                                 UserForm.showUserInfo(name1, name2, name3, teamS, jobS, phone1, phone2, dob, computer, login, email, pathToPhoto);
                             }
                        }                         
                        UserForm.ShowDialog();                // открыть форму
                    }
                }
            }
            catch {}
        }

        /// <summary>
        /// Нажатие на кнопку сортировки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bBI_Sorting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BuildTree();                                      // вывести дерево подразделений и пользователей
        }

        /// <summary>
        /// Нажатие на кнопку свернуть в трей.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bBI_MinimizeToTray_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            WindowState = FormWindowState.Minimized;          // свернуть окно
            this.ShowInTaskbar = false;                       // убрать отображение окна в панели
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

                }
                else                                                  // если окно активно
                {
                    this.WindowState = FormWindowState.Minimized;     // свернуть окно
                    this.ShowInTaskbar = false;                       // убрать отображение окна в панели задач Windows
                    notifyIcon1.Visible = true;                       // иконка в трее видна
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

        private void MainForm_Deactivate_1(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) // если окно свёрнуто
            {
                this.ShowInTaskbar = false;                    // убрать отображение окна в панели задач Windows
                notifyIcon1.Visible = true;                    // иконка в трее видна
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
            string SearchText = this.tB_Search.Text;
            if (SearchText == "")
            {
                return;
            };
            TreeNode SelectedNode = SearchNode(SearchText, treeView1.Nodes[0]); // пытаемся найти в поле Text
            if (SelectedNode != null)
            {
                //нашли, выделяем...
                this.treeView1.SelectedNode = SelectedNode;
                this.treeView1.SelectedNode.Expand();
                this.treeView1.Select();
                tB_Search.Focus();                                              // фокус на строке поиска
            };

        }

        private void bBI_Calendar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Calendar();
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
            // Получить информацию о пользователях:
            string usersGetMeth = "method=users.get";
            string usersGetResponse = null;

            // выполнить post-запрос:
            var uResponse = PostMethod(usersGetMeth, "http://api.nccp-eng.ru/");
            if (uResponse != null)
            {
                var strreader = new StreamReader(uResponse.GetResponseStream(), Encoding.UTF8);
                usersGetResponse = strreader.ReadToEnd();
            }
/*
            // записать полученный json с информацией о пользователях в файл:
            StreamWriter uSW = new StreamWriter(new FileStream(@"usersGet.json", FileMode.Create, FileAccess.Write));
            uSW.Write(usersGetResponse);
            uSW.Close();
*/
            // парсить полученный json с информацией о пользователях:
            string uJson = usersGetResponse;
            usersGetArray = JsonConvert.DeserializeObject<Dictionary<string, usersGetClass>>(uJson);

            // отсортировать пользователей по фамилии в алфавитном порядке 
            usersGetArray = usersGetArray.OrderBy(pair => pair.Value.name1).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        /// Метод получения информации об авторизации сотрудника по IP адресу.
        /// </summary>
        private void authStart()
        {
            // Получить имя компьютера
            string host = Dns.GetHostName();

            // Получить ip-адрес
            IPAddress entIP = Dns.GetHostByName(host).AddressList[0];

            //Получить табельный номер сотрудника по IP адресу:
            string authStartMeth = "method=auth.start&ip=";
            string param = authStartMeth + entIP;
            string authResponse = null;

            // выполнить post-запрос:
            var aResponse = PostMethod(param, "http://api.nccp-eng.ru/");
            if (aResponse != null)
            {
                var strreader = new StreamReader(aResponse.GetResponseStream(), Encoding.UTF8);
                authResponse = strreader.ReadToEnd();
            }

            // парсить полученный json с информацией об авторизации сотрудника по IP адресу:
            string aJson = authResponse;
            authStartArray = JsonConvert.DeserializeObject<authStartClass>(aJson);

            // проверка наличия в системе по табельному номеру:
            int index = authStartArray.number;  // табельный номер
            string s = Convert.ToString(index);
            string userName = usersGetArray[s].name2 + " " + usersGetArray[s].name1;

            if (authStartArray.number != 0)
            {
                l_UserName.Text = "Здравствуйте, " + userName + "!";
            }
            else l_UserName.Text = "Здравствуйте, Гость!";
        }


        /// <summary>
        /// Метод получения информации о подразделениях.
        /// </summary>
        private void teamsGet()
        {
            // Получить информацию о подразделениях:
            string teamsGetMeth = "method=teams.get";
            string teamsGetResponse = null;

            // выполнить post-запрос:
            var tResponse = PostMethod(teamsGetMeth, "http://api.nccp-eng.ru/");
            if (tResponse != null)
            {
                var strreader = new StreamReader(tResponse.GetResponseStream(), Encoding.UTF8);
                teamsGetResponse = strreader.ReadToEnd();
            }
/*
            // записать полученный json с информацией о подразделениях в файл:
            StreamWriter tSW = new StreamWriter(new FileStream(@"teamsGet.json", FileMode.Create, FileAccess.Write));
            tSW.Write(teamsGetResponse);
            tSW.Close();
*/
            // парсить полученный json с информацией о подразделениях:
            string tJson = teamsGetResponse;
            teamsGetArray = JsonConvert.DeserializeObject<Dictionary<string, teamsGetClass>>(tJson);

            // отсортировать подразделения в алфавитном порядке:
            teamsGetArray = teamsGetArray.OrderBy(pair => pair.Value.name).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private void jobsGet()
        {
            // Получить информацию о профессиях:
            string jobsGetMeth = "method=jobs.get";
            string jobsGetResponse = null;

            // выполнить post-запрос:
            var jResponse = PostMethod(jobsGetMeth, "http://api.nccp-eng.ru/");
            if (jResponse != null)
            {
                var strreader = new StreamReader(jResponse.GetResponseStream(), Encoding.UTF8);
                jobsGetResponse = strreader.ReadToEnd();
            }

            // парсить полученный json с информацией о пользователях:
            string jJson = jobsGetResponse;
            jobsGetArray = JsonConvert.DeserializeObject<Dictionary<string, jobsGetClass>>(jJson);

            // отсортировать в алфавитном порядке 
            jobsGetArray = jobsGetArray.OrderBy(pair => pair.Value.name).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public void Calendar()
        {
            CalendarForm CalendarForm = new CalendarForm();            // создать форму
            CalendarForm.showCalendar(notFired, date, Initials);     // загрузить данные календаря
            CalendarForm.ShowDialog();                                 // открыть форму
        }
    }
}