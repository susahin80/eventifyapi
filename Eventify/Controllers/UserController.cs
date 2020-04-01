using Eventify.Controllers.Resources;
using Eventify.Controllers.Resources.Event;
using Eventify.Core;
using Eventify.Core.Domain;
using Eventify.Infrastructure.Photos;
using Eventify.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Eventify.Controllers.Resources.Attendance;

namespace Eventify.Controllers
{

    public class UserController : BaseController
    {

        private readonly IJwtGenerator jwtGenerator;

        public UserController(IJwtGenerator jwtGenerator)
        {
            this.jwtGenerator = jwtGenerator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Register(CreateUserResource userResource)
        {
            if (await UnitOfWork.Users.SingleOrDefault(u => u.Username == userResource.Username) != null)
            {
                throw new RestError(HttpStatusCode.BadRequest, new { Username = "Username already exists" });
            }

            var user = await UnitOfWork.Users.SingleOrDefault(u => u.Email == userResource.Email);

            if (user != null) throw new RestError(HttpStatusCode.BadRequest, new { Email = "Email already exists" });

            user = Mapper.Map<CreateUserResource, User>(userResource);
            user.Password = Hasher<User>.Hash(user, user.Password);
            user.CreatedAt = DateTime.Now;

            UnitOfWork.Users.Add(user);
            await UnitOfWork.CompleteAsync();

            var createdUser = await UnitOfWork.Users.Get(user.Id);

            var response = Mapper.Map<User, ReadUserResource>(createdUser);

            return Ok(response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Login(LoginUserResource loginInfo)
        {

            var user = await UnitOfWork.Users.SingleOrDefault(u => u.Email == loginInfo.Email);

            if (user == null) throw new RestError(HttpStatusCode.BadRequest, new { Email = "Email or password is wrong" });

            if (!Hasher<User>.Verify(user, user.Password, loginInfo.Password))
            {
                throw new RestError(HttpStatusCode.BadRequest, new { Email = "Email or password is wrong" });
            }

            var response = Mapper.Map<User, ReadUserResource>(user);

            response.Token = jwtGenerator.CreateToken(user);

            return Ok(response);
        }

        [HttpPost("photo")]
        [Authorize]
        public async Task<ActionResult<AddUserPhotoResponseResource>> AddUserPhoto([FromForm]AddUserPhotoResource resource)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var user = await UnitOfWork.Users.GetWithRelated(userId, e => e.Photos);

            if (user == null) throw new RestError(HttpStatusCode.NotFound, new { Event = "User not found" });

            PhotoUploadResult result = PhotoAccessor.AddPhoto(resource.File);

            bool isMain = !user.Photos.Any(p => p.IsMain == true);

            Photo photo = new Photo()
            {
                CreatedAt = DateTime.Now,
                IsMain = isMain,
                PublicId = result.PublicId,
                Url = result.Url
            };

            user.Photos.Add(photo);

            await UnitOfWork.CompleteAsync();

            var response = new AddUserPhotoResponseResource()
            {
                IsMain = isMain,
                PublicId = photo.PublicId,
                Url = photo.Url,
                UserId = userId
            };

            return Ok(response);
        }


        [HttpPut("photo/{photoId}")]
        [Authorize]
        public async Task<ActionResult<SetMainPhotoResponseResource>> SetUserMainPhoto(Guid photoId)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var user = await UnitOfWork.Users.GetWithRelated(userId, e => e.Photos);

            if (user == null) throw new RestError(HttpStatusCode.NotFound, new { Event = "User not found" });

            Photo photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null) throw new RestError(HttpStatusCode.NotFound, new { Photo = "Not found" });

            if (photo.IsMain) throw new RestError(HttpStatusCode.BadRequest, new { Photo = "Photo is already the main photo." });

            photo.IsMain = true;

            Photo currentMainPhoto = user.Photos.FirstOrDefault(p => p.IsMain == true);

            if (currentMainPhoto != null)
            {
                currentMainPhoto.IsMain = false;
            }

            await UnitOfWork.CompleteAsync();

            var response = Mapper.Map<Photo, SetMainPhotoResponseResource>(photo);

            return Ok(response);
        }


        [HttpDelete("photo/{photoId}")]
        [Authorize]
        public async Task<ActionResult> DeleteEventPhoto(Guid photoId)
        {

            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var user = await UnitOfWork.Users.GetWithRelated(userId, e => e.Photos);

            if (user == null) throw new RestError(HttpStatusCode.NotFound, new { Event = "User not found" });

            Photo photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null) throw new RestError(HttpStatusCode.NotFound, new { Photo = "Photo not found" });

            if (photo.IsMain) throw new RestError(HttpStatusCode.BadRequest, new { Photo = "You can't delete your main photo" });

            string result = PhotoAccessor.DeletePhoto(photo.PublicId);

            if (result == "ok")
            {
                // evt.Photos.Remove(photo); //only removes relation

                UnitOfWork.Photos.Remove(photo);

                await UnitOfWork.CompleteAsync();
            }
            else
            {
                throw new Exception(result);
            }

            return NoContent();
        }

        [HttpGet("events")]
        [Authorize]
        public async Task<ActionResult<QueryResultResource<ReadEventResource>>> GetUserOwnEvents([FromQuery]FilterEventResource filter)
        {

            var eventQuery = Mapper.Map<FilterEventResource, EventQuery>(filter);

            if (string.IsNullOrEmpty(eventQuery.SortBy))
            {
                Expression<Func<Event, DateTime>> sortingExpression = a => a.StartDate;
                eventQuery.SortBy = PropertyUtil.GetFullPropertyName(sortingExpression);
            }

            Guid userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            eventQuery.HostId = userId;

            QueryResult<Event> events = await UnitOfWork.Events.GetEvents(eventQuery,e => e.Category, e => e.Host);

            var result = Mapper.Map<QueryResult<Event>, QueryResultResource<ReadEventResource>>(events);

            return Ok(result);

        }

        [HttpGet("attending-events")]
        [Authorize]
        public async Task<ActionResult<QueryResultResource<ReadEventResource>>> GetUserAttendingEvents([FromQuery]FilterEventResource filter)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var eventQuery = Mapper.Map<FilterEventResource, EventQuery>(filter);      

            if (string.IsNullOrEmpty(eventQuery.SortBy))
            {
                Expression<Func<Attendance, DateTime>> sortingExpression = a => a.Event.StartDate;
                eventQuery.SortBy = PropertyUtil.GetFullPropertyName(sortingExpression);
            }

            var attendances = await UnitOfWork.Attendances.GetUserAttendances(userId, eventQuery);

            var result = Mapper.Map<QueryResult<Attendance>, QueryResultResource<ReadAttendanceEventResource>>(attendances);

            return Ok(result);
        }

        private static string GetMemberName(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member.Name;
                case ExpressionType.Convert:
                    return GetMemberName(((UnaryExpression)expression).Operand);
                default:
                    throw new NotSupportedException(expression.NodeType.ToString());
            }
        }
    }
}

//PasswordHasher<User> hasher = new PasswordHasher<User>(
//    new OptionsWrapper<PasswordHasherOptions>(
//        new PasswordHasherOptions()
//        {
//            CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3
//        })
//);