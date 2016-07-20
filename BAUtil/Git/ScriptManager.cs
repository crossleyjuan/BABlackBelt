using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace BABlackBelt.Git
{
    public class ScriptManager
    {
        public static List<Script> LoadScripts(string gitFolder)
        {
            List<Script> result = new List<Script>();
            string scriptsFolder = Path.Combine(gitFolder, "Scripts");
            if (Directory.Exists(scriptsFolder))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(scriptsFolder, "*.xml");
                foreach (string file in files)
                {
                    Script script = new Script();
                    XmlReader reader = XmlReader.Create(new FileStream(file, FileMode.Open));
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);

                    XmlNode query = doc.SelectSingleNode("/Script/Source/Query");
                    script.Query = query.InnerText;
                    script.FileName = doc.SelectSingleNode("/Script/Output/FileName").InnerText;
                    script.Folder = doc.SelectSingleNode("/Script/Output/Folder").InnerText;
                    script.Content = doc.SelectSingleNode("/Script/Output/Content").InnerText;
                    script.ScriptName = doc.SelectSingleNode("/Script/Name").InnerText;

                    result.Add(script);
                }
            }
            return result;
        }
    }
}
