using Eventify.Controllers.Resources.Attendance;
using Eventify.Controllers.Resources.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Event
{
    public class ReadEventAttendancesResource
    {

            public Guid Id { get; set; }

            public ReadMinUserResource Host { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public string PlaceName { get; set; }

            public CategoryReadResource Category { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            public DateTime CreatedAt { get; set; }

            public decimal? Price { get; set; }

            public int? MaxNumberOfPeople { get; set; }

            public int? MinAgeLimit { get; set; }

            public int? MaxAgeLimit { get; set; }

            public bool IsActive { get; set; }

            public ICollection<ReadAttendanceResource> Attendees { get; set; }

    }
}
