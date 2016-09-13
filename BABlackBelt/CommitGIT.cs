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
        bool _filteredList;

        public CommitGIT(string gitFolder)
        {
            InitializeComponent();
            _git = new GitUtil(gitFolder);
            _gitFolder = gitFolder;
            _projectSettings = Settings.getProjectSettings(_gitFolder);
            _filteredList = false;
        }

        private void CommitGIT_Load(object sender, EventArgs e)
        {
            RefreshCodeBase();
        }

        private void RefreshCodeBase()
        {
            this.Cursor = Cursors.WaitCursor;

            try { 
                _git.Reset();

                BAFolderSyncManager.SyncElements(_git, _currentBranch, _projectSettings, _gitFolder);

                _gitChanges = _git.GetStatus();
                showChanges();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("Error", "Git has generated and error: " + ex.Message);
                return;
            }
        }

        private void showChanges()
        {
            lstChanges.Items.Clear();

            string filter = txtFilter.Text;

            filter = filter.ToUpper();
            _filteredList = !string.IsNullOrEmpty(filter);

            foreach (GitChange change in _gitChanges)
            {
                bool filtered = false;
                if (!string.IsNullOrEmpty(filter))
                {
                    if (change.File.ToUpper().IndexOf(filter) == -1)
                    {
                        filtered = true;
                    }
                }
                if (!filtered)
                {
                    lstChanges.Items.Add(change);
                }
            }

            this.Cursor = Cursors.Default;
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
            string resultPull;
            try
            {
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

                resultPull = _git.Pull("origin", _currentBranch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", "Git has generated and error: " + ex.Message);
                return;
            }


            bool requireConfirmation = false;
            if (resultPull.IndexOf("Already up-to-date") < 0)
            {
                GitResult.ShowResult("git pull origin master:\r\n\r\n" + resultPull);
                requireConfirmation = true;
            }
            bool performPush = true;
            if (requireConfirmation && MessageBox.Show("Do you want to push the changes?", "Push", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                performPush = false;
            }
            if (performPush)
            {
                try { 
                    string pushResult = _git.Push("origin", _currentBranch);

                    GitResult.ShowResult("git push origin master:\r\n\r\n" + pushResult);
                    RefreshCodeBase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error", "Git has generated and error: " + ex.Message);
                    return;
                }
            }
        }

        private void mnuDiffTool_Click(object sender, EventArgs e)
        {
            GitChange change = (GitChange)lstChanges.SelectedItem;

            if (change != null)
            {
                try
                {
                    _git.ShowDiff(change.File);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error", "Git has generated and error: " + ex.Message);
                    return;
                }
            }
        }

        private void lstChanges_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                mnuDiffTool_Click(sender, null);
            }
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            showChanges();
            RefreshFilterStatus();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            RefreshFilterStatus();
        }

        private void RefreshFilterStatus()
        {
            if (string.IsNullOrEmpty(txtFilter.Text) && _filteredList)
            {
                btnApplyFilter.Text = "Remove Filter";
            }
            else
            {
                btnApplyFilter.Text = "Apply Filter";
            }
        }

    }
}
