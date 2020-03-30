using AutoMapper;
using Eventify.Controllers.Resources;
using Eventify.Controllers.Resources.Event;
using Eventify.Core;
using Eventify.Core.Domain;
using Eventify.Core.Repositories;
using Eventify.Infrastructure.Photos;
using Eventify.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public async Task<ActionResult<AddUserPhotoResponseResource>> AddEventPhoto([FromForm]AddUserPhotoResource resource)
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
        public async Task<ActionResult<SetMainPhotoResponseResource>> SetUserMainPhoto( Guid photoId)
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
    }
}

//PasswordHasher<User> hasher = new PasswordHasher<User>(
//    new OptionsWrapper<PasswordHasherOptions>(
//        new PasswordHasherOptions()
//        {
//            CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3
//        })
//);