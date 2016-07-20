using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BABlackBelt
{
    class DataConnectionFactory
    {

        private static DataConnection _connection;
        private static Dictionary<string, DataConnection> _connections = new Dictionary<string, DataConnection>();

        public static DataConnection getConnection(Settings projectSettings)
        {
            string connectionString = projectSettings["ConnectionString"];
            string sType = projectSettings["DBType"];
            DBTYPE type = DataUtil.GetDatabaseType(sType);

            if (!_connections.ContainsKey(connectionString))
            {
                DataConnection tempCon;
                switch (type)
                {
                    case DBTYPE.SQL:
                        tempCon = new SQLDataConnection(connectionString);
                        break;
                    case DBTYPE.ORACLE:
                        tempCon = new OracleDataConnection(connectionString);
                        break;
                    default:
                        throw new Exception("Unsupported connection type: " + type);
                }
                _connections[connectionString] = tempCon;
                tempCon.Open();
            }
            DataConnection con = _connections[connectionString];
            return con;
        }
    }
}
