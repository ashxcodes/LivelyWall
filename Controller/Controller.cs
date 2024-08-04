using System;
using System.IO;
using System.Windows.Forms;
using LivelyWall.GetWorkerW;

namespace LivelyWall.Controller
{
    public class Controller
    {
        private static Controller instance;
        private static readonly object padlock = new object();

        public static Controller Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Controller();
                    }
                    return instance;
                }
            }
        }

        private readonly ConfigManager configManager = new ConfigManager();
        private Form1 Form1{ get; set; }
        private HomePage homePage {  get; set; }
        private GetWorker WorkerW { get; set; }

        public Controller() 
        {
            WorkerW = new GetWorker();
            Form1 = new Form1("", 1);
            LoadUserConfig();
        }
        public void SetVideo()
        {
            WorkerW.SetVideo(Form1.Handle);
            Form1?.StartVideo();
            Form1?.Show();
        }
        public void ToggleHomePage()
        {
            if (homePage == null)
            {
                homePage = new HomePage(Form1)
                {
                    WindowState = FormWindowState.Minimized
                };
            }
            if (homePage.WindowState == FormWindowState.Minimized)
            {
                homePage.Show();
                homePage.WindowState = FormWindowState.Normal;
            }
            else
            {
                homePage.WindowState = FormWindowState.Minimized;
                homePage.Hide();
            }
        }
        public void SetDefaultWallpaper()
        {
            string defaultWallpaper = Path.Combine(Application.StartupPath,"FrontEnd" , "DefaultWallpaper.jpg");
            WorkerW.SetDefaultWallpaper(defaultWallpaper);
        }
        private void LoadUserConfig()
        {
            UserConfig config = configManager.LoadConfig();
            if (config.Paths.Count != 0)
            {
                Random rnd = new Random();
                int index = rnd.Next(0, config.Paths.Count);
                string filepath = config.Paths[index];
                Form1.UpdateValues(filepath, 1);
                SetVideo();
            } else
            {
                homePage = new HomePage(Form1);
            }

        }
    }
}
