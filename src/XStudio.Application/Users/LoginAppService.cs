using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace XStudio.Users
{
    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class LoginAppService : ApplicationService
    {
        private readonly IdentityUserManager _userManager;
        private readonly SignInManager<Volo.Abp.Identity.IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginAppService(IdentityUserManager userManager,
                               SignInManager<Volo.Abp.Identity.IdentityUser> signInManager,
                               IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new OkObjectResult("Invalid username or password.");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new OkObjectResult("Account is locked. Please try again later.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
            {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user))
                {
                    return new OkObjectResult("Account is locked due to multiple failed login attempts. Please try again later.");
                }

                return new OkObjectResult("Invalid username or password.");
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            var token = GenerateJwtToken(user);
            return new OkObjectResult(new { Token = token });
        }

        private string GenerateJwtToken(Volo.Abp.Identity.IdentityUser user)
        {
            if (_configuration["Jwt:Key"] is string jwtKey && !string.IsNullOrWhiteSpace(jwtKey))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(jwtKey);
                    Array.Resize(ref key, 32);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName)
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return tokenHandler.WriteToken(token);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "";
        }
    }
}
