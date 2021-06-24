using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using oauth_service.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace oauth_service.Helpers
{
    public class JwtAuth : IJwtAuth
    {
        protected string ConfigKey;

        public JwtAuth(string configKey)
        {
            ConfigKey = configKey ?? throw new ArgumentNullException(nameof(ConfigKey));
        }

        public TokenResponse GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var createToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(createToken);

            return new TokenResponse
            {
                AccessToken = token,
                ValidFrom = createToken.ValidFrom,
                ValidTo = createToken.ValidTo,
            };
        }
    }

    public interface IJwtAuth
    {
        TokenResponse GenerateToken(User user);
    }
}
