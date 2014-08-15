using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BABlackBelt
{
    public partial class PrettyfyRule : Form
    {
        Settings _projectSettings;

        public PrettyfyRule(string projectFolder)
        {
            InitializeComponent();
            _projectSettings = Settings.getProjectSettings(projectFolder);
        }

        private void PrettyfyRule_Load(object sender, EventArgs e)
        {
            DataConnection con = DataConnectionFactory.getConnection(_projectSettings["ConnectionString"]);
            DataTable dtRules = con.RunQuery("SELECT * from Bizrule order by ruleName");

            foreach (DataRow row in dtRules.Rows)
            {
                comboBox1.Items.Add(row["ruleName"]);
            }
        }

        private void FormatCode(RichTextBox textBox)
        {
            Regex regexString = new Regex("\"[^\"]*\"");

            string originalText = textBox.Text;

            string text = originalText;
            string[] lines = text.Split('\n');
            StringBuilder sbTmp = new StringBuilder();
            Dictionary<string, string> strs = new Dictionary<string, string>();
            int index = 0;
            foreach (string line in lines)
            {
                bool found;
                string finalLine = line;
                do
                {
                    found = false;
                    Match keyWordMatch = regexString.Match(finalLine);
                    if (keyWordMatch.Success)
                    {
                        string s = finalLine.Substring(keyWordMatch.Index, keyWordMatch.Length);

                        string key = "StrIndex__Elements__" + index;
                        strs.Add(key, s);

                        finalLine = s.Substring(0, keyWordMatch.Index) + key + s.Substring(keyWordMatch.Index + keyWordMatch.Length);
                        
                        found = true;
                    }
                } while (found);

                sbTmp.AppendFormat("{0}\n", finalLine); 
            }
            Regex keyWords = new Regex("\babstract\b|\bas\b|\bbase\b|\bbool\b|\bbreak\b|\bbyte\b|\bcase\b|\bcatch\b|\bchar\b|\bchecked\b|\bclass\b|\bconst\b|\bcontinue\b|\bdecimal\b|\bdefault\b|\bdelegate\b|\bdo\b|\bdouble\b|\belse\b|\benum\b|\bevent\b|\bexplicit\b|\bextern\b|\bfalse\b|\bfinally\b|\bfixed\b|\bfloat\b|\bfor\b|\b" +

             "foreach\b|\bgoto\b|\bif\b|\bimplicit\b|\bin\b|\bint\b|\binterface\b|\binternal\b|\bis\b|\block\b|\blong\b|\bnamespace\b|\bnew\b|\bnull\b|\bobject\b|\boperator\b|\bout\b|\boverride\b|\bparams\b|\bprivate\b|\bprotected\b|\bpublic\b|\breadonly\b|\bref\b|\breturn\b|\bsbyte\b|\bsealed\b|\bshort\b|\bsizeof\b|\bstackalloc\b|\bstatic\b|\b" + "string\b|\bstruct\b|\bswitch\b|\bthis\b|\bthrow\b|\btrue\b|\btry\b|\btypeof\b|\buint\b|\bulong\b|\bunchecked\b|\bunsafe\b|\bushort\b|\busing\b|\bvirtual\b|\bvolatile\b|\bvoid\b|\bwhile\b|");

            //For each match from the regex, highlight the word.  

            foreach (Match keyWordMatch in keyWords.Matches(textBox.Text))
            {
                textBox.Select(keyWordMatch.Index, keyWordMatch.Length);

                textBox.SelectionColor = Color.Blue;

                textBox.SelectionStart = 0;

                textBox.SelectionColor = Color.Black;

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRuleCode.Text = "";
            btnSave.Enabled = false;
            btnLoad.Enabled = true;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void OnFind(string text)
        {
            int start = txtRuleCode.SelectionStart;
            if (start > 0)
            {
                start++;
            }
            int sel = txtRuleCode.Find(text, start, RichTextBoxFinds.None);
            txtRuleCode.ScrollToCaret();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindDialog box = new FindDialog();
            box.OnFind = OnFind;
            box.ShowDialog();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            DataConnection con = DataConnectionFactory.getConnection(_projectSettings["ConnectionString"]);
            DataTable dtRules = con.RunQuery("SELECT * from Bizrule where ruleName = '" + comboBox1.Text + "'");

            txtRuleCode.Text = "";
            if (dtRules.Rows.Count > 0)
            {
                string text = Convert.ToString(dtRules.Rows[0]["ruleFormula"]);
                text = text.Replace("\r\n", "\n");
                text = text.Replace("\n", "\r\n");
                txtRuleCode.Text = text;
                //FormatCode(txtRuleCode);
            }
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataConnection con = DataConnectionFactory.getConnection(_projectSettings["ConnectionString"]);
            DataTable dtRules = con.RunQuery("SELECT * from Bizrule where ruleName = '" + comboBox1.Text + "'");

            string ruleFormula = txtRuleCode.Text;

            ruleFormula = ruleFormula.Replace("\r\n", "\n");

            string sql = "UPDATE BIZRULE SET RuleFormula = @RuleFormula WHERE idRule = " + Convert.ToString(dtRules.Rows[0]["idRule"]);

            con.CreateParameter("RuleFormula", SqlDbType.VarChar, ruleFormula);

            con.ExecuteUpdate(sql);
        }

    }
}
