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
        private WallpaperDetails Details { get; set; } = new WallpaperDetails();
        private UserConfig UserConfig { get; set; }

        public Controller() 
        {
            WorkerW = new GetWorker();
            Form1 = new Form1(Details);
            homePage = new HomePage(Form1);
            if (HasUserConfig())
            {
                Details = GetWallPaper();
                Form1.UpdateValues(Details.FilePath, Details.PlaybackSpeed);
                homePage.WindowState = FormWindowState.Minimized;
                homePage.Show();
                homePage.Hide();
                SetVideo();
            }
            else
            {
                homePage.WindowState = FormWindowState.Normal;
                homePage.Show();
            }
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
                homePage.Show();
                homePage.SendStateToWebView(Details);
            }
            if (homePage.WindowState == FormWindowState.Minimized)
            {
                homePage.Show();
                homePage.SendStateToWebView(Details);
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
        private WallpaperDetails GetWallPaper()
        {

            Random rnd = new Random();
            int index = rnd.Next(0, UserConfig.WallPaperDetails.Count);
            return UserConfig.WallPaperDetails[index];
        }

        private bool HasUserConfig()
        {
            UserConfig = configManager.LoadConfig();
            if (UserConfig.WallPaperDetails.Count != 0)
            {
                return true;
            }
            return false;
        }

        public void DisposeForms()
        {
            Form1?.Close();
            Form1?.Dispose();
            homePage?.Close();
            homePage?.Dispose();
        }
    }
}
