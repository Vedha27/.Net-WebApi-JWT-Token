using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Repositories
{
    public interface ITokenRepository 
    {
        public string CreateJwtTokens(IdentityUser user, List<string> roles);
    }
}
