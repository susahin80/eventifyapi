using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Core.Domain
{
    public class Photo: Common 
    {

        public string PublicId { get; set; }

        public string Url { get; set; }

        public bool IsMain { get; set; }
    }
}
