using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchiveSearch
{
    public partial class Form1 : Form
    {
        // Список файлов и каталогов 
        static string dir = @"\\fs4-c31\Е-архив";
        static string dir1 = @"d:\\Advanced Installer";
        static string dir2 = @"\\fs4-c31\Е-архив\СТ - 120";
        string[] dirsAndFiles = Directory.GetFileSystemEntries(dir);
        string[] allFoundFiles = Directory.GetFiles(dir1, "*", SearchOption.AllDirectories);
        IEnumerable<string> files = Directory.EnumerateFiles(dir1, "*", SearchOption.AllDirectories);

        public Form1()
        {
            InitializeComponent();
        }
/////////////////////////////////////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
                        
            for (int i = 0; i < Convert.ToInt16(dirsAndFiles.Length); i++)
            {
                richTextBox1.Text += dirsAndFiles[i] + Environment.NewLine;
            }
        }
/////////////////////////////////////////////////////////////////////////////////////
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            GetFiles(dir);
            WriteToFile(@"tree.txt", listBox1);
        }

        public void GetFiles(string dir)
        {
            try
            {
                System.IO.DirectoryInfo DirectoryInfo = new System.IO.DirectoryInfo(dir);
                foreach (System.IO.FileInfo FileInfo in DirectoryInfo.GetFiles())
                {
                    //Здесь мы выводим информацию о полученном файле
                    listBox1.Items.Add(FileInfo.FullName);
                }
                foreach (System.IO.DirectoryInfo Dir in DirectoryInfo.GetDirectories())
                {
                    try { GetFiles(Dir.FullName); }
                    catch { }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.ToString());
                Log.Write(ex);
            }
        }

        private void WriteToFile(string path, ListBox listBox)
        {
            using (var sw = new StreamWriter(new FileStream(path, FileMode.Create)))
            {
                if (listBox != null)
                {
                    foreach (var item in listBox.Items) // в таком же порядке
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////
        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (string file in allFoundFiles)
            {
                listBox1.Items.Add(file);
            }
        }
/////////////////////////////////////////////////////////////////////////////////////
        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            try
            {
                var dirs = from dir in Directory.EnumerateFiles(dir1, "*", SearchOption.AllDirectories)
                           select dir;

                foreach (var dir in dirs)
                {
                    listBox1.Items.Add(dir.Substring(dir.LastIndexOf("\\") + 1));
                }

            }
            catch (UnauthorizedAccessException ex) { MessageBox.Show(ex.ToString()); }
        }
/////////////////////////////////////////////////////////////////////////////////////
        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (string file in files)
            {
                listBox1.Items.Add(file);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////
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
