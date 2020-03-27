using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Event
{
    public class FilterEventResource
    {
        public string SortBy { get; set; }

        public bool IsSortAscending { get; set; }

        public int Page { get; set; }

        public byte PageSize { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsFree { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? HostId { get; set; }

    }
}
