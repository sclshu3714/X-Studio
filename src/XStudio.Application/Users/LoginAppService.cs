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
using XStudio.Tenants;
using Volo.Abp;
using Volo.Abp.EventBus.Local;
using XStudio.Managers;

namespace XStudio.Users {
    //[Route("api/xstudio/v{version:apiVersion}/[controller]")]
    //[ApiVersion(1.0)]
    //[ApiController]

    [RemoteService(false)]
    public class LoginAppService : ApplicationService, ILoginAppService {
        private readonly IdentityUserManager _userManager;
        private readonly SignInManager<Volo.Abp.Identity.IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginAppService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpApiHelper _httpApiHelper;
        //private readonly IAccountManager _accountManager;
        public LoginAppService(IdentityUserManager userManager,
                               SignInManager<Volo.Abp.Identity.IdentityUser> signInManager,
                               IConfiguration configuration,
                               IHttpContextAccessor httpContextAccessor,
                               HttpApiHelper httpApiHelper) {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = NullLogger<LoginAppService>.Instance;
            _httpContextAccessor = httpContextAccessor;
            _httpApiHelper = httpApiHelper;
            //_accountManager = accountManager;
        }

        //[HttpPost]
        public async Task<ActionResult<IdentityUserDto>> Login(LoginDto loginDto) {
            var user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmailAddress);
            if (user == null) {
                return new OkObjectResult("Invalid username.");
            }

            if (await _userManager.IsLockedOutAsync(user)) {
                return new OkObjectResult("Account is locked. Please try again later.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user)) {
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
                ClientId = loginDto.ClientId,
                Scope = loginDto.Scope,
                UserName = user.UserName ?? user.Name,
                Password = loginDto.Password,
                GrantType = "password",
            };
            await _httpApiHelper.ChangeBaseAddress(requestUrl); // 设置请求地址
            TokenRes? tokenRes = await _httpApiHelper.TokenAsync("connect/token", request);
            if (tokenRes == null) {
                throw new InvalidOperationException("Failed to get token.");
            }
            IdentityUserDto identityUserDto = ObjectMapper.Map<Volo.Abp.Identity.IdentityUser, IdentityUserDto>(user);
            identityUserDto.ExtraProperties.Add("AccessToken", tokenRes.AccessToken);
            identityUserDto.ExtraProperties.Add("TokenType", tokenRes.TokenType);
            identityUserDto.ExtraProperties.Add("ExpiresIn", tokenRes.ExpiresIn);
            identityUserDto.ExtraProperties.Add("RefreshToken", tokenRes.RefreshToken);
            return new OkObjectResult(identityUserDto);
        }

        /// <summary>
        /// 获取当前请求的Host
        /// </summary>
        /// <returns></returns>
        private string GetRequestUrl() {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request != null) {
                // 获取请求地址
                // return $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
                return $"{request.Scheme}://{request.Host}";
            }
            return string.Empty;
        }

        private string GenerateRefreshToken() {
            // 生成随机的 Refresh Token
            return Guid.NewGuid().ToString();
        }

        private string GenerateAccessToken(IdentityUserDto user) {
            if (_configuration["Jwt:Key"] is string jwtKey && !string.IsNullOrWhiteSpace(jwtKey)) {
                var claims = new List<Claim>() {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return "";
        }


        private string GenerateJwtToken(Volo.Abp.Identity.IdentityUser user) {
            if (_configuration["Jwt:Key"] is string jwtKey && !string.IsNullOrWhiteSpace(jwtKey)) {
                try {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(jwtKey);
                    Array.Resize(ref key, 32);
                    var tokenDescriptor = new SecurityTokenDescriptor {
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
                catch (Exception ex) {
                    throw new InvalidOperationException($"Failed to generate JWT token.", ex);
                }
            }
            return "";
        }

        #region jwt生成token
        public async Task<IdentityUserDto> LoginV2(LoginDto loginDto) {
            if (string.IsNullOrEmpty(loginDto.Password) || string.IsNullOrEmpty(loginDto.UserNameOrEmailAddress)) {
                throw new UserFriendlyException("请输入合理数据！");
            }
            return  null;
            //////校验验证码
            ////ValidationImageCaptcha(input.Uuid, input.Code);

            //Volo.Abp.Identity.IdentityUser? user = null;
            ////校验
            //await _accountManager.LoginValidationAsync(loginDto.UserNameOrEmailAddress, loginDto.Password, x => user = x);

            //if (user == null) { 
            //    throw new UserFriendlyException("用户名或密码错误！");
            //}

            //Volo.Abp.Identity.IdentityUser? userInfo = null;
            ////获取token
            //var accessToken = await _accountManager.GetTokenByUserIdAsync(user.Id, (info) => userInfo = info);
            //var refreshToken = _accountManager.CreateRefreshToken(user.Id);

            ////这里抛出一个登录的事件,也可以在全部流程走完，在应用层组装
            //if (_httpContextAccessor.HttpContext is not null) {
            //    var loginEntity = new LoginLogAggregateRoot().GetInfoByHttpContext(_httpContextAccessor.HttpContext);
            //    var loginEto = loginEntity.Adapt<LoginEventArgs>();
            //    loginEto.UserName = userInfo.User.UserName;
            //    loginEto.UserId = userInfo.User.Id;
            //    await LocalEventBus.PublishAsync(loginEto);
            //}

            //IdentityUserDto identityUserDto = ObjectMapper.Map<Volo.Abp.Identity.IdentityUser, IdentityUserDto>(user);
            //identityUserDto.ExtraProperties.Add("AccessToken", accessToken);
            ////identityUserDto.ExtraProperties.Add("TokenType", tokenRes.TokenType);
            ////identityUserDto.ExtraProperties.Add("ExpiresIn", tokenRes.ExpiresIn);
            //identityUserDto.ExtraProperties.Add("RefreshToken", refreshToken);
            //return identityUserDto;
        }
        #endregion
    }
}
