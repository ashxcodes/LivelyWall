using System;

namespace LivelyWall.Controller
{
    public class Controller
    {
        private readonly ConfigManager configManager = new ConfigManager();
        private Form1 Form1 { get; set; }
        private HomePage homePage {  get; set; }
        private string filepath { get; set; }
        private readonly double playback = 1;

        public Controller() 
        {
            LoadUserConfig();
        }
        private void LoadUserConfig()
        {
            UserConfig config = configManager.LoadConfig();
            if (config.Paths.Count != 0)
            {
                Random rnd = new Random();
                int index = rnd.Next(0, config.Paths.Count);
                filepath = config.Paths[index];
                Form1 = new Form1(filepath, playback);
                Form1.Show();
            } else
            {
                homePage = new HomePage();
            }

        }
    }
}
