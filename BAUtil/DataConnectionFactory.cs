using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BABlackBelt
{
    public class DataConnectionFactory
    {

        private static DataConnection _connection;
        private static Dictionary<string, DataConnection> _connections = new Dictionary<string, DataConnection>();

        public static DataConnection getConnection(string connectionString)
        {
            if (!_connections.ContainsKey(connectionString))
            {
                DataConnection tempCon = new DataConnection(connectionString);
                _connections[connectionString] = tempCon;
                tempCon.Open();
            }
            DataConnection con = _connections[connectionString];
            return con;
        }
    }
}
