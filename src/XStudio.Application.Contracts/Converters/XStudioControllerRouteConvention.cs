using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Converters {
    public class XStudioControllerRouteConvention : IApplicationModelConvention {
        public void Apply(ApplicationModel application) {
            foreach (var controller in application.Controllers) {
                // 将控制器名称转换为小写
                var controllerName = controller.ControllerName.ToLowerInvariant();
                foreach (var selector in controller.Selectors) {
                    // 更新路由模板
                    if (selector.AttributeRouteModel?.Template != null) {
                        selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template.Replace("[controller]", controllerName);
                    }
                }
            }
        }
    }
}
