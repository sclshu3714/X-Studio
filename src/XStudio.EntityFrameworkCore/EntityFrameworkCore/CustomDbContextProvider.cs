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

            var DBMode = configuration["App:DBMode"]; // 数据库模式：MYSQL;DM;
            switch (DBMode)
            {
                case "DM":
                    string? myDmdbms = configuration.GetConnectionString("Dmdbms");
                    if (!string.IsNullOrWhiteSpace(myDmdbms))
                    {
                        var builder = new DbContextOptionsBuilder<XStudioDbContext>()
                            .UseDm(myDmdbms, (DmDbContextOptionsBuilder opt) => { });

                        return new XStudioDbContext(builder.Options);
                    }
                    break;
                default:
                    string? mySql = configuration.GetConnectionString("Default");
                    if (!string.IsNullOrWhiteSpace(mySql))
                    {
                        var builder = new DbContextOptionsBuilder<XStudioDbContext>()
                            .UseMySql(mySql, MySqlServerVersion.LatestSupportedServerVersion);
                        return new XStudioDbContext(builder.Options);
                    }
                    break;
            }
            throw new InvalidOperationException($"数据库连接失败，{DBMode} 模式下，没有检测到连接地址");
        }

        public async Task<XStudioDbContext> GetDbContextAsync()
        {
            XStudioEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var DBMode = configuration["App:DBMode"]; // 数据库模式：MYSQL;DM;
            switch (DBMode)
            {
                case "DM":
                    string? myDmdbms = configuration.GetConnectionString("Dmdbms");
                    if (!string.IsNullOrWhiteSpace(myDmdbms))
                    {
                        var builder = new DbContextOptionsBuilder<XStudioDbContext>()
                            .UseDm(myDmdbms, (DmDbContextOptionsBuilder opt) => { });

                        return new XStudioDbContext(builder.Options);
                    }
                    break;
                default:
                    string? mySql = configuration.GetConnectionString("Default");
                    if (!string.IsNullOrWhiteSpace(mySql))
                    {
                        var builder = new DbContextOptionsBuilder<XStudioDbContext>()
                            .UseMySql(mySql, MySqlServerVersion.LatestSupportedServerVersion);
                        return new XStudioDbContext(builder.Options);
                    }
                    break;
            }
            throw new InvalidOperationException($"数据库连接失败，{DBMode} 模式下，没有检测到连接地址");
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
