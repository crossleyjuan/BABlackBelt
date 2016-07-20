using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BABlackBelt
{
    public class Util
    {
        public DBTYPE GetDBType(string type)
        {
            switch (type.ToUpper())
            {
                case "SQL":
                    return DBTYPE.SQL;
                case "ORACLE":
                    return DBTYPE.ORACLE;
                default:
                    throw new Exception("Undefined type: " + type);
            }
        }
    }
}
