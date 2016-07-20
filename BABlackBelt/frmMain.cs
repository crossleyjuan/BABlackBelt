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
            bool connected = UserWorkspace.Workspace().ConnectBlackBelt();

            _chat = ChatScreen.Create();
            _chat.EnableControls(connected);
            _chat.ShowScreen();
            _chat.Hide();
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
            _workspace.Client.Close();
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
    }
}
