using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;

namespace BABlackBelt.Git
{
    public class Script
    {
        public string ScriptName { get; set; }

        public string Query { set; get; }
        public string Folder { set; get; }
        public string FileName { set; get; }
        public string Content { set; get; }

        private static System.Text.RegularExpressions.Regex _variable = new System.Text.RegularExpressions.Regex("{{([a-zA-Z_][a-zA-Z0-9_]*)}}");

        private string ReplaceVariables(string s, DataRow row)
        {
            string result = s;
            foreach (Match match in _variable.Matches(s))
            {
                string column = match.Groups[1].Value;
                string columnValue = row[column].ToString();
                result = result.Replace("{{" + column + "}}", columnValue);
            }

            return result;
        }

        public bool Process(DataConnection con, string gitFolder)
        {
            DataTable dt = con.RunQuery(Query);
            StatusScreen screen = StatusScreen.ShowStatus(dt.Rows.Count);

            int current = 0;
            foreach (DataRow row in dt.Rows)
            {
                string file = ReplaceVariables(FileName, row);
                string content = ReplaceVariables(Content, row);
                content = content.Replace("\\n", "\n");
                content = content.Replace("\\r", "\r");
                string destFolder = Path.Combine(gitFolder, Folder);
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }
                string fileName = Path.Combine(gitFolder, Folder, file);
                File.WriteAllText(fileName, content);
                screen.UpdateStatus(current++, "Updating: " + file);
            }
            screen.Close();

            return true;
        }
    }
}
