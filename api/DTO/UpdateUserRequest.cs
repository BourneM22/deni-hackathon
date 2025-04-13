using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Validator;

namespace api.DTO
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public String Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email format is invalid")]
        public String Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "Gender is required")]
        [GenderValidation(ErrorMessage = "Gender")]
        public Char Gender { get; set; }

        [Required(ErrorMessage = "Birth date is required")]
        [DateLessThanToday(ErrorMessage = "Birth date")]
        public DateOnly BirthDate { get; set; }
        
        public String? Password { get; set; } = String.Empty;
    }
}