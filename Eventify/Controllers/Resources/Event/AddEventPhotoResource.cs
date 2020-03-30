using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Event
{
    public class AddEventPhotoResource
    {
        public Guid EventId { get; set; }

        public IFormFile File { get; set; }

    }
}
