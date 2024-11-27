using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
using XStudio.Common.Helper;
using XStudio.Users;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace XStudio.Managers
{

    /// <summary>
    /// 用户领域服务
    /// </summary>
    public class AccountManager : DomainService, IAccountManager
    {
        private readonly IUserRepository<IdentityUser> _repository;
        private readonly ILocalEventBus _localEventBus;
        private IdentityUserManager _userManager;
        private readonly IConfiguration _configuration;

        public AccountManager(IUserRepository<IdentityUser> repository
            , ILocalEventBus localEventBus
            , IdentityUserManager userManager
            , IConfiguration configuration)
        {
            _repository = repository;
            _localEventBus = localEventBus;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// 根据用户id获取token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="getUserInfo"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<string> GetTokenByUserIdAsync(Guid userId,Action<IdentityUser>? getUserInfo=null)
        {
            //获取用户信息
            var userInfo = await _userManager.FindByIdAsync(userId.ToString());
            if (userInfo == null) {
                throw new UserFriendlyException("用户不存在！");
            }

            ////判断用户状态
            //if (userInfo.User.State == false)
            //{
            //    throw new UserFriendlyException(UserConst.State_Is_State);
            //}

            //if (userInfo.RoleCodes.Count == 0)
            //{
            //    throw new UserFriendlyException(UserConst.No_Role);
            //}
            //if (!userInfo.PermissionCodes.Any())
            //{
            //    throw new UserFriendlyException(UserConst.No_Permission);
            //}

            if (getUserInfo is not null)
            {
                getUserInfo(userInfo);
            }
            
            var accessToken = CreateToken(this.UserInfoToClaim(userInfo));
            //将用户信息添加到缓存中，需要考虑的是更改了用户、角色、菜单等整个体系都需要将缓存进行刷新，看具体业务进行选择
            return accessToken;
        }

        /// <summary>
        /// 创建令牌
        /// </summary>
        /// <param name="kvs"></param>
        /// <returns></returns>
        private string CreateToken(List<KeyValuePair<string, string>> kvs)
        {
            var jwtSection = _configuration.GetSection("Jwt"); // 获取Jwt相关配置
            var secretKey = jwtSection["SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new UserFriendlyException("请配置Jwt:SecretKey");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = kvs.Select(x => new Claim(x.Key, x.Value.ToString())).ToList();
            var token = new JwtSecurityToken(
               issuer: jwtSection["Issuer"],
               audience: jwtSection["Audience"],
               claims: claims,
               expires: DateTime.Now.AddMinutes(jwtSection["ExpiresMinute"]?.To<int>() ?? 15),
               notBefore: DateTime.Now,
               signingCredentials: creds);
            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);

            return returnToken;
        }

        public string CreateRefreshToken(Guid userId) {
            var jwtSection = _configuration.GetSection("RefreshJwt"); // 获取Jwt相关配置
            var secretKey = jwtSection["SecretKey"];
            if (string.IsNullOrEmpty(secretKey)) {
                throw new UserFriendlyException("请配置Refresh Jwt:SecretKey");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //添加用户id，及刷新token的标识
            var claims = new List<Claim> {
                new Claim(AbpClaimTypes.UserId,userId.ToString()),
                new Claim(TokenTypeConst.Refresh, "true")
            };
            var token = new JwtSecurityToken(
               issuer: jwtSection["Issuer"],
               audience: jwtSection["Audience"],
               claims: claims,
               expires: DateTime.Now.AddMinutes(jwtSection["ExpiresMinute"]?.To<int>() ?? 15),
               notBefore: DateTime.Now,
               signingCredentials: creds);
            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);

            return returnToken;

        }
        /// <summary>
        /// 登录校验
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="userAction"></param>
        /// <returns></returns>
        public async Task LoginValidationAsync(string userName, string password, Action<IdentityUser>? userAction = null)
        {
            //var user = new UserAggregateRoot();
            //if (await ExistAsync(userName, o => user = o))
            //{
            //    if (userAction is not null)
            //    {
            //        userAction.Invoke(user);
            //    }
            //    if (user.EncryPassword.Password == MD5Helper.SHA2Encode(password, user.EncryPassword.Salt))
            //    {
            //        return;
            //    }
            //    throw new UserFriendlyException(UserConst.Login_Error);
            //}
            //throw new UserFriendlyException(UserConst.Login_User_No_Exist);
        }

        /// <summary>
        /// 判断账户合法存在
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userAction"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(string userName, Action<IdentityUser> userAction = null)
        {
            var user = await _repository.FindByUserNameAsync(userName);
            if (userAction is not null)
            {
                userAction.Invoke(user);
            }
            //这里为了兼容解决数据库开启了大小写不敏感问题,还要将用户名进行二次校验
            if (user != null && user.UserName == userName)
            {
                return true;
            }
            return false;
        }
        
        

        /// <summary>
        /// 令牌转换
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public List<KeyValuePair<string, string>> UserInfoToClaim(IdentityUser dto)
        {
            var claims = new List<KeyValuePair<string, string>>();
            AddToClaim(claims, AbpClaimTypes.UserId, dto.Id.ToString());
            AddToClaim(claims, AbpClaimTypes.UserName, dto.UserName ?? dto.Email);
            //if (dto.DeptId is not null)
            //{
            //    AddToClaim(claims, TokenTypeConst.DeptId, dto.DeptId.ToString());
            //}
            if (dto.Email is not null)
            {
                AddToClaim(claims, AbpClaimTypes.Email, dto.Email);
            }
            if (dto.PhoneNumber is not null)
            {
                AddToClaim(claims, AbpClaimTypes.PhoneNumber, dto.PhoneNumber.ToString());
            }
            if (dto.Roles.Count > 0)
            {
                AddToClaim(claims, TokenTypeConst.RoleInfo, JsonConvert.SerializeObject(dto.Roles.Select(x => new { Id = x.RoleId, DataScope = x.ToJson() })));
            }
            //if (UserConst.Admin.Equals(dto.UserName))
            //{
            //    AddToClaim(claims, TokenTypeConst.Permission, UserConst.AdminPermissionCode);
            //    AddToClaim(claims, TokenTypeConst.Roles, UserConst.AdminRolesCode);
            //}
            //else
            //{
            //    dto.PermissionCodes?.ForEach(per => AddToClaim(claims, TokenTypeConst.Permission, per));
            //    dto.RoleCodes?.ForEach(role => AddToClaim(claims, AbpClaimTypes.Role, role));
            //}

            return claims;
        }


        private void AddToClaim(List<KeyValuePair<string, string>> claims, string key, string value)
        {
            claims.Add(new KeyValuePair<string, string>(key, value));
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task UpdatePasswordAsync(Guid userId, string newPassword, string oldPassword)
        {
            //var user = await _repository.GetAsync(userId);
            //if (!user.JudgePassword(oldPassword))
            //{
            //    throw new UserFriendlyException("无效更新！原密码错误！");
            //}
            //user.EncryPassword.Password = newPassword;
            //user.BuildPassword();
            //await _repository.UpdateAsync(user);
        }

        /// <summary>
        /// 重置密码,也可以是找回密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> RestPasswordAsync(Guid userId, string password)
        {
            //var user = await _repository.GetAsync(userId);
            //user.EncryPassword.Password = password;
            //user.BuildPassword();
            //return await _repository.UpdateAsync(user);
            return false;
        }

        /// <summary>
        /// 注册用户，创建用户之后设置默认角色
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task RegisterAsync(string userName, string password, long phone,string? nick)
        {
            //var user = new UserAggregateRoot(userName, password, phone,nick);
            //await _userManager.CreateAsync(user);
            //await _userManager.SetDefautRoleAsync(user.Id);
        }
    }

}
