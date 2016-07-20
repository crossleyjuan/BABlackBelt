using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace BABlackBelt
{
    public class DataConnection
    {
        SqlConnection _connection;
        class DataParameter
        {
            public string Name;
            public SqlDbType DataType;
            public object Value;
        }

        List<DataParameter> _parameters = new List<DataParameter>();
        private string _connectionString;

        public DataConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Open()
        {
            _connection = new SqlConnection();

            _connection.ConnectionString = _connectionString;
            _connection.Open();

        }

        public int ExecuteUpdate(string sql)
        {
            SqlCommand cmd =  _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;
            FillParameters(cmd);

            _parameters.Clear();
            return cmd.ExecuteNonQuery();
        }

        private void FillParameters(SqlCommand cmd)
        {
            foreach (DataParameter parameter in _parameters)
            {
                SqlParameter param = cmd.Parameters.Add(parameter.Name, parameter.DataType);
                param.Value = parameter.Value;
            }
        }

        public DataTable RunQuery(string sql)
        {
            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;

            FillParameters(cmd);
            
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;

            DataSet ds = new DataSet();
            adapter.Fill(ds);

            _parameters.Clear();
            return ds.Tables[0];
        }


        public void CreateParameter(string name, SqlDbType type, object value)
        {
            DataParameter param = new DataParameter()
            {
                Name = name,
                DataType = type,
                Value = value
            };

            _parameters.Add(param);
        }
    }
}
