using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using BABlackBelt.Git;
using ServerLib;

namespace BABlackBelt
{
    public partial class frmMain : Form
    {
        UserWorkspace _workspace;
        static ChatScreen _chat;
        List<ToolStripMenuItem> _mnuExecuteItems = new List<ToolStripMenuItem>();
        List<ToolStripMenuItem> _mnuCancelItems = new List<ToolStripMenuItem>();

        delegate void AsyncCallback(object o);

        public frmMain()
        {
            InitializeComponent();
            _workspace = UserWorkspace.Workspace();
        }


        private void prettifyMyRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckProjectSettings())
            {
                PrettyfyRule pretty = new PrettyfyRule(txtGitFolder.Text);
                pretty.Show();
            }
        }

        private void dBInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void commitRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GitUtil.CheckValidGitFolder(txtGitFolder.Text))
            {
                if (CheckProjectSettings())
                {
                    CommitGIT git = new CommitGIT(txtGitFolder.Text);
                    git.Show();
                }
            }
            else
            {
                MessageBox.Show("Please select the git folder first");
            }
        }

        private void txtGitFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void SelectProjectFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string folder = dialog.SelectedPath;

                if (ValidateProjectFolder(folder))
                {
                    txtGitFolder.Text = folder;
                }
                else
                {
                    MessageBox.Show("The selected folder is not a valid project.");
                }

            }
         }

        private bool ValidateProjectFolder(string folder)
        {
            string projectFileName = Path.Combine(folder, "project.config");
            if (File.Exists(projectFileName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckProjectSettings()
        {
            string ConnectionString = Settings.getProjectSettings(txtGitFolder.Text)["ConnectionString"];
            if (string.IsNullOrEmpty(ConnectionString))
            {
                SettingsForm form = new SettingsForm(txtGitFolder.Text);
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string userDir = Path.Combine( FileUtil.GetUserDirectory(), "bbtool.config");

            Settings globalSettings = Settings.getSettings();

            string lastDir = globalSettings["LastDir"];
            if (!string.IsNullOrEmpty(lastDir))
            {
                if (ValidateProjectFolder(lastDir))
                {
                    txtGitFolder.Text = lastDir;
                }
            }
            if (!string.IsNullOrEmpty(txtGitFolder.Text))
            {
                if (!UserWorkspace.Workspace().LoadProject(txtGitFolder.Text))
                {
                    txtGitFolder.Text = "";
                }
            }
            UserWorkspace.Workspace().ConnectHandler += ConnectHandler;
            UserWorkspace.Workspace().CloseHandler += CloseHandler;
            UserWorkspace.Workspace().ReceiveHandler += ReceivedHandler;

            _chat = ChatScreen.Create();
            _chat.ShowScreen();
            _chat.Hide();

            UserWorkspace.Workspace().ConnectBlackBelt();

        }

        private void CloseHandler(object client)
        {
            if (this.InvokeRequired)
            {
                AsyncCallback d = new AsyncCallback(CloseHandler);
                this.Invoke(d, new object[] { client });
            }
            else
            {
                UpdateCommandStatus("[99:closed]");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = dialog.SelectedPath;

                string checkPath = path + @"\.git";
                if (Directory.Exists(checkPath))
                {
                    txtGitFolder.Text = dialog.SelectedPath;
                }
                else
                {
                    MessageBox.Show("Selected path is not a git folder");
                }

                Settings projectSettings = Settings.getProjectSettings(txtGitFolder.Text);

                if (!UserWorkspace.Workspace().LoadProject(txtGitFolder.Text))
                {
                    txtGitFolder.Text = "";
                }
                UserWorkspace.Workspace().ConnectBlackBelt();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Server Git folder";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string serverFolder = dialog.SelectedPath;

                if (File.Exists(serverFolder + @"\HEAD") && File.Exists(serverFolder + @"\config"))
                {

                    FolderBrowserDialog destFolder = new FolderBrowserDialog();
                    destFolder.Description = "Destination folder";
                    if (destFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string dest = destFolder.SelectedPath;

                        Git.GitUtil git = new Git.GitUtil(dest);

                        string result = git.GitClone(serverFolder, dest);

                        txtGitFolder.Text = dest;
                        GitResult.ShowResult(result);
                    }
                }
                else
                {
                    MessageBox.Show("Selected path is not a git folder");
                }
            }

        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = Settings.getSettings();
            string blackBeltServer = settings["blackbeltserver"];
            SettingsForm form = new SettingsForm(txtGitFolder.Text);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                settings = Settings.getSettings();
                if (settings["blackbeltserver"] != blackBeltServer)
                {
                    UserWorkspace.Workspace().ConnectBlackBelt();
                    _chat.Show();
                }
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void testEmailServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void advancedCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void updateBizagiLogoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateBizagiLogo logo = new frmUpdateBizagiLogo(txtGitFolder.Text);

            logo.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserWorkspace.Workspace().ConnectHandler -= ConnectHandler;
            UserWorkspace.Workspace().CloseHandler -= CloseHandler;
            UserWorkspace.Workspace().ReceiveHandler -= ReceivedHandler;

            string lastProjectUsed = txtGitFolder.Text;
            bool changed = false;
            if (!string.IsNullOrEmpty(lastProjectUsed))
            {
                _workspace.RecentProject = lastProjectUsed;
                changed = true;
            }

            if (changed)
            {
                _workspace.SaveWorkspace();
            }
            if (_workspace.Client != null)
            {
                _workspace.Client.Close();
            }
            if (_chat != null) _chat.CloseForm();
            _chat = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProjectConfiguration conf = new ProjectConfiguration();
            if (conf.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string serverFolder = conf.GitFolder;
                string projectFolder = conf.ProjectFolder;
                string connectionString = conf.ConnectionString;

                StatusScreen fullProcessScreen = StatusScreen.ShowStatus(5);

                Git.GitUtil git = new GitUtil(serverFolder);
                git.ExecuteGitCommand("init --bare", true);
                fullProcessScreen.UpdateStatus(1, "Git folder initialized");

                git = new GitUtil(projectFolder);
                string result = git.GitClone(serverFolder, projectFolder);

                fullProcessScreen.UpdateStatus(2, "Git folder cloned");

                Settings projectSettings = Settings.getProjectSettings(projectFolder);

                projectSettings["ConnectionString"] = connectionString;

                projectSettings.saveSettings();

                fullProcessScreen.UpdateStatus(3, "Project Folder Initialized");

                git.Add("project.config");
                git.Commit("Initial commit");

                result = git.Push("origin", "master");
                fullProcessScreen.UpdateStatus(4, "Ready to use");

                fullProcessScreen.Close();

                txtGitFolder.Text = projectFolder;
            }
        }

        private void xLSTLabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XLSTLab lab = new XLSTLab();
            lab.Show();
        }

        private void testUDPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            if (string.IsNullOrEmpty(txtGitFolder.Text))
            {
                MessageBox.Show("Please load a project first before connecting to the chat application");
                return;
            }
             * */
            if (UserWorkspace.Workspace().Client == null)
            {
                if (UserWorkspace.Workspace().ConnectBlackBelt())
                {
                    if (_chat != null)
                    {
                        _chat.CloseForm();
                    }
                    _chat = ChatScreen.Create();
                }
                else
                {
                    if (_chat != null) _chat.CloseForm();
                    _chat = null;
                }
            }
            if (_chat != null)
            {
                _chat.ShowScreen();
            }
        }

        public void EnableCommands(object message)
        {
            if (this.InvokeRequired)
            {
                AsyncCallback d = new AsyncCallback(EnableCommands);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                if (((string)message).StartsWith("[01")) // Requested
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[02")) // Started
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[03")) // Completed
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[04")) // Cancelled
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[09")) // Available Commands
                {
                    LoadCommands((string)message);
                }
            }
        }

        private void UpdateCommandStatus(string message)
        {
            string ev = message.Substring(1, message.IndexOf(":") - 1);
            string command = message.Substring(message.IndexOf(":") + 1);
            command = command.Substring(0, command.IndexOf("]"));

            ToolStripMenuItem executeItem = null;
            ToolStripMenuItem cancelItem = null;
            foreach (ToolStripMenuItem item in _mnuExecuteItems)
            {
                if (item.Tag.ToString() == command)
                {
                    executeItem = item;
                    break;
                }
            }
            foreach (ToolStripMenuItem item in _mnuCancelItems)
            {
                if (item.Tag.ToString() == command)
                {
                    cancelItem = item;
                    break;
                }
            }
            if ((cancelItem != null) && (executeItem != null))
            {
                switch (ev)
                {
                    case "01": // requested
                        cancelItem.Enabled = true;
                        executeItem.Enabled = false;
                        break;
                    case "02": // Started
                        cancelItem.Enabled = false;
                        executeItem.Enabled = false;
                        break;
                    case "03": // Completed
                    case "04": // Cancelled
                        cancelItem.Enabled = false;
                        executeItem.Enabled = true;
                        break;
                    case "99":
                            cancelItem.Enabled = false;
                        if (command == "connected")
                        {
                            executeItem.Enabled = true;
                        }
                        else if (command == "closed")
                        {
                            executeItem.Enabled = false;
                        }
                        break;
                    default:
                        cancelItem.Enabled = false;
                        executeItem.Enabled = true;
                        break;
                }
            }
        }

        private void EnableCommandControls(object message)
        {
            if (this.InvokeRequired)
            {
                AsyncCallback d = new AsyncCallback(EnableCommandControls);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                if (((string)message).StartsWith("[01")) // Requested
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[02")) // Started
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[03")) // Completed
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[04")) // Cancelled
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[99")) // Internal Commands
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[09")) // Available Commands
                {
                    LoadCommands((string)message);
                }
            }
        }

        private void LoadCommands(string messageAvailableCommands)
        {
            List<Command> cmds = Command.ParseCommands(messageAvailableCommands);
            commandsToolStripMenuItem.DropDownItems.Clear();
            _mnuCancelItems.Clear();
            _mnuExecuteItems.Clear();

            foreach (Command cmd in cmds)
            {
                ToolStripMenuItem item = new ToolStripMenuItem("Execute " + cmd.Text);
                item.Tag = cmd.Key;
                item.Click += OnExecuteCommand_Click;
                item.Enabled = true;
                commandsToolStripMenuItem.DropDownItems.Add(item);
                _mnuExecuteItems.Add(item);
                ToolStripMenuItem itemCancel = new ToolStripMenuItem("Cancel " + cmd.Text);
                itemCancel.Tag = cmd.Key;
                itemCancel.Click += OnCancelCommand_Click;
                itemCancel.Enabled = false;
                commandsToolStripMenuItem.DropDownItems.Add(itemCancel);
                _mnuCancelItems.Add(itemCancel);
            }
        }

        private void OnCancelCommand_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                string command = (string)item.Tag;
                UserWorkspace.Workspace().SendMessage("/cancel " + command);
            }
        }

        private void OnExecuteCommand_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                string command = (string)item.Tag;
                UserWorkspace.Workspace().SendMessage("/command " + command);
            }
        }

        public void ReceivedHandler(ChatClient client, ServerLib.ChatClient.Message message)
        {
            EnableCommandControls(message.Content);
        }

        private void ConnectHandler(object client)
        {
            if (this.InvokeRequired)
            {
                AsyncCallback d = new AsyncCallback(ConnectHandler);
                this.Invoke(d, new object[] { client });
            }
            else
            {
                UpdateCommandStatus("[99:connected]");
            }
        }

    }
}
