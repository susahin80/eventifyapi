using Eventify.Controllers.Resources.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Attendance
{
    public class ReadAttendanceResource
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public DateTime JoinedDate { get; set; }
    }
}
