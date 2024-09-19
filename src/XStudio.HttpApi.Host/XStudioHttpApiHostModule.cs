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
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
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
using static Org.BouncyCastle.Math.EC.ECCurve;
using Nacos.V2;
using Microsoft.AspNetCore.Identity;
using AspNetCoreRateLimit;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using XStudio.Common.Nacos;
using Nacos.V2.Config;
using Nacos.V2.Utils;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.EventBus.Kafka;
using Volo.Abp.Kafka;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Confluent.Kafka;
using XStudio.Helpers;
using Microsoft.Extensions.Logging;

namespace XStudio;

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
    typeof(AbpAspNetCoreSerilogModule),                 // Serilog
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSignalRModule),                 // SignalR
    typeof(AbpCachingStackExchangeRedisModule),         // Redis
    typeof(AbpEventBusKafkaModule)                      // Kafka
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

            // 设置 token 过期时间为 24 小时
            builder.SetAccessTokenLifetime(TimeSpan.FromHours(24));
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

        ConfigureNacos(context, configuration);
        ConfigureSerilog(context, configuration);
        ConfigureKafka(context, configuration);
        ConfigureRedis(context, configuration);
        ConfigureAuthentication(context, configuration);
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
        ConfigureAbpApiVersioning(context);
        ConfigureNewtonsoftJson(context);
        ConfigureRateLimit(context, configuration);
        AddAbpBackgroundJobs(context);

    }

    private async void ConfigureNacos(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddNacosAspNet(configuration, "Nacos");
        context.Services.AddNacosV2Config(configuration);

        string? dataId = configuration.GetSection("Nacos:DataId").Value;//: "xstudio.json",
        string? group = configuration.GetSection("Nacos:GroupName").Value;//: "DEFAULT_GROUP",
        long timeoutMs = 3000;
        //监听nacos 应用配置
        if (dataId != null && group != null)
        {
            string? format = Path.GetExtension(dataId);// configuration.GetSection("Nacos:Format").Value;//: "JSON",
            var serviceProvider = context.Services.BuildServiceProvider();
            INacosConfigService nacosConfigService = serviceProvider.GetRequiredService<INacosConfigService>();
            string currentConfig = await nacosConfigService.GetConfig(dataId, group, timeoutMs);
            if (string.IsNullOrEmpty(currentConfig))
            {
                Log.Warning($"Nacos 配置文件 {dataId} 在组 {group} 中未找到或为空。");
            }
            else if (format.ToUpper() == ".JSON")
            {
                GlobalConfig.Default.NacosConfig = currentConfig.ToObj<GlobalNacosConfig>();
            }
            else if (format.ToUpper() == ".YAML" || format.ToUpper() == ".YML")
            {
                NacosYamlHelper helper = new NacosYamlHelper(nacosConfigService);
                GlobalConfig.Default.NacosConfig = await helper.GetYamlConfigAsync<GlobalNacosConfig>(currentConfig);
            }
        }
        //GlobalConfig.Default.NacosConfig = configuration.Get<GlobalNacosConfig>();
        //configuration.Bind(GlobalConfig.Default.NacosConfig);
        if (GlobalConfig.Default.NacosConfig != null)
        {
            context.Services.AddSingleton(GlobalConfig.Default.NacosConfig);
        }
    }

    private void ConfigureSerilog(ServiceConfigurationContext context, IConfiguration configuration)
    {
        // 将 Serilog 注册到 DI 容器
        context.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders(); // 清除默认的日志提供程序
            loggingBuilder.AddSerilog()
                          .AddConfiguration(configuration)
                          .AddConsole(); // 添加 Serilog
        });
    }
    private void ConfigureKafka(ServiceConfigurationContext context, IConfiguration configuration)
    {
        //配置连接
        var kafkaOptions = new AbpKafkaOptions();
        configuration.GetSection("Kafka:Connections").Bind(kafkaOptions.Connections);
        context.Services.Configure<AbpKafkaOptions>(options =>
        {
            foreach (var connection in kafkaOptions.Connections)
            {
                options.Connections.TryAdd(connection.Key, connection.Value);
                options.Connections[connection.Key].SaslUsername = connection.Value.SaslUsername;
                options.Connections[connection.Key].SaslPassword = connection.Value.SaslPassword;
            }

            options.ConfigureConsumer = config =>
            {
                config.GroupId = "xstudio";
                config.EnableAutoCommit = false;
            };

            options.ConfigureProducer = config =>
            {
                config.MessageTimeoutMs = 6000;
                config.Acks = Acks.All;
            };
        });

        var kafkaEventBusOptions = new AbpKafkaEventBusOptions();
        configuration.GetSection("Kafka:EventBus").Bind(kafkaEventBusOptions);
        context.Services.Configure<AbpKafkaEventBusOptions>(options =>
        {
            options.GroupId = kafkaEventBusOptions.GroupId;
            options.TopicName = kafkaEventBusOptions.TopicName;
            options.ConnectionName = kafkaEventBusOptions.ConnectionName;
        });

        context.Services.Configure<AbpKafkaOptions>(options =>
        {
            // 配置 consumer config
            options.ConfigureConsumer = config =>
            {
                config.GroupId = kafkaEventBusOptions.GroupId;
                config.EnableAutoCommit = false;
            };

            // 配置 producer config
            options.ConfigureProducer = config =>
            {
                config.MessageTimeoutMs = 6000;
                config.Acks = Acks.All;
            };

            // 配置 topic specification
            options.ConfigureTopic = specification =>
            {
                specification.ReplicationFactor = 3;
                specification.NumPartitions = 3;
            };
        });
    }


    /// <summary>
    /// 配置Redis
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ConfigureRedis(ServiceConfigurationContext context, IConfiguration configuration)
    {
        // 配置Redis
        context.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:Configuration"];
            //options.InstanceName = configuration["Redis:InstanceName"];
        });
        // 配置Redis缓存
        //Configure<RedisCacheOptions>(options =>
        //{
        //    options.Configuration = context.Services.GetConfiguration()["Redis:Configuration"];
        //    //options.InstanceName = "MyCustomRedisInstance"; // 如果需要，可以设置Redis实例名称
        //});
    }
    private void AddAbpBackgroundJobs(ServiceConfigurationContext context)
    {
        Configure<AbpAntiForgeryOptions>(options =>
        {
            // TODO wxz
            options.AutoValidate = false; // 禁用自动验证
        });
        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false;
        });
        context.Services.AddTransient<AutoPartitionBackgroundJobService>(); // 注册后台任务服务-数据库分区任务
        context.Services.AddHostedService<AutoPartitionBackgroundJobService>(); // 注册后台任务服务-数据库分区任务
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        EncrypterHelper.EncryptionKey = configuration.GetSection("StringEncryption:DefaultPassPhrase").Value ?? "xstudio_encryptionkey";
        //context.Services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //});
        //.AddFacebook(facebook =>
        //{
        //    facebook.AppId = "...";
        //    facebook.AppSecret = "...";
        //    facebook.Scope.Add("email");
        //    facebook.Scope.Add("public_profile");
        //    facebook.Scope.Add("XStudio");
        //})
        //.AddJwtBearer(options =>
        //{
        //    options.Authority = configuration["AuthServer:Authority"];
        //    options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata");
        //    options.Audience = "XStudio";
        //});
        //context.Services.AddOpenIddict()
        //    .AddServer(options =>
        //    {
        //        options.SetAccessTokenLifetime(TimeSpan.FromHours(24)); // 设置 token 过期时间为 24 小时
        //        options.AllowAuthorizationCodeFlow()
        //               .RequireProofKeyForCodeExchange()
        //               .AllowRefreshTokenFlow();

        //        options.SetTokenEndpointUris("/connect/token")
        //               .SetAuthorizationEndpointUris("/connect/authorize");

        //        options.AddDevelopmentEncryptionCertificate()
        //               .AddDevelopmentSigningCertificate();

        //        options.UseAspNetCore()
        //               .EnableTokenEndpointPassthrough()
        //               .EnableAuthorizationEndpointPassthrough();
        //    })
        //    .AddValidation(options =>
        //    {
        //        options.UseLocalServer();
        //        options.UseAspNetCore();
        //    });

        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });

        context.Services.AddControllersWithViews(Options =>
        {
            Options.Filters.Add<AbpAuthorizeFilter>();
        });

        //默认的
        //context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        //context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        //{
        //    options.IsDynamicClaimsEnabled = true;
        //});
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
        context.Services.AddSignalR();
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

        IConfiguration configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        //nacos 监听
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
        app.UseMiddleware<IpRateLimitMiddleware>();
        //app.UseMiddleware<AbpTokenValidationMiddleware>(); // token验证
        //app.UseJwtTokenMiddleware();
        app.UseAuthentication();
        //app.UseAbpOpenIddictValidation();

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
                provider.If(provider.ApiVersionDescriptions != null, (provider) =>
                {
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
