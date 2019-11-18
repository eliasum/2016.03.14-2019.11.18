﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Birthday
{
    public partial class RemindForm : Form
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

        public void ShowRemind(string Str)
        {
            rTB_dates.Text = Str;
            Rectangle screenSize = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;               // рабочая область экрана
            this.Height = rTB_dates.Lines.Count()*20;  // высота rTB_dates изменяется относительно количества линий rTB_dates
            this.Location = new Point(screenSize.Width - this.Width, screenSize.Height - this.Height);  // новое положение формы
        }

        public void CloseRemind()
        {
            Close();
        }

        private void rTB_dates_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rTB_dates_DoubleClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
