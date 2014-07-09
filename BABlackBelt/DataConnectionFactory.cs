using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BABlackBelt
{
    class DataConnectionFactory
    {

        private static DataConnection _connection;

        public static DataConnection getConnection()
        {
            if (_connection == null)
            {
                DataConnection con = new DataConnection();
                con.Open();
                _connection = con;
            }
            return _connection;

        }
    }
}
