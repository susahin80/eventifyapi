using Eventify.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Validation
{
    public class BirthDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime birthDate = (DateTime)value;

            int age = DateUtil.CalculateAge(birthDate);

            // your validation logic
            if (age >= 18)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("You must be at least 18 years old");
            }
        }

    }
}
