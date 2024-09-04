using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace XStudio;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
//        Log.Logger = new LoggerConfiguration()
//#if DEBUG
//            .MinimumLevel.Debug()
//#else
//            .MinimumLevel.Information()
//#endif
//            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
//            .Enrich.FromLogContext()
//            .WriteTo.Async(c => c.File("Logs/logs.txt"))
//            .WriteTo.Async(c => c.Console())
//            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) // 从配置中读取
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseNacosConfig("Nacos")
                .UseSerilog();
            Log.Information("Starting XStudio.HttpApi.Host.");
            await builder.AddApplicationAsync<XStudioHttpApiHostModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
