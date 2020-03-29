using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Core.Domain
{
    public class Following: Common
    {
        public Guid FollowerId { get; set; }

        public User Follower { get; set; }

        public Guid FollowedId { get; set; }

        public User Followed { get; set; }

    }
}
