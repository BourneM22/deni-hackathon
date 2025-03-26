using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class User
    {
        public String UserId { get; set; } = String.Empty;
        public String Name { get; set; } = String.Empty;
        public Char Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        public String Email { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
        public String ProfilePicturePath { get; set; } = String.Empty;
        public DateTime CreatedDateTime { get; set; }
    }
}