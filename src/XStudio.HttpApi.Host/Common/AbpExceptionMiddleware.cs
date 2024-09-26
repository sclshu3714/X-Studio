using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using Serilog;
using XStudio.Helpers;
using Microsoft.Extensions.Hosting;

namespace XStudio.Common
{
    /// <summary>
    /// 异常情况处理的中间件
    /// </summary>
    public class AbpExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private IWebHostEnvironment environment;

        public AbpExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            this.next = next;
            this.environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
                var features = context.Features;
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        private async Task MyHandleException(HttpContext context, Exception e)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "text/json;charset=utf-8;";
            string? error = e.StackTrace?.ToJson();
            string info = $@"StatusCode:{context.Response.StatusCode}";
            string? remoteIpAddr = context.Connection.RemoteIpAddress?.ToString();
            info = $@"{info}->Body: {error}";
            if (!string.IsNullOrWhiteSpace(error))
            {
                Log.Logger.Error($@"[ExceptionMiddleware]->HTTP-OUTPUT->{remoteIpAddr}->{context.Request.Method}->{context.Request.Path}->" + info);
                await context.Response.WriteAsync(error);
            }
        }

        private async Task HandleException(HttpContext context, Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/json;charset=utf-8;";
            string error = "";
            if (environment.IsDevelopment())
            {
                var json = new { message = e.Message };
                error = JsonConvert.SerializeObject(json);
            }
            else
            {
                error = "抱歉，出错了\r\n" + e.Message + "\r\n" + e.StackTrace;
            }

            string info = $@"StatusCode:{context.Response.StatusCode}";
            string? remoteIpAddr = context.Connection.RemoteIpAddress?.ToString();
            info = $@"{info}->Body: {error}";
            Log.Logger.Error($@"[ExceptionMiddleware]->HTTP-OUTPUT->{remoteIpAddr}->{context.Request.Method}->{context.Request.Path}->" + info);
            await context.Response.WriteAsync(error);
        }
    }
}
