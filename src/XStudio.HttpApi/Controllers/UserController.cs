using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Identity;
using XStudio.Tenants;
using XStudio.Users;

namespace XStudio.Controllers {
    //[RemoteService(Name = "xstudio")]
    [Area("User")]
    [Route("api/xstudio/[controller]")]
    [ApiController]
    public class UserController {
        private readonly ILoginAppService _loginAppService;

        public UserController (ILoginAppService loginAppService) {
            _loginAppService = loginAppService;
        }

        [HttpPost("v{version:apiVersion}/login")]
        [ApiVersion(1.0)]
        public async Task<ActionResult<IdentityUserDto>> Login(LoginDto loginDto) { 
           return await _loginAppService.Login(loginDto);
        }

        [HttpPost("v{version:apiVersion}/login")]
        [ApiVersion(2.0)]
        public async Task<ActionResult<IdentityUserDto>> LoginV2(LoginDto loginDto) {
            return await _loginAppService.Login(loginDto);
        }
    }
}
