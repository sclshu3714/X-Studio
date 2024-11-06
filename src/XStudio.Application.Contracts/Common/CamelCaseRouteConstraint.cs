using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common {
    /// <summary>
    /// 小驼峰路由约束
    /// </summary>
    public class CamelCaseRouteConstraint : IRouteConstraint {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection) {
            if (values.TryGetValue(routeKey, out var value) && value != null) {
                var controllerName = value.ToString();
                var camelCaseName = ToCamelCase(controllerName);
                values[routeKey] = camelCaseName;
                return true;
            }
            return false;
        }

        private string? ToCamelCase(string? input) {
            if (string.IsNullOrEmpty(input)) return input;
            return char.ToLowerInvariant(input[0]) + input.Substring(1);
        }
    }
}
