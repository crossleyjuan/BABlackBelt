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
        public frmMain()
        {
            InitializeComponent();
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
            System.Collections.ArrayList a = new System.Collections.ArrayList();
            a.Add(null);
            a.Add(null);
            a.Add(1);
            string user = Settings.getSettings()["git_fullname"];
            string gitFolder = (string)ConfigurationSettings.AppSettings["GitFolder"];
            if (string.IsNullOrEmpty(user))
            {
                SettingsForm form = new SettingsForm(gitFolder);
                form.ShowDialog();
            }

            txtGitFolder.Text = gitFolder;
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
    }
}
