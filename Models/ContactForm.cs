using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartParking.Models
{
    public class ContactForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide your name")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Your name should be min 2 and max 60 length")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Please provide email address")]
        [StringLength(50, ErrorMessage = "Must be between less than 50 characters")]
        [EmailAddress]
        public string SenderMail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide email subject")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Subject should be min 2 and max 30 length")]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill message box")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Email should be min 1 and max 1000 length")]
        public string Message { get; set; }
    }
}