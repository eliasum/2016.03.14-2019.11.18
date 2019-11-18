using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Далее ваши библиотеки
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace BirthdayMainModule
{
    public partial class MainForm : Form
    {
        string entIP = "192.168.214.96";

        public MainForm()
        {
            InitializeComponent();
        }

        public static HttpWebResponse PostMethod(string postedData, string postUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;

            UTF8Encoding encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes(postedData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;

            using (var newStream = request.GetRequestStream())
            {
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }
            return (HttpWebResponse)request.GetResponse();
        }

        public class RootObject
        {
            public string ip { get; set; }
            public int number { get; set; }
            public string login { get; set; }
            public string hash { get; set; }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            entIP = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // method=auth.start&ip=192.168.214.96
            // method=users.get&numbers=80983,80985,7468,81074&fields=name2

            string meth = "method=auth.start&ip=";
            string param = meth + entIP;

            string responseToString = null;

            // выполнить post-запрос:
            var response = PostMethod(param, "http://api.nccp-eng.ru/");
            if (response != null)
            {
                var strreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                responseToString = strreader.ReadToEnd();
            }

            // парсить полученный json:
            string json = responseToString;
            RootObject RObject = JsonConvert.DeserializeObject<RootObject>(json);
            /*
            richTextBox3.Text = RObject.ip + Environment.NewLine + RObject.number + Environment.NewLine
                               + RObject.login + Environment.NewLine + RObject.hash;
            */
            if (RObject.number != 0) MessageBox.Show("Здравствуйте, " + RObject.number + "!");
            else MessageBox.Show("Нет такого пользователя, введите корректный IP адрес.");
        }
    }
}
