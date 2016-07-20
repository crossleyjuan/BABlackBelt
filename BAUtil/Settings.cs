using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BABlackBelt
{
    public class Settings
    {
        private static Dictionary<string, string> _values;
        private static Settings _instance;
        private string _file;

        public static Settings getSettings()
        {
            if (_instance == null)
            {
                string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                string folder = Path.Combine(userFolder, ".bb");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string settingsFile = Path.Combine(folder, "Settings.config");

                _instance = new Settings(settingsFile);
                _instance.Initialize();
            }
            return _instance;
        }

        public static Settings getProjectSettings(string path)
        {
            string fileName = Path.Combine(path, "project.config");
            Settings settings = new Settings(fileName);

            settings.Initialize();

            return settings;
        }

        public string this[string key]
        {
            get
            {
                if (_values.ContainsKey(key))
                {
                    return _values[key];
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (_values.ContainsKey(key))
                {
                    _values.Remove(key);
                }
                _values.Add(key, value);
            }
        }

        public Settings(string file)
        {
            _file = file;
        }

        public void Initialize()
        {
            _values = new Dictionary<string, string>();
            LoadSettings(_file);
        }

        public Dictionary<string, string> ReadSettingsFile(string fileName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamReader reader = new StreamReader(fs);
            string line;

            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                if (!line.Trim().StartsWith("#"))
                {
                    string key = line.Substring(0, line.IndexOf(':'));
                    string value = line.Substring(line.IndexOf(':') + 1).Trim();
                    if (value.EndsWith(";"))
                    {
                        value = value.Substring(0, value.LastIndexOf(";"));
                    }

                    result[key] = value;
                }
            }
            reader.Close();
            fs.Close();

            return result;
        }

        public void LoadSettings(string settingsFile)
        {
            Dictionary<string, string> globalSettings = ReadSettingsFile(settingsFile);
            foreach (KeyValuePair<string, string> pair in globalSettings)
            {
                this[pair.Key] = pair.Value;
            }

        }

        public void saveSettings()
        {
            FileStream fs = new FileStream(_file, FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            foreach (KeyValuePair<string, string> element in _values)
            {
                string line = string.Format("{0}: {1};", element.Key, element.Value);

                writer.WriteLine(line);
            }

            writer.Close();
            fs.Close();
        }
    }
}
