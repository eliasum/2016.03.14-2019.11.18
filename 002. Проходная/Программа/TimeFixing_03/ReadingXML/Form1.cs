﻿// CTRL-A, затем CTRL-K,CTRL-F
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
                            int in1 = 0;                               // количество входов
                            int out1 = 0;                              // количество выходов

                            int err = 0;                               // переменная ошибки
                            int count = 0;                             // счетчик

                            string s=null;

                            for (int i = 0; i < curRowCount - 1; i++)
                            {
                                string Reader = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value);     // вход/выход
                                string Dt_Device = Convert.ToString(dataGridView1.Rows[i].Cells[0].Value);  // дата

                                int Year = Convert.ToInt32(Dt_Device.Substring(0, 4));
                                int Month = Convert.ToInt32(Dt_Device.Substring(5, 2));
                                int Day = Convert.ToInt32(Dt_Device.Substring(8, 2));
                                int Hour = Convert.ToInt32(Dt_Device.Substring(11, 2));
                                int Minute = Convert.ToInt32(Dt_Device.Substring(14, 2));
                                int Second = Convert.ToInt32(Dt_Device.Substring(17, 2));

                                DateTime Date = new DateTime(Year, Month, Day, Hour, Minute, Second);

                                richTextBox1.Text += Convert.ToString(Date) + Environment.NewLine;

                                long Epoch = (Date.ToUniversalTime().Ticks - 621355968000000000) / 10000000;  // перевод даты в юникстайм

                                richTextBox2.Text += Convert.ToString(Epoch) + Environment.NewLine;

                                // расчет summ:
                                if (Reader == "Выход")
                                {
                                    summ += Epoch; out1++;
                                    s = Convert.ToString(summ) + " " + Convert.ToString(Date);

                                    string ss = "!!!";

                                    richTextBox3.Text += s + Environment.NewLine;

                                    err--;
                                    count++;
                                }
                                else if (Reader == "Вход")
                                {
                                    summ -= Epoch; in1++;
                                    err++;
                                    count++;
                                }
                                
                                if(count%2==0)
                                {
                                    if (err != 0)
                                    {
                                        richTextBox3.Text += "!!!";
                                    }
                                }
                            }

                            // вывод входов, выходов:
                            tB_in.Text = Convert.ToString(in1);
                            tB_out.Text = Convert.ToString(out1);

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

        private void b_saveOrderedXml_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.InnerXml = newDS.GetXml();  // загрузить в документ данные newDS

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "*.xml";

            DialogResult savRes = saveFileDialog1.ShowDialog();
            if (savRes == DialogResult.OK)
                doc.Save(saveFileDialog1.FileName);
        }
    }
}
