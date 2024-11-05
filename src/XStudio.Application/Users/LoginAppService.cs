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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Http;
using XStudio.Models;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using XStudio.Models.Requests;
using XStudio.Models.Responses;

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
        private readonly ILogger<LoginAppService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpApiHelper _httpApiHelper;

        public LoginAppService(IdentityUserManager userManager,
                               SignInManager<Volo.Abp.Identity.IdentityUser> signInManager,
                               IConfiguration configuration,
                               IHttpContextAccessor httpContextAccessor,
                               HttpApiHelper httpApiHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = NullLogger<LoginAppService>.Instance;
            _httpContextAccessor = httpContextAccessor;
            _httpApiHelper = httpApiHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmailAddress);
            if (user == null)
            {
                return new OkObjectResult("Invalid username.");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new OkObjectResult("Account is locked. Please try again later.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user))
                {
                    return new OkObjectResult("Account is locked due to multiple failed login attempts. Please try again later.");
                }

                return new OkObjectResult("Invalid username or password.");
            }

            var identityResult = await _userManager.ResetAccessFailedCountAsync(user);

            var requestUrl = GetRequestUrl(); // 获取完整请求地址
            if (string.IsNullOrEmpty(requestUrl)) { 
                return new OkObjectResult("Invalid request url.");
            }
            // 获取token
            var request = new TokenReq {
                ClientId = "XStudio_App",
                Scope = "XStudio",
                UserName = user.UserName ?? user.Name,
                Password = loginDto.Password,
                GrantType = "password",
            };
            _httpApiHelper.SetBaseAddress(requestUrl); // 设置请求地址
            TokenRes? tokenRes = await _httpApiHelper.TokenAsync("connect/token", request);
            if (tokenRes == null) { 
                throw new InvalidOperationException("Failed to get token.");
            }
            user.ExtraProperties["AccessToken"] = tokenRes.AccessToken;
            user.ExtraProperties["TokenType"] = tokenRes.TokenType;
            user.ExtraProperties["ExpiresIn"] = tokenRes.ExpiresIn;
            user.ExtraProperties["RefreshToken"] = tokenRes.RefreshToken;
            return new OkObjectResult(user);
        }

        public string GetRequestUrl() {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request != null) {
                // 获取请求地址
                // return $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
                return $"{request.Scheme}://{request.Host}";
            }
            return string.Empty;
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
                    throw new InvalidOperationException($"Failed to generate JWT token.", ex);
                }
            }
            return "";
        }
    }
}
