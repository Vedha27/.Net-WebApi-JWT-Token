using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Repositories;

namespace WebApplication2.Service
{
    public class TokenService : ITokenRepository
    {
        private readonly IConfiguration config;

        public TokenService(IConfiguration config) 
        {
            this.config = config;
        }
        public string CreateJwtTokens(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                                             config["Jwt:Audience"],
                                             claims,
                                             expires: DateTime.Now.AddMinutes(15),
                                             signingCredentials: credentials
                                             );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
