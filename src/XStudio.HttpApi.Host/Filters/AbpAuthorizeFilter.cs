using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace XStudio.Filters
{
    public class AbpAuthorizeFilter : AuthorizeFilter
    {
        private readonly IWebHostEnvironment _env;
        public AbpAuthorizeFilter(IServiceProvider serviceProvider, IWebHostEnvironment env)
            : base(policyProvider: serviceProvider.GetRequiredService<IAuthorizationPolicyProvider>(), authorizeData: new[] { new AuthorizeAttribute() })
        {
            _env = env;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            OnAuthorizationAsync(context);
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (HasAllowAnonymous(context))
            {
                return Task.CompletedTask;
            }
            //过滤动态API
            if ((context.HttpContext?.Request?.Path.Value != null &&
                context.HttpContext.Request.Path.Value.EndsWith("/api-definition")) ||
                _env.IsDevelopment())
            {
                return Task.CompletedTask;
            }
            return base.OnAuthorizationAsync(context);
        }
        private bool HasAllowAnonymous(AuthorizationFilterContext context)
        {
            var filters = context.Filters;
            if (filters.OfType<IAllowAnonymousFilter>().Any())
            { 
                return true;
            }
            var endpoint = context.HttpContext.GetEndpoint();
            return endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
        }
    }
}
