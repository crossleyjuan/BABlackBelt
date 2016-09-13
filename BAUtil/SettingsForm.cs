using BABlackBelt.Git;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BABlackBelt
{
    public partial class SettingsForm : Form
    {
        Settings _projectSettings;
        Settings _userSettings;

        public SettingsForm(string projectFolder)
        {
            InitializeComponent();
            _projectSettings = Settings.getProjectSettings(projectFolder);
            _userSettings = Settings.getSettings();
        }

        public SettingsForm()
        {
            InitializeComponent();
            _projectSettings = null;
            _userSettings = Settings.getSettings();
            gProjectInfo.Enabled = false;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Git.GitUtil git = new Git.GitUtil("");
            try
            {
                txtFullname.Text = git.GetGlobal("user.name");
                txtEmail.Text = git.GetGlobal("user.email");
                txtGitFolder.Text = GitUtil.GetGitFolder();
            }
            catch
            {
                // Exceptions are ignored as they will fail because of the configuration
            }
            txtBlackBeltServer.Text = _userSettings["blackbeltserver"];
            if (_projectSettings != null)
            {
                txtConnectionString.Text = _projectSettings["ConnectionString"];
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (CheckSettings())
            {
                Settings settings = Settings.getSettings();
                settings["blackbeltserver"] = txtBlackBeltServer.Text;
                settings["version"] = "1.0";
                settings["Git_PATH"] = txtGitFolder.Text;
                settings.saveSettings();

                Git.GitUtil git = new Git.GitUtil("");
                if (!string.IsNullOrEmpty(txtFullname.Text))
                {
                    git.setGlobal("user.name", txtFullname.Text);
                }
                if (!string.IsNullOrEmpty(txtEmail.Text))
                {
                    git.setGlobal("user.email", txtEmail.Text);
                }

                if (_projectSettings != null)
                {
                    _projectSettings["ConnectionString"] = txtConnectionString.Text;
                    _projectSettings.saveSettings();
                }
            }
            else
            {
                
            }
        }

        private bool CheckSettings()
        {
            if (string.IsNullOrEmpty(txtConnectionString.Text) && _projectSettings != null)
            {
                MessageBox.Show("Connection String is required");
                return false;
            }
            if (!string.IsNullOrEmpty(txtConnectionString.Text))
            {
                try
                {
                    DataConnection tempCon = new DataConnection(txtConnectionString.Text);
                    tempCon.Open();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error trying to connect to the database, please check the connection information and try again");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(txtBlackBeltServer.Text))
            {
                MessageBox.Show("BlackBelt server is required");
                return false;
            }
            if (string.IsNullOrEmpty(txtGitFolder.Text))
            {
                MessageBox.Show("Git Installation folder is required");
                return false;
            }
            if (!GitUtil.CheckValidGitFolderInstalation(txtGitFolder.Text))
            {
                MessageBox.Show("Git Installation folder is invalid please select the folder where the bin/git.exe is installed on.");
                return false;
            }
            return true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btnSelectGitFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string folder = dialog.SelectedPath;

                if (GitUtil.CheckValidGitFolderInstalation(folder))
                {
                    txtGitFolder.Text = folder;
                }
                else
                {
                    MessageBox.Show("Please select the folder where git is installed, it has to have a bin folder on it with git.exe on it.");
                }

            }
        }

    }
}
