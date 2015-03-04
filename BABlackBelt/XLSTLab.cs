using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Xsl;
using System.Xml;
using System.IO;

namespace BABlackBelt
{
    public partial class XLSTLab : Form
    {
        public XLSTLab()
        {
            InitializeComponent();
        }

        private void XLSTLab_Load(object sender, EventArgs e)
        {
            scintilla1.ConfigurationManager.Language = "xml";
            scintilla2.ConfigurationManager.Language = "xml";
            scintilla3.ConfigurationManager.Language = "xml";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                XslCompiledTransform transform = new XslCompiledTransform();
                TextReader reader = new System.IO.StringReader(scintilla2.Text);
                XmlTextReader xmlReader = new XmlTextReader(reader);
                transform.Load(xmlReader);

                XmlDocument doc1 = new XmlDocument();
                doc1.LoadXml(scintilla1.Text);

                StringWriter swriter = new StringWriter();
                XmlTextWriter writer = new XmlTextWriter(swriter);
                writer.Formatting = Formatting.Indented;

                transform.Transform(doc1.CreateNavigator(), null, writer);
                scintilla3.Text = swriter.ToString();
            }
            catch (Exception ex)
            {
                scintilla3.Text = ex.Message + "\n" + ex.StackTrace;
            }
        }
    }
}
