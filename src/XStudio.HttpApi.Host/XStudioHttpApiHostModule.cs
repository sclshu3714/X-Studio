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
using Microsoft.AspNetCore.Identity;
using AspNetCoreRateLimit;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;

namespace XStudio;


//[DependsOn(
//    typeof(BookStoreHttpApiModule),
//    typeof(AbpAutofacModule),
//    typeof(AbpCachingStackExchangeRedisModule),
//    typeof(AbpDistributedLockingModule),
//    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
//    typeof(BookStoreApplicationModule),
//    typeof(BookStoreEntityFrameworkCoreModule),
//    typeof(AbpAspNetCoreSerilogModule),
//    typeof(AbpSwashbuckleModule)
//)]

[DependsOn(
    typeof(XStudioHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(XStudioApplicationModule),
    typeof(XStudioEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAspNetCoreAuthenticationOAuthModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class XStudioHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        PreConfigureEnvironment(configuration);
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
            //添加自定义MyTokenExtensionGrantConsts
            builder.Configure(openIddictServerOptions =>
            {
                openIddictServerOptions.GrantTypes.Add(MyTokenExtensionGrantConsts.GrantType);
            });
        });
    }

    private void PreConfigureEnvironment(IConfiguration configuration)
    {
        // 检查环境变量是否已设置，如果没有，则设置为开发环境
        var environment = configuration["App:Environment"]; // Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(environment) ||
            (environment != Environments.Development && environment != Environments.Staging && environment != Environments.Production))
        {
            // 这里可以根据需要设置不同的环境
            Log.Warning($"当前运行环境：{environment}, 不是常规环境，环境变量将切换到Development环境，但是配置文件依然读取 appsettings.{environment}.json");
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        }
        else
        {
            Log.Warning($"当前运行环境：{environment}");
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
        }
        
        // 根据环境加载不同的配置文件
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);

        // 加载补充配置文件
        if (File.Exists($"appsettings.{environment}.json"))
        {
            builder.AddJsonFile($"appsettings.{environment}.json", optional: true, true);
        }
        else
        {
            Log.Warning($"没有检查到配置文件:appsettings.{environment}.json; 告知：全部配置默认在appsettings.json中");
        }
        builder.AddEnvironmentVariables();
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
        ConfigureRateLimit(context, configuration);
        ConfigureAuthentication(context, configuration);
    }

    private void ConfigureRateLimit(ServiceConfigurationContext context, IConfiguration configuration)
    {
        // 限制配置
        context.Services.AddOptions();
        context.Services.AddMemoryCache();
        context.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        context.Services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
        context.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        context.Services.AddInMemoryRateLimiting();

        context.Services.Configure<IdentityOptions>(options =>
        {
            // 锁定配置
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;
        });
    }

    private void ConfigureNacos(ServiceConfigurationContext context, IConfiguration Configuration)
    {
        context.Services.AddNacosAspNet(Configuration, "Nacos");
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

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        //if (configuration["Jwt:Key"] is string jwtKey && !string.IsNullOrWhiteSpace(jwtKey))
        //{
        //    var key = Encoding.ASCII.GetBytes(jwtKey);
        //    context.Services.AddAuthentication(x =>
        //    {
        //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(x =>
        //    {
        //        x.RequireHttpsMetadata = true;
        //        x.SaveToken = true;
        //        x.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false
        //            //or
        //            //ValidateIssuer = true,
        //            //ValidateAudience = true,
        //            //ValidIssuer = Configuration["Jwt:Issuer"],
        //            //ValidAudience = Configuration["Jwt:Audience"],
        //        };
        //    });
        //}

        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });

        context.Services.AddControllersWithViews(Options =>
        {
            Options.Filters.Add<AbpAuthorizeFilter>();
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
        app.UseIpRateLimiting(); // 启用访问限制
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
    }
}
