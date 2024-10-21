using System;
using System.IO;
using DeviceDetectorNET;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using XStudio.Common;
using XStudio.Common.Helper;
using XStudio.Common.Nacos;

namespace XStudio.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class XStudioDbContextFactory : IDesignTimeDbContextFactory<XStudioDbContext>
{
    public XStudioDbContext CreateDbContext(string[] args)
    {
        XStudioEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();
        if (GlobalConfig.Default.NacosConfig == null) {
            GlobalConfig.Default.NacosConfig = new GlobalNacosConfig();
        }
        if (!GlobalConfig.Default.NacosConfig.Databases.Any()) {
            configuration.Bind(GlobalConfig.Default.NacosConfig);
        }
        DatabaseInfo? info = GlobalConfig.Default.NacosConfig?.Databases?.Find(db => db.ConnectionName == "Default");
        if (GlobalConfig.Default.NacosConfig == null || info == null)
        {
            throw new InvalidOperationException($"数据库连接失败，没有检测到连接地址, 配置内容：{GlobalConfig.Default.NacosConfig?.ToJson()}");
        }
        // Console.WriteLine($"配置内容：{GlobalConfig.Default.NacosConfig.ToJson()}");
        switch (info.DbMode) {
            case DatabaseType.Dmdbms:
                if (!string.IsNullOrWhiteSpace(info.ConnectionString)) {
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

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../XStudio.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
