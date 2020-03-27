using Eventify.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources
{
    public class CreateUserResource
    {

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between {2} and {1} characters")]
        public string Username { get; set; }


        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage = "Not a valid email addres")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "DisplayName must be between {2} and {1} characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Password must be between {2} and {1} characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringRange(AllowableValues = new[] { "M", "F" }, ErrorMessage = "Gender must be either 'M' or 'F'.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "BirthDate is required")]
        [BirthDateValidation]
        public DateTime BirthDate { get; set; }

    }

}


