using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Birthday.Properties;

namespace Birthday
{
    public partial class SkinsForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public SkinsForm()
        {
            InitializeComponent();
            SkinHelper.InitSkinGallery(galleryControl1);
            UserLookAndFeel.Default.SkinName = Settings.Default["ApplicationSkinName"].ToString();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            // Запомнить скин:
            Settings.Default["ApplicationSkinName"] = UserLookAndFeel.Default.SkinName;
            Settings.Default.Save();
     
            Close();
        }
    }
}
