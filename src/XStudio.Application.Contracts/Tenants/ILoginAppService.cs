using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using XStudio.Users;

namespace XStudio.Tenants {
    public interface ILoginAppService {
        public Task<ActionResult<IdentityUserDto?>> Login(LoginDto loginDto);
        public Task<IdentityUserDto?> LoginV2(LoginDto loginDto);
    }
}
