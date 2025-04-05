using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class ChatBotRequest
    {
        public String Prompt { get; set; } = String.Empty;
    }
}