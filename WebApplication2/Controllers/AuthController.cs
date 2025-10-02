using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO;
using WebApplication2.Repositories;
using WebApplication2.Service;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenService _token;

        public AuthController(UserManager<IdentityUser> userManager,TokenService token)
        {
            this._userManager = userManager;
            this._token = token;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.UserName,

            };

            var identityResult = await _userManager.CreateAsync(identityUser,registerDto.Password);

            if (identityResult.Succeeded)
            {
                if (registerDto.Roles != null && registerDto.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        
                        return Ok("Register Successfull! Please Login...");
                    }
                }
            }

            return BadRequest("Something Went Wrong");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.UserName);

            if(user!=null)
            {
                var pwdResult = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if(pwdResult)
                {
                    var roles = await  _userManager.GetRolesAsync(user);

                    if (roles != null)
                    {

                        var jwtToken = _token.CreateJwtTokens(user, roles.ToList());

                        var loginSuccesDto = new LoginSuccessDto() { JwtToken = jwtToken };
                        
                        return Ok(loginSuccesDto);
                    }
                }
            }

            return BadRequest("Username or Password is wrong");

        }
    }
}
