using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace XStudio.Users
{
    public class LoginAppService : ApplicationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginAppService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password,false);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false); // 设置为true可以记住用户的登录状态
                                                                   // 登录成功后的逻辑，例如重定向到主页
                }
                else
                {
                    // 密码错误的逻辑
                }
            }
            else
            {
                // 用户不存在的逻辑
            }
        }
    }
}
