using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class LoginRequest
    {
        [Required( ErrorMessage = "Email is required" )]
        public String Email { get; set; } = String.Empty;

        [Required( ErrorMessage = "Password is required" )]
        public String Password { get; set; } = String.Empty;
    }
}