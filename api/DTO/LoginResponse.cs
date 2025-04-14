using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class LoginResponse
    {
        public String TokenType { get; set; } = String.Empty;
        public String AccessToken { get; set; } = String.Empty;
        public long ExpiresInMinutes { get; set; }
    }
}