using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Event
{
    public class AddUserPhotoResponseResource
    {
        public string PublicId { get; set; }

        public string Url { get; set; }

        public bool IsMain { get; set; }

        public Guid UserId { get; set; }
    }
}
