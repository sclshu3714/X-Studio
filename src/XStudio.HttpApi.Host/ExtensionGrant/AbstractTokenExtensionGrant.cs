using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict.Controllers;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.OpenIddict;
using Microsoft.Extensions.DependencyInjection;
using XStudio.Common;


namespace XStudio.ExtensionGrant
{
    public abstract class AbstractTokenExtensionGrant 
        : AbpOpenIdDictControllerBase, ITokenExtensionGrant
    {
        protected ExtensionGrantContext? GrantContext { get; set; }
        protected string ProviderKey { get; set; } = string.Empty;
        protected IOptions<IdentityOptions> IdentityOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<IdentityOptions>>();
        protected IdentitySecurityLogManager IdentitySecurityLogManager => LazyServiceProvider.LazyGetRequiredService<IdentitySecurityLogManager>();

        public abstract string Name { get; }
        /// <summary>
        /// 服务器验证
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<ClaimsPrincipal?> ServerValidate()
        {
            var user = await UserManager.FindByLoginAsync(Name, ProviderKey);
            if (user == null)
            {
                return null;
            }

            var principal = await SignInManager.CreateUserPrincipalAsync(user);
            if (GrantContext != null)
            {
                var scopes = GrantContext.Request.GetScopes();
                if (scopes != null)
                {
                    principal.SetScopes(scopes);
                    var resources = await GetResourcesAsync(scopes);
                    principal.SetResources(resources);

                    await GrantContext
                        .HttpContext
                        .RequestServices
                        .GetRequiredService<AbpOpenIddictClaimsPrincipalManager>()
                        .HandleAsync(GrantContext.Request, principal);
                }
            }
            return principal;
        }
        /// <summary>
        /// 根据code得到用户信息
        /// </summary>
        /// <returns></returns>
        protected abstract Task<IResults> GetResultsAsync();

        public virtual async Task<IActionResult> HandleAsync(ExtensionGrantContext context)
        {
            GrantContext = context;
            LazyServiceProvider = context.HttpContext.RequestServices.GetRequiredService<IAbpLazyServiceProvider>();
            var results = await GetResultsAsync();
            if (results.ErrorCode != 0)
            {
                string msg = "errcode:" + results.ErrorCode + ";msg:" + results.ErrorMessage;
                return await NewForbidResult(msg);
            }
            //ProviderKey = results.UserId;
            await IdentityOptions.SetAsync();
            return await SwitchtVoidAsync(results);
        }
        protected virtual async Task<IActionResult> SwitchtVoidAsync(IResults results)
        {
            switch (results.ErrorCode)
            {
                case 0:
                    var claimsPrincipal = await ServerValidate();
                    if (claimsPrincipal == null)
                    {
                        return await NewForbidResult("未绑定", ProviderKey);
                    }
                    else
                    {
                        return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    }
                default:
                    return await NewForbidResult(results.ErrorMessage);
            }
        }

        protected async Task<ForbidResult> NewForbidResult(string msg, string? userCode = null)
        {
            return new ForbidResult(
                new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                await GetAuthProperties(msg, userCode));
        }
        private async Task<AuthenticationProperties> GetAuthProperties(string msg, string? userCode)
        {
            if (userCode == null)
            {
                Dictionary<string, string?> pairs = new Dictionary<string, string?>()
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = msg
                };
                return new AuthenticationProperties(pairs);
            }
            else
            {
                var dictionary = new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.LoginRequired,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = msg
                };
                if (userCode != null)
                {
                    /////加密获得的UserId或OpenId等，用于将前段用户登入后进行绑定
                    //Encrypter encrypter = new Encrypter();
                    //var ivBytes = encrypter.GenerateRandomHexText(16);
                    //string secureKey = "sKey123456";
                    //var eCode = encrypter.AES_Encrypt(userCode, secureKey, ivBytes);

                    //string iv = Convert.ToBase64String(ivBytes).Replace('/', '_');
                    //string url = "/user/login?redirect=%252Fworkplace?m={0}%2526code={1}.{2}";
                    //dictionary.Add(
                    //    OpenIddictServerAspNetCoreConstants.Properties.ErrorUri,
                    //    string.Format(url, Name, eCode, iv));
                }
                return new AuthenticationProperties(dictionary);
            }
        }
    }
}
