using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BABlackBelt.Git
{
    public class GitChange
    {
        public string ChangeType
        {
            get;
            set;
        }

        public string File
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{0,-15}{1}", ChangeType, File);
        }
    }
}
