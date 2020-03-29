using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Follower
{
    public class ReadFollowingResource
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public DateTime FollowedDate { get; set; }
    }
}
