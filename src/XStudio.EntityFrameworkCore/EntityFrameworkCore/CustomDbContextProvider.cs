using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;
using XStudio.Common.Nacos;
using XStudio.Common;

namespace XStudio.EntityFrameworkCore
{
    public class CustomDbContextProvider : IDbContextProvider<XStudioDbContext>, ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        private readonly IConfiguration _configuration;

        public CustomDbContextProvider(
            ICurrentUser currentUser,
            IConfiguration configuration)
        {
            _currentUser = currentUser;
            _configuration = configuration;
        }

        public XStudioDbContext GetDbContext()
        {
            XStudioEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            if (GlobalConfig.Default.NacosConfig == null || GlobalConfig.Default.NacosConfig.Databases == null ||
                !GlobalConfig.Default.NacosConfig.Databases.Any())
            {
                configuration.Bind(GlobalConfig.Default.NacosConfig);
            }
            DatabaseInfo? info = GlobalConfig.Default.NacosConfig?.Databases?.Find(db => db.ConnectionName == "Default");
            if (info == null)
            {
                throw new InvalidOperationException($"数据库连接失败，没有检测到连接地址");
            }
            switch (info.DbMode)
            {
                case DatabaseType.Dmdbms:
                    if (!string.IsNullOrWhiteSpace(info.ConnectionString))
                    {
                        var builder = new DbContextOptionsBuilder<XStudioDbContext>()
                            .UseDm(info.ConnectionString, (DmDbContextOptionsBuilder opt) => { });

                        return new XStudioDbContext(builder.Options);
                    }
                    break;
                default:
                    if (!string.IsNullOrWhiteSpace(info.ConnectionString))
                    {
                        var builder = new DbContextOptionsBuilder<XStudioDbContext>()
                            .UseMySql(info.ConnectionString, MySqlServerVersion.LatestSupportedServerVersion);
                        return new XStudioDbContext(builder.Options);
                    }
                    break;
            }
            throw new InvalidOperationException($"数据库连接失败，{info.DbMode} 模式下，没有检测到连接地址");
        }

        public async Task<XStudioDbContext> GetDbContextAsync()
        {
            XStudioEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            if (GlobalConfig.Default.NacosConfig == null || GlobalConfig.Default.NacosConfig.Databases == null ||
                !GlobalConfig.Default.NacosConfig.Databases.Any())
            {
                configuration.Bind(GlobalConfig.Default.NacosConfig);
            }
            DatabaseInfo? info = GlobalConfig.Default.NacosConfig?.Databases?.Find(db => db.ConnectionName == "Default");
            if (info == null)
            {
                throw new InvalidOperationException($"数据库连接失败，没有检测到连接地址");
            }
            switch (info.DbMode)
            {
                case DatabaseType.Dmdbms:
                    if (!string.IsNullOrWhiteSpace(info.ConnectionString))
                    {
                        var builder = new DbContextOptionsBuilder<XStudioDbContext>()
                            .UseDm(info.ConnectionString, (DmDbContextOptionsBuilder opt) => { });

                        return new XStudioDbContext(builder.Options);
                    }
                    break;
                default:
                    if (!string.IsNullOrWhiteSpace(info.ConnectionString))
                    {
                        var builder = new DbContextOptionsBuilder<XStudioDbContext>()
                            .UseMySql(info.ConnectionString, MySqlServerVersion.LatestSupportedServerVersion);
                        return new XStudioDbContext(builder.Options);
                    }
                    break;
            }
            await Task.CompletedTask;
            throw new InvalidOperationException($"数据库连接失败，{info.DbMode} 模式下，没有检测到连接地址");
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../XStudio.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
