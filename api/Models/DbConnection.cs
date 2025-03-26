using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace api.Models
{
    public class DbConnection
    {
        private MySqlModel _mySql;
        public DbConnection(IOptions<MySqlModel> mySql)
        {
            _mySql = mySql.Value;
        }
        private String ConnectionStringBuilder(DbConfig dbConfig)
        {
            MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder()
            {
                Server = dbConfig.Server,
                Database = dbConfig.Database,
                UserID = dbConfig.Uid,
                Password = dbConfig.Password,
                Port = (uint)dbConfig.Port,
                SslMode = MySqlSslMode.Disabled,
            };

            return connBuilder.ToString();
        }
        public MySqlConnection GetConnection()
        {
            String connString = ConnectionStringBuilder(_mySql);
            MySqlConnection mySqlConn = new MySqlConnection(connString);
            
            if (mySqlConn.State != System.Data.ConnectionState.Open)
            {
                mySqlConn.Open();
            }

            return mySqlConn;
        }
    }
}