using Eventify.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Core.Domain
{
    public class Attendance: Common
    {

        public Guid AttendeeId { get; set; }

        public User Attendee { get; set; }

        public Guid EventId { get; set; }

        public Event Event { get; set; }

    }
}
