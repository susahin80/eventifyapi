using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Eventify.Controllers.Resources.Follower;
using Eventify.Core.Domain;
using Eventify.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.Controllers
{

    [Authorize]
    public class FollowerController : BaseController
    {
        [HttpPost("{userId}")]
        public async Task<ActionResult> Follow(Guid userId)
        {

            User followedUser = await UnitOfWork.Users.GetWithRelated(userId, u => u.Followers);

            if (followedUser == null) throw new RestError(HttpStatusCode.NotFound, new { Event = "User doesn't exist." });

            Guid followerUserId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (followedUser.Followers.Any(f => f.FollowerId == followerUserId)) throw new RestError(HttpStatusCode.BadRequest, new { User = "You are already following this user." });

            // why doesn't this work?
            //followedUser.Followers.Add(new Following()
            //{
            //    CreatedAt = DateTime.Now,
            //    FollowerId = followerUserId,
            //    FollowedId = userId
            //});

            var following = new Following()
            {
                CreatedAt = DateTime.Now,
                FollowerId = followerUserId,
                FollowedId = userId
            };

            UnitOfWork.Followers.Add(following);

            await UnitOfWork.CompleteAsync();

            return NoContent();
        }


        [HttpDelete("{userId}")]
        public async Task<ActionResult> Unfollow(Guid userId)
        {

            User followedUser = await UnitOfWork.Users.Get(userId);

            if (followedUser == null) throw new RestError(HttpStatusCode.NotFound, new { Event = "User doesn't exist." });

            Guid followerUserId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var following = await UnitOfWork.Followers.SingleOrDefault(f => f.FollowedId == userId && f.FollowerId == followerUserId);

            if (following == null) throw new RestError(HttpStatusCode.BadRequest, new { User = "You are not following this user." });

            UnitOfWork.Followers.Remove(following);

            await UnitOfWork.CompleteAsync();

            return NoContent();
        }


        [HttpGet("{id}/followers")]
        public async Task<ActionResult<User>> GetFollowers(Guid id)
        {

            IEnumerable<Following> followers = await UnitOfWork.Followers.FindWithRelated(f => f.FollowedId == id, null, false, f => f.Follower);

            var result = Mapper.Map<IEnumerable<Following>, IEnumerable<ReadFollowerResource>>(followers);

            return Ok(result);
        }

        [HttpGet("{id}/followings")]
        public async Task<ActionResult<User>> GetFollowings(Guid id)
        {

            IEnumerable<Following> followers = await UnitOfWork.Followers.FindWithRelated(f => f.FollowerId == id, null, false, f => f.Followed);

            var result = Mapper.Map<IEnumerable<Following>, IEnumerable<ReadFollowingResource>>(followers);

            return Ok(result);
        }
    }
}