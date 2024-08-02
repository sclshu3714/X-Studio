using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace XStudio.Filters
{
    public class AbpAuthorizationHandler : AuthorizationHandler<AbpRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AbpRequirement requirement)
        {
            // 在这里编写验证第三方Token权限的逻辑
            // 可以检查Token中的信息或调用第三方服务来验证权限

            if (true)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class AbpRequirement : IAuthorizationRequirement
    { 
        
    }
}
