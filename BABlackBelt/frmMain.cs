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

namespace BABlackBelt
{
    public partial class frmMain : Form
    {
        UserWorkspace _workspace;
        public frmMain()
        {
            InitializeComponent();
            _workspace = new UserWorkspace();
            _workspace.Initialize();

            if (!string.IsNullOrEmpty(_workspace.RecentProject))
            {
                txtGitFolder.Text = _workspace.RecentProject;
            }
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
            globalSettings.LoadSettings(userDir);

            string lastDir = globalSettings["LastDir"];
            if (!string.IsNullOrEmpty(lastDir))
            {
                if (ValidateProjectFolder(lastDir))
                {
                    txtGitFolder.Text = lastDir;
                }
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
            SettingsForm form = new SettingsForm(txtGitFolder.Text);
            form.ShowDialog();
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
    }
}
