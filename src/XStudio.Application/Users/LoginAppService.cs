using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace XStudio.Users
{
    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class LoginAppService : ApplicationService
    {
        private readonly IdentityUserManager _userManager;

        public LoginAppService(IdentityUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<string?> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await _userManager.GetAuthenticationTokenAsync(user, "a","b");
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
