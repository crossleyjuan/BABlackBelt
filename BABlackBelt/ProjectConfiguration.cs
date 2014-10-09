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
    public partial class ProjectConfiguration : Form
    {
        private System.Windows.Forms.DialogResult _result;

        public string GitFolder
        {
            get { return txtGitFolder.Text; }
        }

        public string ProjectFolder
        {
            get { return txtProjectFolder.Text; }
        }

        public string ConnectionString
        {
            get { return txtConnectionString.Text; }
        }

        public ProjectConfiguration()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Server Git folder";
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string serverFolder = dialog.SelectedPath;
                txtGitFolder.Text = serverFolder;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.None;
        }

        private void btnBrowseProject_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Project Local folder";
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string projectFolder = dialog.SelectedPath;
                txtProjectFolder.Text = projectFolder;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.None;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGitFolder.Text) && !string.IsNullOrEmpty(txtProjectFolder.Text) && !string.IsNullOrEmpty(txtConnectionString.Text))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("All the values are mandatory.");
                DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;

        }

    }
}
