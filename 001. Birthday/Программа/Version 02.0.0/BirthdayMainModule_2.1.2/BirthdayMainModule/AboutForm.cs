using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Birthday
{
    public partial class AboutForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void sB_OK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
