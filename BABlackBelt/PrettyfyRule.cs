using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BABlackBelt
{
    public partial class PrettyfyRule : Form
    {
        public PrettyfyRule()
        {
            InitializeComponent();
        }

        private void PrettyfyRule_Load(object sender, EventArgs e)
        {
            DataConnection con = DataConnectionFactory.getConnection();
            DataTable dtRules = con.RunQuery("SELECT * from Bizrule order by ruleName");

            foreach (DataRow row in dtRules.Rows)
            {
                comboBox1.Items.Add(row["ruleName"]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataConnection con = DataConnectionFactory.getConnection();
            DataTable dtRules = con.RunQuery("SELECT * from Bizrule where ruleName = '" + comboBox1.Text + "'");

            textBox1.Text = "";
            if (dtRules.Rows.Count > 0)
            {
                string text = Convert.ToString(dtRules.Rows[0]["ruleFormula"]);
                text = text.Replace("\r\n", "\n");
                text = text.Replace("\n", "\r\n");
                textBox1.Text = text;
            }
            btnSave.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataConnection con = DataConnectionFactory.getConnection();
            DataTable dtRules = con.RunQuery("SELECT * from Bizrule where ruleName = '" + comboBox1.Text + "'");

            string ruleFormula = textBox1.Text;

            ruleFormula = ruleFormula.Replace("\r\n", "\n");

            string sql = "UPDATE BIZRULE SET RuleFormula = @RuleFormula WHERE idRule = " + Convert.ToString(dtRules.Rows[0]["idRule"]);

            con.CreateParameter("RuleFormula", SqlDbType.VarChar, ruleFormula);

            con.ExecuteUpdate(sql);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            btnSave.Enabled = false;
            btnLoad.Enabled = true;
        }
    }
}
