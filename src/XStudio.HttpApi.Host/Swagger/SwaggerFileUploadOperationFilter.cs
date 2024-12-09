using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace XStudio.Swagger {
    // 自定义操作过滤器以支持文件上传
    public class SwaggerFileUploadOperationFilter : IOperationFilter {
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {
            var parameters = operation.Parameters;
            var requestBody = operation.RequestBody;

            // 检查是否有 IFormFile 参数
            if(context.MethodInfo.GetParameters().Any(p => p.ParameterType == typeof(IFormFile))) {
                parameters.Clear(); // 清除默认参数
                operation.Parameters.Add(new OpenApiParameter {
                    Name = "form",
                    In = ParameterLocation.Query,
                    Required = true,
                    Schema = new OpenApiSchema {
                        Type = "string",
                        Format = "binary"
                    }
                });
            }
        }
    }
}
