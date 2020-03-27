using AutoMapper;
using Eventify.Controllers.Resources;
using Eventify.Core;
using Eventify.Core.Domain;
using Eventify.Core.Repositories;
using Eventify.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
    }
}

//PasswordHasher<User> hasher = new PasswordHasher<User>(
//    new OptionsWrapper<PasswordHasherOptions>(
//        new PasswordHasherOptions()
//        {
//            CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3
//        })
//);