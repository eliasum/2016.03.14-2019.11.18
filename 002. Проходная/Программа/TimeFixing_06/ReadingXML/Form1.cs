// CTRL-A, затем CTRL-K,CTRL-F
using System;
using System.Data;
using System.IO;
using System.Windows.Forms; 
using System.Drawing;

namespace TimeFixing
{
    public partial class MainForm : Form
    {
        DataSet newDS = new DataSet(); // новый Dataset для отсортированных данных (представляет кэш данных в памяти)

        public MainForm()
        {
            InitializeComponent();
        }

        public static long summL = 0;  // переменная хранения суммы юникстаймов для каждого дня

        private void b_OpenXML_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "xml files (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        rTB_Errors.Clear();                   // очистить список ошибок

                        tb_hours.BackColor = Color.White;
                        tb_minutes.BackColor = Color.White;

                        tb_hours.Text = "";
                        tb_minutes.Text = "";

                        using (myStream)
                        {
                            
                            // очистить облать вывода файла XML dGV_XML:
                            while (dGV_XML.Rows.Count > 1)
                                for (int i = 0; i < dGV_XML.Rows.Count - 1; i++)
                                    dGV_XML.Rows.Remove(dGV_XML.Rows[i]);  // удалить все строки из таблицы dGV_XML

                            newDS.Tables.Clear();                          // удалить DataTable с именем "Table" из DataSet
                            dGV_XML.DataSource = null;                     // удалить источник данных для dGV_XML

                            // открыть и загрузить файл в DataSet:
                            string filePath = openFileDialog1.FileName;
                            AuthorsDataSet.ReadXml(filePath);

                            dGV_XML.DataSource = AuthorsDataSet;           // Свойство DataGridView.DataSource возвращает или задает источник данных, данные которого отображает DataGridView.
                            dGV_XML.DataMember = "Table";                  // Свойство DataGridView.DataMember получает или задает имя списка или таблицы в источнике данных, для которого объект DataGridView является отображением данных.

                            dGV_XML.Columns["Dt_Device"].Selected = true;

                            //////////Сортировка//////////
                            // Get a DataView of the table contained in the dataset.
                            DataTableCollection tables = AuthorsDataSet.Tables;  // Представляет коллекцию таблиц для DataSet
                            DataView view1 = new DataView(tables[0]);            // Представляет допускающее привязку данных, настраиваемое представление DataTable для сортировки, фильтрации, поиска, изменения и навигации. DataView не сохраняет данные, а представляет связанное представление соответствующего DataTable. Изменения данных DataView повлияют на DataTable. Изменения данных DataTable повлияют на все связанные с ним DataView.

                            // Create a BindingSource and set its DataSource property to
                            // the DataView.
                            BindingSource source1 = new BindingSource();         // Инкапсулирует источник данных для формы.
                            source1.DataSource = view1;

                            // Set the data source for the DataGridView.
                            dGV_XML.DataSource = source1;

                            source1.Sort = "Dt_Device";                      // отсортировать по дате
                            //////////////////////////////

                            newDS.Tables.Add(view1.Table.Copy());            // отсортированные данные занести в newDS

                            int curRowCount = dGV_XML.RowCount;              // количество строк
                            long summ = 0;                                   // суммарное количество секунд нахождения на предприятии
                            
                            DateTime[] Date = new DateTime[curRowCount];     // даты YMD HMS
                            DateTime[] DateYMD = new DateTime[curRowCount];  // даты YMD
                            DateTime[] DateD = new DateTime[curRowCount];    // даты дней без повторений
                            string[] DateS = new string[curRowCount];        // даты дней без повторений
                            Color[] ColorD = new Color[curRowCount];         // цвета дней без повторений

                            string[] Reader = new string[curRowCount];
                            long[] Epoch = new long[curRowCount];
                            long[] summLocal = new long[curRowCount];

                            for (int i = 0; i < curRowCount-1; i++)
                            {
                                Reader[i] = Convert.ToString(dGV_XML.Rows[i].Cells[1].Value);         // вход/выход
                                string Dt_Device = Convert.ToString(dGV_XML.Rows[i].Cells[0].Value);  // дата типа строка

                                int Year = Convert.ToInt32(Dt_Device.Substring(0, 4));
                                int Month = Convert.ToInt32(Dt_Device.Substring(5, 2));
                                int Day = Convert.ToInt32(Dt_Device.Substring(8, 2));
                                int Hour = Convert.ToInt32(Dt_Device.Substring(11, 2));
                                int Minute = Convert.ToInt32(Dt_Device.Substring(14, 2));
                                int Second = Convert.ToInt32(Dt_Device.Substring(17, 2));

                                Date[i] = new DateTime(Year, Month, Day, Hour, Minute, Second);                // дата YMD HMS
                                DateYMD[i] = new DateTime(Year, Month, Day);                                   // дата YMD

                                Epoch[i] = (Date[i].ToUniversalTime().Ticks - 621355968000000000) / 10000000;  // перевод даты в юникстайм
                            }

                            // получить даты дней без повторений:
                            DateD[0] = DateYMD[0];
                            int j = 1;

                            for (int i = 1; i < curRowCount; i++)
                            {
                                if (DateYMD[i] != DateYMD[i-1])
                                {
                                    DateD[j] = DateYMD[i];

                                    j++;
                                }
                            }
                            
                            // изменить размерность массивов: summLocal
                            Array.Resize(ref DateD, j);
                            Array.Resize(ref ColorD, j);
                            Array.Resize(ref summLocal, j);

                            // получить суммарные юникстаймы для каждого дня:
                            int k = 0;
                            
                            for (int i = 0; i < curRowCount; i++)
                            {
                                if (DateD[k] == DateYMD[i])
                                {
                                    if (Reader[i] == "Выход")
                                    {
                                        summL += Epoch[i];
                                        summ += Epoch[i];
                                    }
                                    else if (Reader[i] == "Вход")
                                    {
                                        summL -= Epoch[i];
                                        summ -= Epoch[i];
                                    }

                                    summLocal[k] = summL;
                                }
                                else
                                if (DateD[k] != DateYMD[i])
                                {
                                    summL = 0;

                                    i--;

                                    k++;
                                }
                            }

                            // получить цвета для каждого дня:
                            for (int l = 0; l < j-1; l++)
                            {
                                if ((summLocal[l] > 86400) || (summLocal[l] < 0))  
                                {
                                    ColorD[l] = Color.Red; 
                                }
                                else
                                if ((summLocal[l] <= 86400) || (summLocal[l] >= 0))
                                {
                                    ColorD[l] = Color.Black;
                                }
                            }
                         
                            // получить дни ошибок:
                            for (int l = 0; l < j - 1; l++)
                            {
                                DateS[l] = DateD[l].ToString("dd MMMM yyyy");
                            }
                            
                            rTB_Errors.Clear();
                            bool flag = true;

                            for (int l = 0; l < j - 1; l++)
                            {
                                if (ColorD[l] == Color.Red)
                                {
                                    rTB_Errors.AppendText(DateS[l] + "\n", ColorD[l]);
                                    flag = false;
                                }
                            }

                            if (flag)
                            {
                                rTB_Errors.AppendText("Ошибок нет!");

                                // вывод часов и минут:
                                var ts = TimeSpan.FromSeconds(summ);
                                tb_hours.Text = Convert.ToString(Convert.ToInt32(ts.Hours) + Convert.ToInt32(ts.Days) * 24);
                                tb_minutes.Text = Convert.ToString(ts.Minutes);

                                tb_hours.BackColor = Color.White;
                                tb_minutes.BackColor = Color.White;
                            }
                            else
                            if (!flag)
                            {
                                tb_hours.Text = "";
                                tb_minutes.Text = "";

                                tb_hours.BackColor = Color.Red;
                                tb_minutes.BackColor = Color.Red;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AboutForm AboutForm = new AboutForm();   // создать форму
            AboutForm.ShowDialog();                  // открыть форму 
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
