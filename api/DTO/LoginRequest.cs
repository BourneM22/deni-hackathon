using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class LoginRequest
    {
        public String Email { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
    }
}