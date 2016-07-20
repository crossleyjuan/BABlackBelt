using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Data;

namespace BABlackBelt
{
    public class OracleDataConnection: DataConnection
    {

        OracleConnection _connection;
        class DataParameter
        {
            public string Name;
            public OracleDbType DataType;
            public object Value;
        }

        List<DataParameter> _parameters = new List<DataParameter>();
        private string _connectionString;

        public OracleDataConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Open()
        {
            _connection = new OracleConnection ();

            _connection.ConnectionString = _connectionString;
            _connection.Open();

        }

        public int ExecuteUpdate(string sql)
        {
            OracleCommand cmd =  _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;
            FillParameters(cmd);

            _parameters.Clear();
            return cmd.ExecuteNonQuery();
        }

        private void FillParameters(OracleCommand cmd)
        {
            foreach (DataParameter parameter in _parameters)
            {
                OracleParameter param = cmd.Parameters.Add(parameter.Name, parameter.DataType);
                param.Value = parameter.Value;
            }
        }

        public DataTable RunQuery(string sql)
        {
            OracleCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;

            FillParameters(cmd);
            
            OracleDataAdapter adapter = new OracleDataAdapter();
            adapter.SelectCommand = cmd;

            DataSet ds = new DataSet();
            adapter.Fill(ds);

            _parameters.Clear();
            return ds.Tables[0];
        }


        public void CreateParameter(string name, DBDataType type, object value)
        {
            DataParameter param = new DataParameter()
            {
                Name = name,
                DataType = DataUtil.GetOracleType(type),
                Value = value
            };

            _parameters.Add(param);
        }
    }
}
