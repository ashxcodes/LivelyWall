using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace LivelyWall
{
    public class WallpaperDetails
    {
        public string FilePath { get; set; } = string.Empty;
        public double PlaybackSpeed { get; set; } = 1.0;
    }

    public class UserConfig
    {
        private static UserConfig instance;
        public List<WallpaperDetails> WallPaperDetails { get; set; }

        private UserConfig()
        {
            WallPaperDetails = new List<WallpaperDetails>();
        }

        public static UserConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserConfig();
                }
                return instance;
            }
        }
    }

    public class ConfigManager
    {
        private readonly string filePath;

        public ConfigManager()
        {
            string directoryPath = Path.Combine(Application.StartupPath, "config");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            filePath = Path.Combine(directoryPath, "user_config.json");
        }

        public void SaveConfig(WallpaperDetails details)
        {
            UserConfig config = LoadConfig();

            if (!config.WallPaperDetails.Exists(w => w.FilePath == details.FilePath))
            {
                config.WallPaperDetails.Add(details);
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        public UserConfig LoadConfig()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<UserConfig>(json);
            }

            return UserConfig.Instance;
        }
    }
}
