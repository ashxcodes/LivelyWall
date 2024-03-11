using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace LivelyWall
{
    public class UserConfig
    {
        private static UserConfig instance;
        public List<string> Paths { get; set; }

        private UserConfig()
        {
            Paths = new List<string>();
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

            // Ensure that the "config" folder exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            filePath = Path.Combine(directoryPath, "user_config.json");
        }

        public void SaveConfig(string newPath)
        {
            UserConfig config = LoadConfig();

            if (!config.Paths.Contains(newPath))
            {
                config.Paths.Add(newPath);
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
