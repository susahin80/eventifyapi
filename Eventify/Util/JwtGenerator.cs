using Eventify.Core.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eventify.Util
{

    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }

    public class JwtGenerator: IJwtGenerator
    {
        private readonly SymmetricSecurityKey _key;

        public JwtGenerator(IOptions<AppSettings> appSettings)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JwtSecret));
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
            };

            //generate signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
