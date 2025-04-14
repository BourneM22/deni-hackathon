using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public abstract class DbConfig
    {
        public String Server { get; set; } = String.Empty;
        public String Uid { get; set; } = String.Empty;
        public int Port { get; set; }
        public String Password { get; set; } = String.Empty;
        public String Database { get; set; } = String.Empty;
    }
    public class MySqlModel : DbConfig
    {
        public int MinPoolSize { get; set; }
        public int MaxPoolSize { get; set; }
        public int ConnectionTimeout { get; set; }
        public int DefaultCommandTimeOut { get; set; }
    }
}