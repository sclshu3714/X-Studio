using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Volo.Abp.OpenIddict;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using XStudio.Helpers;
using XStudio.Encrypter;
using Nacos.V2;
using System.Net.Http;
using Nacos.V2.Utils;
using System.Text;

namespace XStudio.Controllers
{
    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(2.0)]
    [ApiController]
    public class TokenController : AbpController
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly INacosNamingService _nnsvc;
        private readonly INacosConfigService _ncsvc;
        public TokenController(IOpenIddictApplicationManager applicationManager, IOpenIddictScopeManager scopeManager, INacosNamingService nnsvc, INacosConfigService ncsvc)
        {
            _applicationManager = applicationManager;
            _scopeManager = scopeManager;
            _nnsvc = nnsvc;
            _ncsvc = ncsvc;
        }

        [AllowAnonymous]
        [HttpPost("connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request != null && request.IsClientCredentialsGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.

                var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
                if (application == null)
                {
                    throw new InvalidOperationException("The application details cannot be found in the database.");
                }

                // Create the claims-based identity that will be used by OpenIddict to generate tokens.
                var identity = new ClaimsIdentity(
                    authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                    nameType: Claims.Name,
                    roleType: Claims.Role);

                // Add the claims that will be persisted in the tokens (use the client_id as the subject identifier).
                identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
                identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));

                // Note: In the original OAuth 2.0 specification, the client credentials grant
                // doesn't return an identity token, which is an OpenID Connect concept.
                //
                // As a non-standardized extension, OpenIddict allows returning an id_token
                // to convey information about the client application when the "openid" scope
                // is granted (i.e specified when calling principal.SetScopes()). When the "openid"
                // scope is not explicitly set, no identity token is returned to the client application.

                // Set the list of scopes granted to the client application in access_token.
                identity.SetScopes(request.GetScopes());
                identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
                identity.SetDestinations(GetDestinations);

                return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            throw new NotImplementedException("The specified grant type is not implemented.");
        }

        private static IEnumerable<string> GetDestinations(Claim claim)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            return claim.Type switch
            {
                Claims.Name or Claims.Subject => [Destinations.AccessToken, Destinations.IdentityToken],

                _ => [Destinations.AccessToken],
            };
        }

        [HttpPost("encrypt")]
        public IActionResult Encrypt(EncryptDto plain)
        {
            return Ok(EncrypterHelper.Encrypt(plain.PlainText));
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt(DecryptDto encrypted)
        {
            return Ok(EncrypterHelper.Decrypt(encrypted.EncryptedText));
        }

        // GET api/values/test
        [HttpGet("TestAsync")]
        public async Task<ActionResult<string>> TestAsync()
        {
            var instance = await _nnsvc.SelectOneHealthyInstance("XStudio", "wxz");
            if (instance == null)
            {
                return $"没有在Nacos中发现服务 XStudio";
            }
            var isHttps = HttpContext.Request.IsHttps; // 检查是否是通过 HTTPS 访问
            var host = $"{instance.Ip}:{(isHttps ? instance.Metadata["HttpsPort"] : instance.Port)}";
            var baseUrl = isHttps ? $"https://{host}" : $"http://{host}";

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return "empty";
            }
            string path = $"api/xstudio/v1/project/list";
            var url = $"{baseUrl.TrimEnd('/')}/{path}";
            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(handler))
                {
                    var page = new {
                        maxResultCount = 1,
                        skipCount = 0,
                        sorting = "id"
                    };
                    using HttpContent httpContent = new StringContent(page.ToJsonString(), Encoding.UTF8);
                    string contentType = "application/json";
                    if (contentType != null)
                    {
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                    }
                    try
                    {
                        var result = await client.PostAsync(url, new StringContent(page.ToJsonString()));
                        return await result.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {

                        return $"{ex.Message}: \r\n {ex.StackTrace}";
                    }
                }
            }
        }
    }
}
