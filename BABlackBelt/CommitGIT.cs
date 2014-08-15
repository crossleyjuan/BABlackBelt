using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using BABlackBelt.Git;

namespace BABlackBelt
{
    public partial class CommitGIT : Form
    {
        List<GitChange> _gitChanges;
        GitUtil _git;
        string _currentBranch = "master";
        string _gitFolder;
        Settings _projectSettings;

        public CommitGIT(string gitFolder)
        {
            InitializeComponent();
            _git = new GitUtil(gitFolder);
            _gitFolder = gitFolder;
            _projectSettings = Settings.getProjectSettings(_gitFolder);
        }

        private void CommitGIT_Load(object sender, EventArgs e)
        {
            RefreshCodeBase();
        }

        private void RefreshCodeBase()
        {
            _git.Reset();

            string connectionString = _projectSettings["ConnectionString"];
            DataConnection con = DataConnectionFactory.getConnection(connectionString);
            DataTable dtRules = con.RunQuery("SELECT * from Bizrule order by ruleName");

            string rulesFolder = Path.Combine( _gitFolder, "Rules");

            // Always cleanup the previous directory to check for deleted rules
            if (Directory.Exists(rulesFolder))
            {
                Directory.Delete(rulesFolder, true);
            }
            Directory.CreateDirectory(rulesFolder);
            // the pull is required prior to commit
            string resultPull = _git.Pull("origin", _currentBranch);

            StatusScreen screen = StatusScreen.ShowStatus(dtRules.Rows.Count);

            int current = 0;
            foreach (DataRow row in dtRules.Rows)
            {
                string ruleFileName = string.Format("{0}.brl", row["ruleName"].ToString());
                string fileName = Path.Combine(rulesFolder, ruleFileName);
                string content = (string)row["ruleFormula"];
                if (content != null)
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
                    FileUtil.SaveFile(fileName, data);
                }
                screen.UpdateStatus(current++, "Refreshing: " + fileName);
            }

            screen.Close();
            _gitChanges = _git.GetStatus();
            lstChanges.Items.Clear();

            foreach (GitChange change in _gitChanges)
            {
                lstChanges.Items.Add(change);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshCodeBase();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCommit.Text))
            {
                return;
            }
            foreach (var item in lstChanges.CheckedItems)
            {
                GitChange change = (GitChange)item;

                if (change.ChangeType != "deleted")
                {
                    _git.Add(change.File);
                }
                else
                {
                    _git.Remove(change.File);
                }
            }

            _git.Commit(txtCommit.Text);

            string resultPull = _git.Pull("origin", _currentBranch);

            GitResult.ShowResult("git pull origin master:\r\n\r\n" + resultPull);

            if (MessageBox.Show("Do you want to push the changes?", "Push", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string pushResult = _git.Push("origin", _currentBranch);

                GitResult.ShowResult("git push origin master:\r\n\r\n" + pushResult);
            }
            RefreshCodeBase();
        }

        private void mnuDiffTool_Click(object sender, EventArgs e)
        {
            GitChange change = (GitChange)lstChanges.SelectedItem;

            if (change != null)
            {
                _git.ShowDiff(change.File);
            }
        }

        private void lstChanges_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                mnuDiffTool_Click(sender, null);
            }
        }

    }
}
