using Eventify.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Util
{
    public static class Hasher<TUser> where TUser : class
    {

        public  static string Hash(TUser user, string password)
        {

            PasswordHasher<TUser> hasher = new PasswordHasher<TUser>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3
                    })
            );
            return hasher.HashPassword(user, password);
        }

        public static bool Verify(TUser user, string hashedPassword, string password)
        {

            PasswordHasher<TUser> hasher = new PasswordHasher<TUser>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3
                    })
            );

            return hasher.VerifyHashedPassword(user, hashedPassword, password) == PasswordVerificationResult.Success;
        }


    }
}
