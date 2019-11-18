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
        string NewL = Environment.NewLine;

        public MainForm()
        {
            InitializeComponent();
        }

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
        /// Класс преобразования json для получения данных auth.start.
        /// </summary>
        public class authStart
        {
            public string ip { get; set; }
            public int number { get; set; }
            public string login { get; set; }
            public string hash { get; set; }
        }

        /// <summary>
        /// Класс преобразования json для получения данных users.get.
        /// </summary>
        public class usersGet
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
        /// Словарь: класс Dictionary<TKey, TValue>.
        /// </summary>
        Dictionary<string, usersGet> array = new Dictionary<string, usersGet>();

        private void MainForm_Load(object sender, EventArgs e)
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

            richTextBox1.Text = usersGetResponse;

            // парсить полученный json:
            string uJson = usersGetResponse;
            Dictionary<string, usersGet> usersGetArray = JsonConvert.DeserializeObject<Dictionary<string, usersGet>>(uJson);

            // вывод в richTextBox:
            richTextBox2.Clear();

            foreach (KeyValuePair<string, usersGet> kvp in usersGetArray)
            //или кратко foreach (var kvp in array)
            {
                richTextBox2.Text += Convert.ToString(kvp.Key) + NewL +
                                     Convert.ToString(kvp.Value.name1) + NewL + Convert.ToString(kvp.Value.name2) + NewL +
                                     Convert.ToString(kvp.Value.name3) + NewL + Convert.ToString(kvp.Value.email) + NewL +
                                     Convert.ToString(kvp.Value.computer) + NewL + Convert.ToString(kvp.Value.login) + NewL +
                                     Convert.ToString(kvp.Value.dob) + NewL + NewL;
            }

            // Получение имени компьютера.
            string host = Dns.GetHostName();

            // Получение ip-адреса.
            IPAddress entIP = Dns.GetHostByName(host).AddressList[0];

            //Авторизовать сотрудника по IP адресу:
            string authMeth = "method=auth.start&ip=";
            string param = authMeth + entIP;
            string authResponse = null;

            // выполнить post-запрос:
            var aResponse = PostMethod(param, "http://api.nccp-eng.ru/");
            if (aResponse != null)
            {
                var strreader = new StreamReader(aResponse.GetResponseStream(), Encoding.UTF8);
                authResponse = strreader.ReadToEnd();
            }

            // парсить полученный json:
            string aJson = authResponse;
            authStart authStartResponse = JsonConvert.DeserializeObject<authStart>(aJson);
            /*
            richTextBox3.Text = authStartResponse.ip + NewL + authStartResponse.number + NewL
                               + authStartResponse.login + NewL + authStartResponse.hash;
            */
            // проверка наличия в системе:
            int index = authStartResponse.number;  // табельный номер
            string s = Convert.ToString(index);
            string userName = usersGetArray[s].name2 + " " + usersGetArray[s].name1;

            if (authStartResponse.number != 0)
            {
                l_UserName.Text = "Здравствуйте, " + userName + "!";
            }
            else l_UserName.Text = "Здравствуйте, Гость!";
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {

        }
    }
}
