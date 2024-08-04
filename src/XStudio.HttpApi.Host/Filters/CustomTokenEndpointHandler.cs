namespace XStudio.Filters
{
    //public class CustomTokenEndpointHandler : OpenIddictServerHandler<OpenIddictServerOptions>
    //{
    //    public CustomTokenEndpointHandler(IServiceProvider services)
    //        : base(services)
    //    {
    //    }
    //    public override async Task HandleAsync([NotNull] OpenIddictServerHandleContext context)
    //    {
    //        if (context == null)
    //        {
    //            throw new ArgumentNullException(nameof(context));
    //        }
    //        // 从数据库检索客户机应用程序.
    //        var application = await context.HttpContext.GetOpenIddictServerApplicationAsync();
    //        if (application == null)
    //        {
    //            throw new InvalidOperationException("The client application cannot be retrieved.");
    //        }
    //        // 从授权服务器设置检索用户主体.
    //        var principal = context.HttpContext.User;
    //        // 确保允许应用程序使用指定的授权类型。
    //        if (!await ValidateClientRedirectUriAsync(application, context.Request))
    //        {
    //            throw new InvalidOperationException("The grant type is not allowed for this application.");
    //        }
    //        //注意：这个自定义令牌终端点总是忽略“scopes”参数，并根据授予的scopes/roles自动定义声明。
    //        var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(),
    //            OpenIddictServerDefaults.AuthenticationScheme);
    //        // 根据请求的自定义授权类型自定义令牌生命周期.
    //        if (string.Equals(context.Request.GrantType, "urn:custom_grant", StringComparison.OrdinalIgnoreCase))
    //        {
    //            // Set the token expiration to 1 hour.
    //            ticket.Properties.ExpiresUtc = context.Options.SystemClock.UtcNow.AddHours(1);
    //        }
    //        else
    //        {
    //            // 将令牌过期时间设置为默认持续时间(5分钟)
    //            ticket.Properties.ExpiresUtc = context.Options.SystemClock.UtcNow.Add(
    //                context.Options.AccessTokenLifetime ?? TimeSpan.FromMinutes(5));
    //        }
    //        context.Logger.LogInformation("The custom token request was successfully processed.");
    //        await context.HttpContext.SignInAsync(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
    //        // 将响应标记为已处理，以跳过管道的其余部分.
    //        context.HandleRequest();
    //    }
    //}
}
