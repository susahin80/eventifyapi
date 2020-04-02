using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Eventify.Controllers.Resources.Profile;
using Eventify.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.Controllers
{

    public class ProfileController : BaseController
    {

        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<ProfileResource>> Get(string username)
        {
            Guid currentUserId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var currentUser =  await UnitOfWork.Users.GetWithRelated(currentUserId,  u => u.Followings);

            var user = await UnitOfWork.Users.SingleOrDefaultWithRelated(u => u.Username == username, u => u.Followers, u => u.Followings, u => u.Photos);

          //  var user = await UnitOfWork.Users.GetUser(username);

            var profile = new ProfileResource
            {
                FollowersCount = user.Followers.Count,
                FollowingCount = user.Followings.Count,
                Username = user.Username,
                Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Photos = Mapper.Map<ICollection<Photo>, ICollection<PhotoResource>>(user.Photos),
                IsFollowed = currentUser.Followings.Any(x => x.FollowedId == user.Id)
            };

            return Ok(profile);
        }
    }
}