using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace NoteTree.Models
{
    public class ConfigModel
    {
        [JsonProperty("rootFolderPath")]
        public string RootFolderPath { get; set; }

        public ConfigModel()
        {
            RootFolderPath = null;
        }

        public static void SaveConfig(ConfigModel config)
        {
            try
            {
                string jsonConfig = JsonConvert.SerializeObject(config);
                using (StreamWriter writer = new StreamWriter("config.json"))
                {
                    writer.Write(jsonConfig);
                    writer.Close();
                }
            }
            catch (Exception exc)
            {
                Debug.Print($"Ошибка при сохранении: {exc?.Message}");
            }
        }


        public static ConfigModel LoadConfig()
        {
            try
            {
                if (File.Exists("config.json"))
                {
                    string data = "";

                    FileInfo info = new FileInfo("config.json");
                    using (StreamReader reader = new StreamReader("config.json"))
                    {
                        data = reader.ReadToEnd();
                    }

                    var setting = new JsonSerializerSettings()
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    ConfigModel config = JsonConvert.DeserializeObject<ConfigModel>(data, setting);
                    return config;
                }
            }
            catch (Exception exc)
            {
                Debug.Print($"Ошибка при загрузке: {exc.Message}");
            }
            return null;
        }
    }
}
