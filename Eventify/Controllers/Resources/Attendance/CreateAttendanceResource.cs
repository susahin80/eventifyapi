using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Attendance
{
    public class CreateAttendanceResource
    {

        [Required(ErrorMessage = "Event is required")]
        public Guid EventId { get; set; }

    }
}
