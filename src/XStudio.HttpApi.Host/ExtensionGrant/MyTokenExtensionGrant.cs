using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.OpenIddict.Controllers;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using XStudio.Common;
using XStudio.Projects;

namespace XStudio.ExtensionGrant
{
    [IgnoreAntiforgeryToken]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MyTokenExtensionGrant : AbpOpenIdDictControllerBase, ITokenExtensionGrant
    {
        public string Name => MyTokenExtensionGrantConsts.GrantType;

        protected IIdentityUserAppService IdentityUserAppService => this.LazyServiceProvider.LazyGetRequiredService<IIdentityUserAppService>();

        [HttpPost]
        public virtual async Task<IActionResult> HandleAsync(ExtensionGrantContext context)
        {
            HttpContext httpContext = context.HttpContext;
            OpenIddictRequest request = context.Request;

            //this.LazyServiceProvider = httpContext.RequestServices.GetRequiredService<IAbpLazyServiceProvider>();

            string? appcode = request?.GetParameter("code")?.ToString();
            string? password = request?.GetParameter("password")?.ToString();
            if (true == string.IsNullOrWhiteSpace(appcode) || true == string.IsNullOrWhiteSpace(password))
            {
                string errorDescription = "请输入正确数据";
                AuthenticationProperties properties = new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            IdentityUserDto userDto = await this.IdentityUserAppService.FindByUsernameAsync(appcode);
            IdentityUser user = ObjectMapper.Map<IdentityUserDto, IdentityUser>(userDto);
            if (null == user)
            {
                string errorDescription = "没有查询到用户信息";
                AuthenticationProperties properties = new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            return await this.SetSuccessResultAsync(request, user);
        }

        protected virtual async Task<IActionResult> SetSuccessResultAsync(OpenIddictRequest? request, IdentityUser user)
        {
            ClaimsPrincipal principal = await this.SignInManager.CreateUserPrincipalAsync(user);

            //principal.SetScopes(request.GetScopes());
            //principal.SetResources(await GetResourcesAsync(request.GetScopes()));

            ImmutableArray<string> scopes = MyTokenExtensionGrantConsts.Scopes;
            principal.SetScopes(scopes);
            principal.SetResources(await GetResourcesAsync(scopes));

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}
