using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace XStudio.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class XStudioDbContextFactory : IDesignTimeDbContextFactory<XStudioDbContext>
{
    public XStudioDbContext CreateDbContext(string[] args)
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
