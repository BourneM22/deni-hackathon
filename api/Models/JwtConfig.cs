using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class JwtConfig
    {
        public String Issuer { get; set; } = String.Empty;
        public String Audience { get; set; } = String.Empty;
        public String Key { get; set; } = String.Empty;
        public long TokenValidityMins { get; set; }
    }
}