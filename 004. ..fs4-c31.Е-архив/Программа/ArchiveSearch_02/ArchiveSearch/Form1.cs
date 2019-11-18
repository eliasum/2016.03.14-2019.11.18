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
/*
При написании кода будем использовать .Net сборки взаимодействия с приложениями 
Microsoft Office. Напомним, что при использовании импортирования библиотеки типов
Com объекта и добавлении ссылки на него в Solutation Explorer (References, вкладка Com),
нам сразу становится доступно пространство имен объекта (в данном случае Excel).
При использовании .Net сборок взаимодействия с приложениями Microsoft Office нам,
после добавления ссылки на Microsoft.Office.Interop.Excel (References, вкладка .Net),
потребуется введение алиаса пространства имен Excel:
*/
using Excel = Microsoft.Office.Interop.Excel;

namespace ArchiveSearch
{
    public partial class Form1 : Form
    {
        // Список файлов и каталогов 
        static string myDir = @"\\srv-fs\Е-архив";
        static string dir1 = @"d:\\Advanced Installer";
/*
Объекты, которыми оперирует сервер Excel, несколько десятков. Мы будем рассматривать 
лишь основные, которые непосредственно требуются для обмена информацией приложения и 
сервера. Все объекты имеют иерархическую структуру. Сам сервер - объект Application
или приложение Excel, может содержать одну или более книг, ссылки на которые содержит
свойство Workbooks. Книги - объекты Workbook, могут содержать одну или более страниц, 
ссылки на которые содержит свойство Worksheets или (и) диаграмм - свойство Charts.
Страницы - Worksheet, содержать объекты ячейки или группы ячеек, ссылки на которые
становятся доступными через объект Range. Ниже в иерархии: строки, столбцы... Аналогично
и для объекта Chart серии линий, легенды...
*/
        private Excel.Application excelapp;         // приложение Excel
/*
Немного обособленно от этой иерархической структуры объектов находится свойство
Windows объекта Excel.Application, предназначенное для управления окнами сервера 
Excel. Свойство Windows содержит набор объектов Window, которые имеют, в свою очередь,
набор свойств и методов для управления размерами, видом, масштабом и упорядочиванием
открытых окон, отображением заголовков, цветами и т.д. Эти же возможности доступны и
для свойств и методов объекта Excel.Application - ActiveWindow (ссылка на активное окно).
*/
        private Excel.Window excelWindow;           // окно приложения

        // массив ссылок на созданные книги и на объект - конкретную книгу:
        private Excel.Workbooks excelappworkbooks;  // ссылки на книги приложения Excel
        private Excel.Workbook excelappworkbook;    // книга приложения Excel
/*
Вывод информации может быть выполнен на конкретный лист рабочей книги Excel в 
конкретную ячейку, поэтому нам необходимо определить объекты лист, в коллекции 
листов и ячейку. Обратим внимание на разницу задания всех и рабочих листов книги.
*/
        private Excel.Sheets excelsheets;
        private Excel.Worksheet excelworksheet;     // страница книги
/*
Аналогичного определения для ячеек и ячейки мы задать не можем, так как отдельно
данные объекты как самостоятельные в C# отсутствуют, а есть понятие области
выделенных ячеек, которая может включать одну или более ячеек, с которыми можно
выполнять действия. Поэтому для ячеек, с которыми выполняется действие, введем 
следующее определение.
Обратим внимание на то, что, интерфейс C# вместо понятия ячейки использует 
объекты Range (выбранная ячейка или группа ячеек).
*/
        private Excel.Range excelcells;             // ссылка на ячейки страницы

        private int lCount;

        public Form1()
        {
            InitializeComponent();
        }
/////////////////////////////////////////////////////////////////////////////////////
        private void b_saveToTxt_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            GetFiles(dir1);
            WriteToFile(@"tree.txt", listBox1);
            MessageBox.Show("Текстовый файл записан!");
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

                lCount = listBox1.Items.Count;
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

        private void button_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(((Button)(sender)).Tag);
            switch (i)
            {
                case 1:
                    excelapp = new Excel.Application();   // запуск Excel
                    excelapp.Visible = true;
                    // создание рабочей книги из 3-х листов:
                    excelapp.SheetsInNewWorkbook = 3;     // Свойство SheetsInNewWorkbook возвращает или устанавливает количество листов, автоматически помещаемых Excel в новые рабочие книги.
/*
В качестве параметра методу Add можно передать имя шаблона рабочей книги, 
однако, в этом случае мы привязываемся к пути, по которому инсталлированы 
приложения MicrosoftOffice. В примере использован другой способ: Type - 
класс декларации типов, Type.Missing - отсутствие значения. Некоторые методы
Excel принимают необязательные параметры, которые не поддерживаются в C#. 
Для решения этой проблемы в коде на C# требуется передавать поле Type.Missing
вместо каждого необязательного параметра, который является ссылочным типом 
(reference type). Кроме того, (этого нет в документации) при задании в методе ADD
чисел от 1 до 7 будет создана книга с одним листом (1, 6), диаграмма(2), макрос
(3, 4) и книга с четырьмя листами (5).
*/
                    excelapp.Workbooks.Add(Type.Missing);
                    //Получаем набор ссылок на объекты Workbook (на созданные книги)
                    excelappworkbooks = excelapp.Workbooks;
                    //Получаем ссылку на книгу 1 - нумерация от 1
                    excelappworkbook = excelappworkbooks[1];
                    //Ссылку можно получить и так, но тогда надо знать имена книг,
                    //причем, после сохранения - знать расширение файла
                    //excelappworkbook=excelappworkbooks["Книга 1"];
                    //Запроса на сохранение для книги не должно быть

                    //Получаем массив ссылок на листы выбранной книги
                    excelsheets = excelappworkbook.Worksheets;
                    excelappworkbook.Saved = true;
                    //Получаем ссылку на лист 1
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);
                    //Делаем третий лист активным
                    excelworksheet.Activate();

                    //excelcells = excelworksheet.get_Range("A1", "B1");
                    //excelcells = (Excel.Range)excelworksheet.Cells[1, 2];

                    string cell = null;
                    //Вывод в ячейки используя номер строки и столбца Cells[строка, столбец]
                    for (int m = 1; m <= lCount; m++)
                    {/*
                        excelcells = (Excel.Range)excelworksheet.Cells[m, 1];
          
                        //Выводим координаты ячеек
                        excelcells.Value2 = listBox1.Items[m - 1];
                        /*
                        excelcells.Font.Size = 20;
                        excelcells.Font.Italic = true;
                        excelcells.Font.Bold = true;
                        */

                        //cell = "A" + m;

                        try
                        {
                            Excel.Range rangeToHoldHyperlink = (Excel.Range)excelworksheet.Cells[m, 1];
                            //Excel.Range rangeToHoldHyperlink = excelworksheet.get_Range(cell, Type.Missing);
                            string hyperlinkTargetAddress = listBox1.Items[m - 1].ToString();

                            excelworksheet.Hyperlinks.Add(
                                    rangeToHoldHyperlink, hyperlinkTargetAddress,
                                    string.Empty, "Screen Tip Text", hyperlinkTargetAddress);
                        }
                        
                        catch (COMException ex)
                        {
                            MessageBox.Show(ex.ToString());
                            Log.Write(ex);
                        }
                    }

                    excelapp.Columns.AutoFit();  // выровнять ширину колонок по самой широкой записи
                    
                    break;
                case 2:
                    //Устанавливаем формат
                    excelapp.DefaultSaveFormat = Excel.XlFileFormat.xlWorkbookDefault;
                    
                    try
                    {
                        if (File.Exists(excelapp.DefaultFilePath))  // если файл по пути по умолчанию существует
                        {
                            //Будем спрашивать разрешение на запись поверх существующего документа
                            excelapp.DisplayAlerts = true;  // При значении свойства DisplayAlerts=true Excel будет спрашивать - записать ли сохраняемый документ поверх существующего, при значении false - нет.

                            //Сохраняем результат
                            excelappworkbooks = excelapp.Workbooks;
                            excelappworkbook = excelappworkbooks[1];
                            excelappworkbook.SaveAs(Type.Missing,  //object Filename
                               Type.Missing,                       //object FileFormat
                               Type.Missing,                       //object Password 
                               Type.Missing,                       //object WriteResPassword  
                               Type.Missing,                       //object ReadOnlyRecommended
                               Type.Missing,                       //object CreateBackup
                               Excel.XlSaveAsAccessMode.xlNoChange,//XlSaveAsAccessMode AccessMode
                               Type.Missing,                       //object ConflictResolution
                               Type.Missing,                       //object AddToMru 
                               Type.Missing,                       //object TextCodepage
                               Type.Missing,                       //object TextVisualLayout
                               Type.Missing);                      //object Local
                        }
                    }

                    catch (COMException ex)
                    {
                        MessageBox.Show(ex.ToString());
                        Log.Write(ex);
                    }
                    excelapp.Quit();
                    break;
                default:
                    Close();
                    break;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////
        void AutomateExcel()
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true;

            Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            workbook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            workbook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];
            Excel.Range rangeToHoldHyperlink = worksheet.get_Range("A1", Type.Missing);
            string hyperlinkTargetAddress = "Sheet2!A1";

            worksheet.Hyperlinks.Add(
                rangeToHoldHyperlink,
                string.Empty,
                hyperlinkTargetAddress,
                "Screen Tip Text",
                "Hyperlink Title");

            MessageBox.Show("Ready to clean up?");

            // Cleanup:
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.FinalReleaseComObject(excelcells);

            Marshal.FinalReleaseComObject(worksheet);

            workbook.Close(false, Type.Missing, Type.Missing);
            Marshal.FinalReleaseComObject(workbook);

            excelApp.Quit();
            Marshal.FinalReleaseComObject(excelApp);
        }

        void AutomateExcel2()
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(System.Reflection.Missing.Value);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Excel.Hyperlink link =
                (Excel.Hyperlink)
                xlWorkSheet.Hyperlinks.Add(xlWorkSheet.get_Range("L500", Type.Missing), "#Sheet1!B1", Type.Missing,
                                           "Go top",
                                           "UP");

            xlWorkSheet.Hyperlinks.Add(xlWorkSheet.get_Range("C5", Type.Missing), "www.google.com", Type.Missing, "Click me to go to Google ", "Google.com");
            xlApp.Visible = true;
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

        private void b_start_Click(object sender, EventArgs e)
        {
            button_Click(sender, e);
            MessageBox.Show("Файл Excel сгенерирован!");
        }

        private void b_stop_Click(object sender, EventArgs e)
        {
            button_Click(sender, e);
        }

        private void b_exit_Click(object sender, EventArgs e)
        {
            button_Click(sender, e);
        }

        private void b_saveToXlsx_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            GetFiles(dir1);
            button_Click(sender, e);
            MessageBox.Show("Файл Excel сгенерирован!");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AutomateExcel();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AutomateExcel2();
        }
    }
}
