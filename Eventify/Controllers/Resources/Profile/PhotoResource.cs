using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Profile
{
    public class PhotoResource
    {
        public string PublicId { get; set; }

        public string Url { get; set; }

        public bool IsMain { get; set; }
    }
}
