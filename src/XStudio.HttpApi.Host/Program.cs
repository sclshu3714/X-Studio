using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Serilog;
using Serilog.Events;

namespace XStudio;

public class Program {
    public async static Task<int> Main(string[] args) {

        try {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac();
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            if (builder.Configuration.GetValue<bool>("Nacos:IsEnabled")) {
                builder.Host.UseNacosConfig("Nacos", Nacos.YamlParser.YamlConfigurationStringParser.Instance);
            }
            builder.Host.UseSerilog((context, logger) => {
                logger.ReadFrom.Configuration(context.Configuration)
                               .Enrich
                               .FromLogContext();
                Log.Information("Starting XStudio Serilog.");
            });

            await builder.AddApplicationAsync<XStudioHttpApiHostModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex) {
            if (ex is HostAbortedException) {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally {
            Log.CloseAndFlush();
        }
    }
}
