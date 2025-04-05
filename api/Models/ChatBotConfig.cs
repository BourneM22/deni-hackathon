using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class ChatBotConfig
    {
        public String Url { get; set; } = String.Empty;
        public int Port { get; set; }
    }
}