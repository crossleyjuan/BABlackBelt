using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace BABlackBelt.Git
{
    class BAFolderSyncManager
    {
        class AttribInfo
        {
            public int idAttrib;
            public string attribName;

            public override string ToString()
            {
                return string.Format("{{\r\n      Id: {0},\r\n      Name: \"{1}\"\r\n   }}", idAttrib, attribName);
            }
        }

        class EntityInfo
        {
            public List<AttribInfo> Attributes = new List<AttribInfo>();
            public int idEnt;
            public string entName;
            public string entDisplayName;

            public override string ToString()
            {
                StringBuilder sbAttribs = new StringBuilder();
                foreach (AttribInfo attr in Attributes)
                {
                    if (sbAttribs.Length > 0)
                    {
                        sbAttribs.AppendFormat(", {0}", attr.ToString());
                    }
                    else
                    {
                        sbAttribs.AppendFormat("{0}", attr.ToString());
                    }
                }

                if (entDisplayName != null)
                {
                    return string.Format("{{\r\n   Id: {0},\r\n   Name: \"{1}\",\r\n   DisplayName: \"{2}\",\r\n   Attributes: [{3}]\r\n}}", idEnt, entName, entDisplayName, sbAttribs.ToString());
                }
                else
                {
                    return string.Format("{{\n   Id: {0},\n   Name: \"{1}\",\n   Attributes: [{2}]\n}}", idEnt, entName, sbAttribs.ToString());
                }
            }
        }

        private static void CleanupFolders(string gitFolder)
        {
            string rulesFolder = Path.Combine(gitFolder, "Rules");

            // Always cleanup the previous directory to check for deleted rules
            if (Directory.Exists(rulesFolder))
            {
                Directory.Delete(rulesFolder, true);
            }
            Directory.CreateDirectory(rulesFolder);

            string entitiesFolder = Path.Combine(gitFolder, "Entities");
            // Always cleanup the previous directory to check for deleted entities (renamed)
            if (Directory.Exists(entitiesFolder))
            {
                Directory.Delete(entitiesFolder, true);
            }
            Directory.CreateDirectory(entitiesFolder);

            string formsFolder = Path.Combine(gitFolder, "Forms");
            // Always cleanup the previous directory to check for deleted entities (renamed)
            if (Directory.Exists(formsFolder))
            {
                Directory.Delete(formsFolder, true);
            }
            Directory.CreateDirectory(formsFolder);

        }

        private static void RunRulesRefresh(DataConnection con, string folder)
        {
            StatusScreen screen = StatusScreen.ShowStatus(0);

            string rulesFolder = Path.Combine(folder, "Rules");
            if (!Directory.Exists(rulesFolder))
            {
                Directory.CreateDirectory(rulesFolder);
            }

            DataTable dtRules = con.RunQuery("SELECT * from Bizrule order by ruleName");
            screen.MaxValue += dtRules.Rows.Count;

            int current = 0;
            foreach (DataRow row in dtRules.Rows)
            {
                string ruleFileName = string.Format("{0}.brl", row["ruleName"].ToString());
                string fileName = Path.Combine(rulesFolder, ruleFileName);
                string content = (string)row["ruleFormula"];
                if (content != null)
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
                    FileUtil.SaveFile(fileName, data);
                }
                screen.UpdateStatus(current++, "Refreshing: " + fileName);
            }
            screen.Close();
        }

        private static void RunFormsRefresh(DataConnection con, string folder)
        {
            StatusScreen screen = StatusScreen.ShowStatus(0);

            string formsFolder = Path.Combine(folder, "Forms");
            if (!Directory.Exists(formsFolder))
            {
                Directory.CreateDirectory(formsFolder);
            }

            DataTable dtForms = con.RunQuery("SELECT objName, objContent from babizagicatalog where objtype = 52 order by objName");
            screen.MaxValue += dtForms.Rows.Count;

            int current = 0;
            foreach (DataRow row in dtForms.Rows)
            {
                string formFileName = string.Format("{0}.frm", row["objName"].ToString());
                formFileName = formFileName.Replace("\r", "");
                formFileName = formFileName.Replace("\n", "");
                string fileName = Path.Combine(formsFolder, formFileName);
                string content = (string)row["objContent"];
                if (content != null)
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
                    FileUtil.SaveFile(fileName, data);
                }
                screen.UpdateStatus(current++, "Refreshing: " + fileName);
            }
            screen.Close();
        }

        private static EntityInfo GetEntityInfo(DataConnection con, DataRow row)
        {
            EntityInfo entity = new EntityInfo
            {
                idEnt = Convert.ToInt32(row["idEnt"]),
                entName = Convert.ToString(row["entName"])
            };

            if (row["entDisplayName"] != null)
            {
                entity.entDisplayName = Convert.ToString(row["entDisplayName"]);
            }

            DataTable dtAttrib = con.RunQuery(string.Format("SELECT * from Attrib where idEnt = {0} order by idAttrib", entity.idEnt));
            foreach (DataRow rowAttrib in dtAttrib.Rows)
            {
                AttribInfo attrInfo = new AttribInfo
                {
                    idAttrib = Convert.ToInt32(rowAttrib["idAttrib"]),
                    attribName = Convert.ToString(rowAttrib["attribName"])
                };
                entity.Attributes.Add(attrInfo);
            }

            return entity;
        }

        private static void RunEntitiesRefresh(DataConnection con, string folder)
        {
            StatusScreen screen = StatusScreen.ShowStatus(0);

            string entitiesFolder = Path.Combine(folder, "Entities");
            if (!Directory.Exists(entitiesFolder))
            {
                Directory.CreateDirectory(entitiesFolder);
            }

            DataTable dtRules = con.RunQuery("SELECT * from Entity order by entName");
            screen.MaxValue += dtRules.Rows.Count;

            int current = 0;
            foreach (DataRow row in dtRules.Rows)
            {
                string entFileName = string.Format("{0}.ent", row["entName"].ToString());
                string fileName = Path.Combine(entitiesFolder, entFileName);

                EntityInfo entity = GetEntityInfo(con, row);
                string content = (string)entity.ToString();
                if (content != null)
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
                    FileUtil.SaveFile(fileName, data);
                }
                screen.UpdateStatus(current++, "Refreshing: " + fileName);
            }
            screen.Close();
        }

         public static void SyncElements(Git.GitUtil gitUtil, string currentBranch, Settings projectSettings, string folder)
        {
            CleanupFolders(folder);

            DataConnection con = DataConnectionFactory.getConnection(projectSettings);

            StatusScreen fullProcessScreen = StatusScreen.ShowStatus(2);

            RunRulesRefresh(con, folder);
            fullProcessScreen.UpdateStatus(1, "Rules updated");

            RunEntitiesRefresh(con, folder);
            fullProcessScreen.UpdateStatus(2, "Entities updated");

             /*
            RunFormsRefresh(con, folder);
            fullProcessScreen.UpdateStatus(3, "Forms updated");
             */
            fullProcessScreen.Close();
        }
    }
}
