using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Event
{
    public class CreateEventResource
    {
        //todo: validation annotations

        public string Title { get; set; }

        public string Description { get; set; }

        public string PlaceName { get; set; }

        public Guid CategoryId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal? Price { get; set; }

        public int? MaxNumberOfPeople { get; set; }

        public int? MinAgeLimit { get; set; }

        public int? MaxAgeLimit { get; set; }

        public bool IsActive { get; set; }

    }
}
