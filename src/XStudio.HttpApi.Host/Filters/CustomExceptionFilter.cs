using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.AspNetCore.Components.Web;
using XStudio.Common;

namespace XStudio.Filters
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _env;

        public CustomExceptionFilter(IHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// 2021 -02-02 -yt
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            if (context == null)
            {
                Log.Error($"全局异常拦截 => 异常消息:未知异常，拦截到的ExceptionContext为空");
                return;
            }
            var response = new
            {
                Title = context.Exception?.GetErrorMessage(),
                Message = context.Exception?.Message,
                StackTrace = context.Exception?.StackTrace
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            // 标记异常已处理
            context.ExceptionHandled = true;

            //采用log4net 进行错误日志记录
            Log.Error($"全局异常拦截 => 异常消息:{context.Exception?.Message}; 异常堆栈: {context.Exception?.StackTrace}");
        }
    }
}
