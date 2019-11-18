using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
        string uJson;
        string tJson;

        Dictionary<string, usersGetClass> usersGetArray = new Dictionary<string, usersGetClass>();   // словарь пользователей
        Dictionary<string, teamsGetClass> teamsGetArray = new Dictionary<string, teamsGetClass>();   // словарь подразделений

        // Создать массивы имён, ключей и родителей подразделений: 
        string[] namesOfTeams = new string[100];
        int[] keysOfTeams = new int[100];
        int[] parentsOfTeams = new int[100];

        // Создать массивы фамилий и подразделений пользователей: 
        string[] arrOfNames1 = new string[100];
        int[] arrOfTeams = new int[100];
        int[] arrOfKeys = new int[100];

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
            ////////////////////////////////Алгоритм авторизации сотрудника/////////////////////////////////
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
            Dictionary<string, usersGetClass> usersGetArray = JsonConvert.DeserializeObject<Dictionary<string, usersGetClass>>(uJson);

            // отсортировать пользователей по фамилии в алфавитном порядке 
            usersGetArray = usersGetArray.OrderBy(pair => pair.Value.name1).ToDictionary(pair => pair.Key, pair => pair.Value);

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
            authStartClass authStartArray = JsonConvert.DeserializeObject<authStartClass>(aJson);

            // проверка наличия в системе по табельному номеру:
            int index = authStartArray.number;  // табельный номер
            string s = Convert.ToString(index);
            string userName = null;
            try
            {
                userName = usersGetArray[s].name2 + " " + usersGetArray[s].name1;
            }
            catch(Exception ex)
            {
                l_UserName.Text = "Здравствуйте, Гость!";
            }

            if (authStartArray.number != 0)
            {
                l_UserName.Text = "Здравствуйте, " + userName + "!";
            }
            else l_UserName.Text = "Здравствуйте, Гость!";
            ////////////////////////////////////////////////////////////////////////////////////////////////

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
            Dictionary<string, teamsGetClass> teamsGetArray = JsonConvert.DeserializeObject<Dictionary<string, teamsGetClass>>(tJson);

            // отсортировать подразделения в алфавитном порядке:
            teamsGetArray = teamsGetArray.OrderBy(pair => pair.Value.name).ToDictionary(pair => pair.Key, pair => pair.Value);

            BuildTree();  // вывести дерево подразделений и пользователей
        }

        private void b_Sorting_Click(object sender, EventArgs e)
        {
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
            // записать полученный json с информацией о пользователях в файл:
            StreamWriter uSW = new StreamWriter(new FileStream(@"usersGet.json", FileMode.Create, FileAccess.Write));
            uSW.Write(usersGetResponse);
            uSW.Close();
*/
            // парсить полученный json с информацией о подразделениях:
            string tJson = teamsGetResponse;
            Dictionary<string, teamsGetClass> teamsGetArray = JsonConvert.DeserializeObject<Dictionary<string, teamsGetClass>>(tJson);

            // отсортировать подразделения в алфавитном порядке:
            teamsGetArray = teamsGetArray.OrderBy(pair => pair.Value.name).ToDictionary(pair => pair.Key, pair => pair.Value);

            // найти число подразделений:
            int teams = 0;
            foreach (KeyValuePair<string, teamsGetClass> kvp in teamsGetArray)
            //или кратко foreach (var kvp in array1)
            {
                teams++;  // число подразделений
            }

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
            Dictionary<string, usersGetClass> usersGetArray = JsonConvert.DeserializeObject<Dictionary<string, usersGetClass>>(uJson);

            // отсортировать пользователей по фамилии в алфавитном порядке 
            usersGetArray = usersGetArray.OrderBy(pair => pair.Value.name1).ToDictionary(pair => pair.Key, pair => pair.Value);

            // найти число неуволенных пользователей:
            int notFired = 0;
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

            Array.Resize(ref arrOfNames1, notFired);  // изменить размерность массива arrOfNames1
            Array.Resize(ref arrOfTeams, notFired);   // изменить размерность массива arrOfTeams
            Array.Resize(ref arrOfKeys, notFired);    // изменить размерность массива arrOfKeys

            // поэлементный перебор словаря:
            int index = 0;                            // индекс элемента словаря

            foreach (KeyValuePair<string, teamsGetClass> kvp in teamsGetArray)
            //или кратко foreach (var kvp in array1)
            {
                namesOfTeams[index] = Convert.ToString(kvp.Value.name);      // записать название подразделения
                keysOfTeams[index] = Convert.ToInt32(kvp.Key);               // записать ключ подразделения
                parentsOfTeams[index] = Convert.ToInt32(kvp.Value.parent);   // записать родителя подразделения

                index++;
            }

            // поэлементный перебор словаря:
            int index1 = 0;                            // индекс элемента словаря

            foreach (KeyValuePair<string, usersGetClass> kvp in usersGetArray)
            //или кратко foreach (var kvp in array)
            {
                int fired = Convert.ToInt32(kvp.Value.fired);
                if (fired == 0)                                             // если пользователь не уволен
                {
                    arrOfNames1[index1] = kvp.Value.name1 +" "+ kvp.Value.name2 + " " + kvp.Value.name3;   // запись фамилий в массив arrOfNames1
                    arrOfTeams[index1] = Convert.ToInt32(kvp.Value.team);   // запись номеров подразделений в массив arrOfTeams, соответствующих фамилии пользователя
                    arrOfKeys[index1] = Convert.ToInt32(kvp.Key);
                    index1++;
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
                dt.Rows.Add(1, "НЗХК-Инжиниринг", 0);        // добавить строку корневого узла в таблицу

                int index2 = 0;
                foreach (string s in arrOfNames1)            // перебор по строкам массива arrOfNames1[]
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
                                dt.Rows.Add(arrOfKeys[j], arrOfNames1[j], teamKey);  // добавить строку с инфо о пользователе в таблицу
                            }
                        }
                    }
                }

                FillTreeView2(treeView1, bindingSource);                     // заполнить дерево из таблицы
            }
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
                        ////////////////////////////////////////////////////////////////////////////////////////////////
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

                        // парсить полученный json с информацией о подразделениях:
                        string tJson = teamsGetResponse;
                        Dictionary<string, teamsGetClass> teamsGetArray = JsonConvert.DeserializeObject<Dictionary<string, teamsGetClass>>(tJson);

                        // отсортировать подразделения в алфавитном порядке:
                        teamsGetArray = teamsGetArray.OrderBy(pair => pair.Value.name).ToDictionary(pair => pair.Key, pair => pair.Value);

                        // поэлементный перебор словаря:
                        int teams = 0;                           // индекс элемента словаря

                        foreach (KeyValuePair<string, teamsGetClass> kvp in teamsGetArray)
                        //или кратко foreach (var kvp in array1)
                        {
                            namesOfTeams[teams] = Convert.ToString(kvp.Value.name);      // записать название подразделения
                            keysOfTeams[teams] = Convert.ToInt32(kvp.Key);               // записать ключ подразделения
                            parentsOfTeams[teams] = Convert.ToInt32(kvp.Value.parent);   // записать родителя подразделения

                            teams++;  // число подразделений
                        }
                        ////////////////////////////////////////////////////////////////////////////////////////////////
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

                        // парсить полученный json с информацией о пользователях:
                        string uJson = usersGetResponse;
                        Dictionary<string, usersGetClass> usersGetArray = JsonConvert.DeserializeObject<Dictionary<string, usersGetClass>>(uJson);

                        // отсортировать пользователей по фамилии в алфавитном порядке 
                        usersGetArray = usersGetArray.OrderBy(pair => pair.Value.name1).ToDictionary(pair => pair.Key, pair => pair.Value);
                        ////////////////////////////////////////////////////////////////////////////////////////////////
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
                        Dictionary<string, jobsGetClass> jobsGetArray = JsonConvert.DeserializeObject<Dictionary<string, jobsGetClass>>(jJson);

                        // отсортировать в алфавитном порядке 
                        jobsGetArray = jobsGetArray.OrderBy(pair => pair.Value.name).ToDictionary(pair => pair.Key, pair => pair.Value);
                                               
                        string[] arrOfJobs = new string[500];
                        int[] arrOfKeyJobs = new int[500];
                        
                        // поэлементный перебор словаря:
                        int jobs = 0;                            // индекс элемента словаря

                        foreach (KeyValuePair<string, jobsGetClass> kvp in jobsGetArray)
                        //или кратко foreach (var kvp in array)
                        {
                            arrOfJobs[jobs] = kvp.Value.name;
                            arrOfKeyJobs[jobs] = Convert.ToInt32(kvp.Key);
                            jobs++;
                        }
                        
                        Array.Resize(ref arrOfJobs, jobs);  // изменить размерность массива arrOfNames1
                        Array.Resize(ref arrOfKeyJobs, jobs);   // изменить размерность массива arrOfTeams
                        
                        // поэлементный перебор словаря:
                        int index1 = 0;                            // индекс элемента словаря
                        
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
                            
                            string teamS = null;
                            string jobS = null;

                            for (int i=0; i< teams; i++)
                            {
                                if (keysOfTeams[i] == team)
                                    teamS = namesOfTeams[i];
                            }
                            
                            for (int i = 0; i < jobs; i++)
                            {
                                if (arrOfKeyJobs[i] == job)
                                    jobS = arrOfJobs[i];
                            }

                            string pathToPhoto= "http://api.nccp-eng.ru/photo/" + Key + ".jpg";

                            if (treeView1.SelectedNode.Text == name1 + " " + name2 + " " + name3)
                             {
                                 UserForm.showUserInfo(name1, name2, name3, teamS, jobS, phone1, phone2, dob, computer, login, email, pathToPhoto);
                             }

                            
                        }                         
                        UserForm.ShowDialog();           // открыть форму

                    }
                }
            }
            catch {}
        }
    }
}