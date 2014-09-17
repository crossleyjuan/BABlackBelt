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
    public partial class frmUpdateBizagiLogo : Form
    {
        Settings _projectSettings;

        public frmUpdateBizagiLogo(string gitFolder)
        {
            InitializeComponent();
            _projectSettings = Settings.getProjectSettings(gitFolder);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();


            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFilePath.Text = dialog.FileName;
                btnUpload.Enabled = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            string connectionString = _projectSettings["ConnectionString"];
            DataConnection con = DataConnectionFactory.getConnection(connectionString);

            byte[] bytes = FileUtil.LoadFile(txtFilePath.Text);
            con.CreateParameter("fileData", SqlDbType.Image, bytes);

            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE BAWPFILETHEME SET fileData = @fileData WHERE idTheme = 10000 and idFilePath = 4");

            con.ExecuteUpdate(sb.ToString());

            MessageBox.Show("Logo updated!");
        }
    }
}
