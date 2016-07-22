using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ServerLib;
using System.Windows.Forms;

namespace BABlackBelt
{
    public class UserWorkspace
    {
        private Dictionary<string, string> _values = new Dictionary<string, string>();

        private static UserWorkspace _instance;
        private ChatClient _client;

        public delegate void MessageReceivedHandler(ChatClient client, ServerLib.ChatClient.Message message);
        public delegate void ClientCloseHandler(object client);
        public delegate void ClientConnectedHandler(object client);

        public event MessageReceivedHandler ReceiveHandler;
        public event ClientCloseHandler CloseHandler;
        public event ClientConnectedHandler ConnectHandler;

        private UserWorkspace()
        {
        }

        public ChatClient Client
        {
            get { return _client; }
        }

        public static UserWorkspace Workspace()
        {
            if (_instance == null)
            {
                _instance = new UserWorkspace();
                _instance.Initialize();
            }
            return _instance;
        }

        private void Initialize()
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

            Settings globalSettings = Settings.getSettings();
            globalSettings["LastDir"] = RecentProject;
            globalSettings.saveSettings();
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

        public string ChatUser
        {
            get
            {
                if (_values.ContainsKey("chatuser"))
                {
                    return _values["chatuser"];
                }
                return null;
            }
            set
            {
                if (_values.ContainsKey("chatuser"))
                {
                    _values.Remove("chatuser");
                }
                _values.Add("chatuser", value);
            }
        }


        public bool LoadProject(string projectPath)
        {
            Settings projectSettings = Settings.getProjectSettings(projectPath);

            Settings settings = Settings.getSettings();
            if (string.IsNullOrEmpty(settings["version"]) || (settings["version"] != "1.0"))
            {
                SettingsForm frm = new SettingsForm(projectPath);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Settings incompleted, load project aborted");
                    return false;
                }

            }
            RecentProject = projectPath;
            SaveWorkspace();
            return true;
        }

        public void Client_Connected(ChatClient client)
        {
            if (string.IsNullOrEmpty(UserWorkspace.Workspace().ChatUser))
            {
                UserWorkspace.Workspace().ChatUser = "NoName";
            }
            client.ChangeNick(UserWorkspace.Workspace().ChatUser);
            if (ConnectHandler != null) ConnectHandler(client);
        }

        public bool ConnectBlackBelt()
        {
            Settings settings = Settings.getSettings();
            if (string.IsNullOrEmpty(settings["blackbeltserver"]))
            {
                SettingsForm frm = new SettingsForm();
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Settings incompleted, load project aborted");
                    return false;
                }
                // Reload
                settings = Settings.getSettings();
            }
            try
            {
                _client = new ChatClient(settings["blackbeltserver"], 9000);
                _client.ConnectHandler += Client_Connected;
                _client.CloseHandler += Client_CloseHandler;
                _client.ReceiveHandler += Client_ReceiveMessage;
                _client.Connect();

            }
            catch (Exception e)
            {
                MessageBox.Show("Error connecting to blackbelt server");
                return false;
            }
            return true;
        }

        private void Client_ReceiveMessage(ChatClient client, ChatClient.Message message)
        {
            if (ReceiveHandler != null) ReceiveHandler(client, message);
        }

        void Client_CloseHandler(ChatClient client)
        {
            _client = null;
            if (CloseHandler != null) CloseHandler(client);
        }

        public ChatClient.Message SendMessage(string sMessage)
        {
            ServerLib.ChatClient.Message message = new ChatClient.Message();
            if (!string.IsNullOrEmpty(ChatUser))
            {
                message.From = ChatUser;
            }
            else
            {
                message.From = _client.Id;
            }
            message.Date = DateTime.Now.ToString();
            message.Content = sMessage;
            Sender s = new Sender()
            {
                Name = "Me"
            };
            _client.SendMessage(s, message);
            if (sMessage.StartsWith("/hello "))
            {
                ChatUser = sMessage.Substring(7).Trim();
                SaveWorkspace();
            }
            return message;
        }

    }
}
