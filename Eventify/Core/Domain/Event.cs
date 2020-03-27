using Eventify.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Core.Domain
{
    public class Event: Common
    {

        public Guid HostId { get; set; }

        public User Host { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PlaceName { get; set; }


        //[Column(TypeName = "decimal(9,6)")]
        //public decimal PlaceLatitude { get; set; }

        //[Column(TypeName = "decimal(9,6)")]
        //public decimal PlaceLongitude { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        [Column(TypeName = "decimal(4,0)")]
        public decimal? Price { get; set; }

        public int? MaxNumberOfPeople { get; set; }

        public int? MinAgeLimit { get; set; }

        public int? MaxAgeLimit { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Attendance> Attendances { get; set; }


    }
}
