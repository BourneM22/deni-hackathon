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
        private String MySqlConnectionStringBuilder(MySqlModel mySqlModel)
        {
            MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder()
            {
                Server = mySqlModel.Server,
                Database = mySqlModel.Database,
                UserID = mySqlModel.Uid,
                Password = mySqlModel.Password,
                Port = (uint)mySqlModel.Port,
                SslMode = MySqlSslMode.Disabled,
                MinimumPoolSize = (uint)mySqlModel.MinPoolSize,
                MaximumPoolSize = (uint)mySqlModel.MaxPoolSize,
                ConnectionTimeout = (uint)mySqlModel.ConnectionTimeout,
                DefaultCommandTimeout = (uint)mySqlModel.DefaultCommandTimeOut
            };

            return connBuilder.ToString();
        }
        public MySqlConnection GetConnection()
        {
            String connString = MySqlConnectionStringBuilder(_mySql);
            MySqlConnection mySqlConn = new MySqlConnection(connString);
            
            if (mySqlConn.State != System.Data.ConnectionState.Open)
            {
                mySqlConn.Open();
            }

            return mySqlConn;
        }
    }
}