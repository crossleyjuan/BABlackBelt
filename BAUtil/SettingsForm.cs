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
            txtFullname.Text = git.GetGlobal("user.name");
            txtEmail.Text = git.GetGlobal("user.email");
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
                Git.GitUtil git = new Git.GitUtil("");
                git.setGlobal("user.name", txtFullname.Text);
                git.setGlobal("user.email", txtEmail.Text);

                Settings settings = Settings.getSettings();
                settings["blackbeltserver"] = txtBlackBeltServer.Text;
                settings["version"] = "1.0";
                settings.saveSettings();

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
            return true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

    }
}
