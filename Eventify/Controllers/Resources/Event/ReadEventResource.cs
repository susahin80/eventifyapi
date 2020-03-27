using Eventify.Controllers.Resources.Attendance;
using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Event
{
    public class ReadEventResource
    {
        public Guid Id { get; set; }

        public string Host { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PlaceName { get; set; }

        public string Category { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal? Price { get; set; }

        public int? MaxNumberOfPeople { get; set; }

        public int? MinAgeLimit { get; set; }

        public int? MaxAgeLimit { get; set; }

        public bool IsActive { get; set; }

      //  [JsonPropertyName("attendees")]
        public ICollection<ReadAttendanceResource> Attendees { get; set; }


    }
}
