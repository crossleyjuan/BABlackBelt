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
    public partial class GitResult : Form
    {
        private GitResult()
        {
            InitializeComponent();
        }

        public static void ShowResult(string result)
        {
            GitResult dialog = new GitResult();
            dialog.txtStatus.Text = result;

            dialog.ShowDialog();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
