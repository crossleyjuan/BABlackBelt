using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BABlackBelt
{
    public class FileUtil
    {

        public static void SaveFile(string fileName, byte[] data)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);

            fs.Write(data, 0, data.Length);

            fs.Close();
        }

        public static byte[] LoadFile(string file)
        {
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);

            byte[] buffer = new byte[1024];
            MemoryStream ms = new MemoryStream();
            int readed = 0;

            while ((readed = fs.Read(buffer, 0, 1024)) > 0)
            {
                ms.Write(buffer, 0, readed);
            }
            ms.Flush();

            fs.Close();

            return ms.ToArray();
        }

        public static string GetUserDirectory()
        {
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                path = Directory.GetParent(path).ToString();
            }
            return path;
        }

        public static void createFolder(string destFolder)
        {
            Directory.CreateDirectory(destFolder);
        }
    }
}
