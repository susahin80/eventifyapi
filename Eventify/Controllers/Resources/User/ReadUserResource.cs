using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources
{
    public class ReadUserResource
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

    }
}
