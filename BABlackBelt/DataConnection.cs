using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BABlackBelt
{
    public interface DataConnection
    {
        void Open();
        int ExecuteUpdate(string sql);

        DataTable RunQuery(string sql);
        void CreateParameter(string name, DBDataType type, object value);
    }
}
