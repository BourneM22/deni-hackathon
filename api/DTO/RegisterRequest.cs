using System.ComponentModel.DataAnnotations;
using api.Validator;

namespace api.DTO
{
    public class RegisterRequest
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
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(maximumLength: 20, MinimumLength = 8, ErrorMessage = "Password minimum length is 8 and maximum length is 20")]
        public String Password { get; set; } = String.Empty;
    }
}