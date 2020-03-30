using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Event
{
    public class AddUserPhotoResource
    {
        public IFormFile File { get; set; }
    }
}
