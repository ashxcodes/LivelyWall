using LivelyWall.Controller;
using System;
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
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Controller.Controller controller = Controller.Controller.Instance;
            Application.Run();
        }
        static void OnProcessExit(object sender, EventArgs e)
        {
            Controller.Controller.Instance.SetDefaultWallpaper();
        }
    }
}
