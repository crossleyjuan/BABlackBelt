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
    public partial class StatusScreen : Form
    {
        private StatusScreen(int max)
        {
            InitializeComponent();
            this.pbStatus.Maximum = max;
        }

        public static StatusScreen ShowStatus(int max)
        {
            StatusScreen screen = new StatusScreen(max);
            screen.Show();
            return screen;
        }

        public void UpdateStatus(int value, string message)
        {
            txtMessage.Text = message;
            pbStatus.Value = value;
            txtMessage.Refresh();
        }

    }
}
