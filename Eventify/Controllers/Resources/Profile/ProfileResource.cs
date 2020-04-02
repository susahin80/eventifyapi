using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Profile
{
    public class ProfileResource
    {

        public string Username { get; set; }

        public string Image { get; set; }

        public ICollection<PhotoResource> Photos { get; set; }

        /// <summary>
        /// when viewing a profile, this will give if we are following this user or not
        /// </summary>
        public bool IsFollowed { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

    }
}
