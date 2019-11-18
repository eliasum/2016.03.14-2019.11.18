using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Birthday
{
    public partial class RemindForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public RemindForm()
        {
            InitializeComponent();
        }

        private void RemindForm_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RemindForm_DoubleClick(object sender, EventArgs e)
        {
            Close();
        }

        public void ShowRemind(string[] Str, Color[] Clr, int integer)
        {
            for (int i = 0; i < integer; i++)
            {
                rTB_dates.AppendText(Str[i], Clr[i]);                 // вывести сообщение
            }

            Rectangle screenSize = Screen.PrimaryScreen.WorkingArea;  // рабочая область экрана

            // высота rTB_dates изменяется относительно количества линий rTB_dates
            this.Height = rTB_dates.Lines.Count() * 20;

            // новое положение формы
            this.Location = new Point(screenSize.Width - this.Width, screenSize.Height - this.Height);  
        }

        private void rTB_dates_DoubleClick(object sender, EventArgs e)
        {
            Close();
        }

        private void RemindForm_Load(object sender, EventArgs e)
        {
            Activate();                                               // активировать форму
        }

        private void t_RemindForm_Tick(object sender, EventArgs e)
        {
            t_RemindForm.Enabled = false;
            Close();
        }

        private void RemindForm_Deactivate(object sender, EventArgs e)
        {
            t_RemindForm.Enabled = true;
        }

        private void RemindForm_Activated(object sender, EventArgs e)
        {
            t_RemindForm.Enabled = false;
        }

        private void rTB_dates_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
