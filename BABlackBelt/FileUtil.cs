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
    }
}
