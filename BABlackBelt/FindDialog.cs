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
    public partial class FindDialog : Form
    {
        public delegate void OnFindMethod(string text);

        public OnFindMethod OnFind;

        public FindDialog()
        {
            InitializeComponent();
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            OnFind(txtText.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
