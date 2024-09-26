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


        //Log.Logger = new LoggerConfiguration()
        //    .MinimumLevel.Debug() // 设置全局最小日志级别  
        //    .WriteTo.File("Logs/app-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}")
        //        .Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Information && !e.Properties.ContainsKey("KafkaSource")) // 仅记录非Kafka的日志，且级别为Info及以上  
        //    .WriteTo.File("Logs/kafka-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}")
        //        .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("KafkaSource")) // 仅记录带有KafkaSource属性的日志  
        //    .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) // 从配置中读取
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseNacosConfig("Nacos", Nacos.YamlParser.YamlConfigurationStringParser.Instance) // 不使用Nacos.YamlParser.YamlConfigurationStringParser.Instance，默认将解析json格式
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
