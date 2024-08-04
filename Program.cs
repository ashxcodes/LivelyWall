using LivelyWall.Controller;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LivelyWall
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetProcessDPIAware();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Controller.Controller controller = Controller.Controller.Instance;
            Application.Run();
        }
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        static void OnProcessExit(object sender, EventArgs e)
        {
            Controller.Controller.Instance.SetDefaultWallpaper();
        }
    }
}
