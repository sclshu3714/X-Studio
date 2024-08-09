using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XStudio.EntityFrameworkCore;
using XStudio.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Asp.Versioning.ApplicationModels;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using XStudio.Swagger;
using XStudio.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using XStudio.Common;
using XStudio.Converters;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Nacos.V2.DependencyInjection;
using Nacos.AspNetCore.V2;
using static IdentityModel.ClaimComparer;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using XStudio.ExtensionGrant;
using Serilog;
using XStudio.Nacos;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Nacos.V2;

namespace XStudio;

[DependsOn(
    typeof(XStudioHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(XStudioApplicationModule),
    typeof(XStudioEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreAuthenticationOAuthModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class XStudioHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        ConfigureEnvironment(configuration);
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("XStudio");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            //添加自定义ITokenExtensionGrant
            builder.Configure(openIddictServerOptions =>
            {
                openIddictServerOptions.GrantTypes.Add(MyTokenExtensionGrantConsts.GrantType);
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
        ConfigureAbpApiVersioning(context);
        ConfigureNewtonsoftJson(context);
        ConfigureNacos(context, configuration);
        ConfigureAuthentication(context);
    }

    private void ConfigureEnvironment(IConfiguration configuration)
    {
        // 检查环境变量是否已设置，如果没有，则设置为开发环境
        var environment = configuration["App:Environment"]; // Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(environment) || 
            (environment != Environments.Development && environment != Environments.Staging && environment != Environments.Production))
        {
            // 这里可以根据需要设置不同的环境
            environment = "Development";
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        }
        else
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        }

        // 根据环境加载不同的配置文件
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);
        if (File.Exists($"appsettings.{environment}.json"))
        {
            builder.AddJsonFile($"appsettings.{environment}.json", optional: true, true);
        }
        builder.AddEnvironmentVariables();
    }

    private void ConfigureNacos(ServiceConfigurationContext context, IConfiguration Configuration)
    {
        context.Services.AddNacosAspNet(Configuration, "Nacos");

        //context.Services.AddNacosAspNet(a =>
        //{
        //    a.Ephemeral = config.Ephemeral;
        //    a.Ip = config.Ip;
        //    a.Port = config.Port;
        //    a.Secure = config.Secure;
        //    a.ClusterName = config.ClusterName;
        //    a.GroupName = config.GroupName;
        //    a.Weight = config.Weight;
        //    a.ServiceName = config.ServiceName;
        //    a.Namespace = config.Namespace;
        //    a.Password = config.Password;
        //    a.UserName = config.UserName;
        //    a.AccessKey = config.AccessKey;
        //    a.ContextPath = config.ContextPath;
        //    a.EndPoint = config.EndPoint;
        //    a.ListenInterval = config.ListenInterval;
        //    a.SecretKey = config.SecretKey;
        //    a.ServerAddresses = config.ServerAddresses;
        //    a.ConfigFilterAssemblies = config.ConfigFilterAssemblies;
        //    a.ConfigUseRpc = config.ConfigUseRpc;
        //    a.DefaultTimeOut = config.DefaultTimeOut;
        //    a.NamingUseRpc = config.NamingUseRpc;
        //    a.RamRoleName = config.RamRoleName;
        //    a.Metadata = config.Metadata;
        //    a.ConfigFilterExtInfo = config.ConfigFilterExtInfo;
        //    a.NamingCacheRegistryDir = config.NamingCacheRegistryDir;
        //    a.NamingLoadCacheAtStart = config.NamingLoadCacheAtStart;
        //    a.NamingLoadCacheAtStart = config.NamingLoadCacheAtStart;
        //});

        GlobalConfig.Default.NacosConfig = Configuration.Get<GlobalNacosConfig>();
        if (GlobalConfig.Default.NacosConfig != null)
        {
            context.Services.AddSingleton(GlobalConfig.Default.NacosConfig);
        }
        context.Services.AddNacosV2Config(Configuration);
    }

    private void ConfigureNewtonsoftJson(ServiceConfigurationContext context)
    {
        context.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
        {
            //修改属性名称的序列化方式，首字母小写
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //修改时间的序列化方式
            options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            options.SerializerSettings.Converters.Add(new IpAddressConverter());
            options.SerializerSettings.Converters.Add(new IpEndPointConverter());
        });
    }

    private void ConfigureAbpApiVersioning(ServiceConfigurationContext context)
    {
        context.Services.AddAbpApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("version")
            );
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ChangeControllerModelApiExplorerGroupName = false;
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        //context.Services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("XStudioPolicy", policy =>
        //    {
        //        policy.RequireAuthenticatedUser();
        //        policy.Requirements.Add(new AbpRequirement());
        //    });
        //});
        //context.Services.AddSingleton<IAuthorizationHandler, AbpAuthorizationHandler>();

        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
        //context.Services.AddControllersWithViews(Options =>
        //{
        //    Options.Filters.Add<AbpAuthorizeFilter>();
        //});

        //配置自定义ITokenExtensionGrant
        context.Services.Configure<AbpOpenIddictExtensionGrantsOptions>(options =>
        {
            options.Grants.Add(MyTokenExtensionGrantConsts.GrantType, new MyTokenExtensionGrant());
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<XStudioDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}XStudio.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<XStudioDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}XStudio.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<XStudioApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}XStudio.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<XStudioApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}XStudio.Application"));
            });
        }
    }

    // 自动生成控制器
    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(XStudioApplicationModule).Assembly, opts =>
            {
                // 指定后:https://localhost:44345/api/xstudio/project; 默认:https://localhost:44345/api/app/project
                opts.RootPath = "xstudio";
                // opts.TypePredicate = type => { return true; }; //是否公开
            });
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                    {"XStudio", "XStudio API"}
            },
            options =>
            {
                //options.SwaggerDoc("v1", new OpenApiInfo { Title = "XStudio V1 API", Version = "v1" });
                //options.SwaggerDoc("v2", new OpenApiInfo { Title = "XStudio V2 API", Version = "v2" });
                //options.DocInclusionPredicate((docName, description) => true);
                //options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        //nacos 监听
        IConfiguration configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        IHostApplicationLifetime appLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        INacosConfigService ncsvc = app.ApplicationServices.GetRequiredService<INacosConfigService>();
        NacosConfigListener _configListen = new NacosConfigListener(appLifetime);
        // 遍历 Nacos:Listeners 内的值
        var listeners = configuration.GetSection("Nacos:Listeners").Get<List<NacosListener>>();
        if (listeners != null)
        {
            foreach (var listener in listeners)
            {
                ncsvc.AddListener(listener.DataId, listener.Group, _configListen);
            }
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseMiddleware<AbpExceptionMiddleware>(); //ExceptionMiddleware 加入管道
        //app.UseMiddleware<AbpTokenValidationMiddleware>(); // token验证
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }
        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            //c.SwaggerEndpoint("/swagger/v1/swagger.json", "XStudio API");
            IApiVersionDescriptionProvider provider;
            try
            {
                provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                provider.If(provider.ApiVersionDescriptions != null, (provider) => {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
                c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                c.OAuthScopes("XStudio");
            }
            catch (Exception ex)
            {
                Log.Error($"构建swagger文档失败,{ex.StackTrace}");
            }          
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapControllers().RequireAuthorization("XStudioPolicy");
        //});
    }
}
