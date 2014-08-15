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

        public SettingsForm(string projectFolder)
        {
            InitializeComponent();
            _projectSettings = Settings.getProjectSettings(projectFolder);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Git.GitUtil git = new Git.GitUtil("");
            txtFullname.Text = Settings.getSettings()["git_fullname"];
            if (string.IsNullOrEmpty(txtFullname.Text))
            {
                txtFullname.Text = git.GetGlobal("user.name");
            }
            txtEmail.Text = Settings.getSettings()["git_email"];
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                txtEmail.Text = git.GetGlobal("user.email");
            }

            txtConnectionString.Text = _projectSettings["ConnectionString"];
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Git.GitUtil git = new Git.GitUtil("");
            git.setGlobal("user.name", txtFullname.Text);
            git.setGlobal("user.email", txtEmail.Text);

            Settings settings = Settings.getSettings();
            settings["git_fullname"] = txtFullname.Text;
            settings["git_email"] = txtEmail.Text;

            settings.saveSettings();

            _projectSettings["ConnectionString"] = txtConnectionString.Text;

            _projectSettings.saveSettings();
        }
    }
}
