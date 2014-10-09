using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BABlackBelt
{
    public class UserWorkspace
    {
        private Dictionary<string, string> _values = new Dictionary<string, string>();

        public UserWorkspace()
        {
        }

        public void Initialize()
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string folder = Path.Combine(userFolder, ".bb");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string workspaceFile = Path.Combine(folder, "workspace.bb");

            LoadFile(workspaceFile);
        }

        public void SaveWorkspace()
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string folder = Path.Combine(userFolder, ".bb");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string workspaceFile = Path.Combine(folder, "workspace.bb");

            SaveFile(workspaceFile);
        }

        private void LoadFile(string file)
        {
            byte[] content = FileUtil.LoadFile(file);

            string text = System.Text.Encoding.ASCII.GetString(content);

            text = text.Replace("\\r", "");
            text = text.Replace("\\n", "");

            string[] elements = text.Split(';');

            foreach (string element in elements)
            {
                if (element.IndexOf(":") > 0)
                {
                    string key = element.Substring(0, element.IndexOf(":")).Trim();
                    string value = element.Substring(element.IndexOf(":") + 1).Trim();
                    _values.Add(key, value);
                }
            }
        }

        private void SaveFile(string file)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvalue in _values)
            {
                sb.AppendFormat("{0}: {1};", kvalue.Key, kvalue.Value);
            }

            FileUtil.SaveFile(file, ASCIIEncoding.ASCII.GetBytes(sb.ToString()));
        }

        public string RecentProject
        {
            get
            {
                if (_values.ContainsKey("recent project"))
                {
                    return _values["recent project"];
                }
                return null;
            }
            set
            {
                if (_values.ContainsKey("recent project"))
                {
                    _values.Remove("recent project");
                }
                _values.Add("recent project", value);
            }
        }
    }
}
