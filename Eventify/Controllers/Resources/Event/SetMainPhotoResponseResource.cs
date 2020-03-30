using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Event
{
    public class SetMainPhotoResponseResource
    {
        public Guid Id { get; set; }

        public string PublicId { get; set; }

        public string Url { get; set; }

        public bool IsMain { get; set; }
    }
}
