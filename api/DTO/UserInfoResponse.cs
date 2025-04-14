using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class UserInfoResponse
    {
        public String Name { get; set; } = String.Empty;
        public Char Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        public String Email { get; set; } = String.Empty;
    }
}