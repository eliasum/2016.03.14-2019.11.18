using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PylonC.NET;

namespace Camcorder
{
    static class Camcorder
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            /* This is a special debug setting needed only for GigE cameras.
                See 'Building Applications with pylon' in the Programmer's Guide. */
            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "300000" /*ms*/);
#endif
            try
            {
                Pylon.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                Pylon.Terminate();
            }
            Pylon.Terminate();
        }
    }
}