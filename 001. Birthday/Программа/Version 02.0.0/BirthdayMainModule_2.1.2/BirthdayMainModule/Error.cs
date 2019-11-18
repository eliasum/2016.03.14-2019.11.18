using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Birthday
{
    public partial class F_Error : DevExpress.XtraEditors.XtraForm
    {
        public F_Error()
        {
            InitializeComponent();
        }

        private void sB_OK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}