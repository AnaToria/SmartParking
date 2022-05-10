using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Models
{
    public class SignUpForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide first name")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First Name Should be min 2 and max 30 length")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide last name")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Last name Should be min 2 and max 30 length")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please provide phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please provide email address")]
        [StringLength(50, ErrorMessage = "Must be between less than 50 characters")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Must be between 6 and 30 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(30, ErrorMessage = "Must be between 6 and 30 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
