using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Util
{
    public class DateUtil
    {
        public static int CalculateAge(DateTime birthDate)
        {
            // Calculate the age.
            var age = DateTime.Today.Year - birthDate.Year;

            // Go back to the year the person was born in case of a leap year
            if (birthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
