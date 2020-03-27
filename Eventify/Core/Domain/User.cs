using Eventify.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Core.Domain
{
    public class User: Common
    {

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Gender { get; set; }

        public string Role { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsVerified { get; set; }

        public DateTime? VerifiedDate { get; set; }

        public ICollection<Event> Events { get; set; }

        public ICollection<Attendance> Attendances { get; set; }

    }
}
