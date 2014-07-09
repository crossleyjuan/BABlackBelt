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
    public partial class DBUtils : Form
    {
        public DBUtils()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectObjects obj = new SelectObjects();
            obj.ShowDialog();
            List<Entity> entities = obj.SelectedEntities();

            
        }
    }
}
