using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
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

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception caught in AppDomain.");
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Log.Fatal(e.Exception, "Unobserved task exception caught.");
                e.SetObserved();
            };

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
            // 也可以使用ASP.NET Core内置的异常处理
            app.UseExceptionHandler("/error"); // 指定错误处理的路径
            // 定义错误处理路径的处理逻辑
            app.Map("/error", async context => { // 使用 async lambda
                var feature = context.Features.Get<IExceptionHandlerFeature>(); // 使用 context.Features 而不是 context.ServerFeatures
                var error = feature?.Error;
                Log.Fatal(error, "Global error handler caught an exception.");
                // 这里使用 IResult 进行返回
                var result = Results.Problem("An unexpected error occurred.");
                await context.Response.WriteAsJsonAsync(result); // 返回 JSON 响应
            });
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
