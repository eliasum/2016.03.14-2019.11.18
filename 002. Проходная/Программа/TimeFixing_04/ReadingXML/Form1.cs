// CTRL-A, затем CTRL-K,CTRL-F
using System;
using System.Data;
using System.IO;
using System.Windows.Forms; 
using System.Xml;
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

        public static long summL = 0;

        private void b_OpenXML_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "xml files (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // очистить dataGridView1:
                            while (dataGridView1.Rows.Count > 1)
                                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                                    dataGridView1.Rows.Remove(dataGridView1.Rows[i]);

                            newDS.Tables.Clear();  // удалить DataTable с именем "Table" из DataSet

                            // открыть и загрузить файл в DataSet:
                            string filePath = openFileDialog1.FileName;
                            AuthorsDataSet.ReadXml(filePath);

                            dataGridView1.DataSource = AuthorsDataSet;  // Свойство DataGridView.DataSource возвращает или задает источник данных, данные которого отображает DataGridView.
                            dataGridView1.DataMember = "Table";         // Свойство DataGridView.DataMember получает или задает имя списка или таблицы в источнике данных, для которого объект DataGridView является отображением данных.

                            //this.dataGridView1.Columns["Dt_Device"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dataGridView1.Columns["Dt_Device"].Selected = true;

                            //////////Сортировка//////////
                            // Get a DataView of the table contained in the dataset.
                            DataTableCollection tables = AuthorsDataSet.Tables;  // Представляет коллекцию таблиц для DataSet
                            DataView view1 = new DataView(tables[0]);            // Представляет допускающее привязку данных, настраиваемое представление DataTable для сортировки, фильтрации, поиска, изменения и навигации. DataView не сохраняет данные, а представляет связанное представление соответствующего DataTable. Изменения данных DataView повлияют на DataTable. Изменения данных DataTable повлияют на все связанные с ним DataView.

                            // Create a BindingSource and set its DataSource property to
                            // the DataView.
                            BindingSource source1 = new BindingSource();  // Инкапсулирует источник данных для формы.
                            source1.DataSource = view1;

                            // Set the data source for the DataGridView.
                            dataGridView1.DataSource = source1;

                            source1.Sort = "Dt_Device";                // отсортировать по дате
                            //////////////////////////////

                            newDS.Tables.Add(view1.Table.Copy());      // отсортированные данные занести в newDS

                            int curRowCount = dataGridView1.RowCount;  // количество строк
                            long summ = 0;                             // суммарное количество секунд нахождения на предприятии
                            
                            DateTime[] Date = new DateTime[curRowCount];     // даты YMD HMS
                            DateTime[] DateYMD = new DateTime[curRowCount];  // даты YMD
                            DateTime[] DateD = new DateTime[curRowCount];    // даты дней без повторений
                            string[] ColorYMD = new string[curRowCount];
                            Color[] ColorD = new Color[curRowCount];
                            Color[] ColorN = new Color[curRowCount];

                            int[] nDay = new int[curRowCount];
                            string[] Reader = new string[curRowCount];
                            long[] Epoch = new long[curRowCount];
                            long[] summLocal = new long[curRowCount];

                            for (int i = 0; i < curRowCount-1; i++)
                            {
                                Reader[i] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value);         // вход/выход
                                string Dt_Device = Convert.ToString(dataGridView1.Rows[i].Cells[0].Value);  // дата типа строка

                                int Year = Convert.ToInt32(Dt_Device.Substring(0, 4));
                                int Month = Convert.ToInt32(Dt_Device.Substring(5, 2));
                                int Day = Convert.ToInt32(Dt_Device.Substring(8, 2));
                                int Hour = Convert.ToInt32(Dt_Device.Substring(11, 2));
                                int Minute = Convert.ToInt32(Dt_Device.Substring(14, 2));
                                int Second = Convert.ToInt32(Dt_Device.Substring(17, 2));

                                Date[i] = new DateTime(Year, Month, Day, Hour, Minute, Second);             // дата YMD HMS
                                DateYMD[i] = new DateTime(Year, Month, Day);                                // дата YMD

                                richTextBox1.Text += Convert.ToString(Date[i]) + Environment.NewLine;

                                Epoch[i] = (Date[i].ToUniversalTime().Ticks - 621355968000000000) / 10000000;  // перевод даты в юникстайм

                                richTextBox2.Text += Convert.ToString(Epoch[i]) + Environment.NewLine;

                                //richTextBox3.Text += Date[i] + Environment.NewLine;
                            }

                            // получить даты дней без повторений:
                            DateD[0] = DateYMD[0];
                            int j = 1;
                            //richTextBox3.Text += Convert.ToString(DateD[j]) + Environment.NewLine;

                            for (int i = 1; i < curRowCount; i++)
                            {
                                if (DateYMD[i] != DateYMD[i-1])
                                {
                                    DateD[j] = DateYMD[i];

                                    //richTextBox3.Text += Convert.ToString(DateD[j]) + Environment.NewLine;

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

                                    //richTextBox3.Text += summLocal[k] + Environment.NewLine;

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

                                //richTextBox3.Text += ColorD[l] + Environment.NewLine;
                            }

                            /*
                            // получить цвета для каждой даты:
                            int m = 0;

                            for (int i = 0; i < curRowCount-1; i++) 
                            {
                                if (DateYMD[i] == DateD[m])
                                {
                                    ColorYMD[i] = Date[i] + " " + ColorD[m];

                                    richTextBox3.Text += ColorYMD[i] + Environment.NewLine;
                                }
                                else
                                if  (DateYMD[i] != DateD[m])
                                {
                                    i--;

                                    m++;
                                }
                            }               
                            */


                            richTextBox3.Clear();

                            // получить цвета для каждой даты:
                            int m = 0;

                            for (int i = 0; i < curRowCount - 1; i++)
                            {
                                if (DateYMD[i] == DateD[m])
                                {
                                    ColorN[i] = ColorD[m];

                                    //richTextBox3.Text += ColorYMD[i] + Environment.NewLine;

                                    richTextBox3.AppendText(Convert.ToString(Date[i]) + "\n", ColorN[i]);
                                }
                                else
                                if (DateYMD[i] != DateD[m])
                                {
                                    i--;

                                    m++;
                                }
                            }



                                    // вывод часов и минут:
                                    var ts = TimeSpan.FromSeconds(summ);
                                    tb_hours.Text = Convert.ToString(Convert.ToInt32(ts.Hours) + Convert.ToInt32(ts.Days) * 24);
                                    tb_minutes.Text = Convert.ToString(ts.Minutes);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
