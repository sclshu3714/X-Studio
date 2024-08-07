using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.Extensions
{
    /// <summary>
    /// 适配器扩展服务
    /// </summary>
    public static class RegionAdapterExtensions
    {
        /// <summary>
        /// SfNavigationDrawer控件区域适配器
        /// </summary>
        /// <param name="regionAdapterMappings"></param>
        /// <param name="container"></param>
        public static void ConfigurationAdapters(
            this RegionAdapterMappings regionAdapterMappings,
            IContainerProvider container)
        {
            //regionAdapterMappings.RegisterMapping(typeof(SfNavigationDrawer),
            //    container.Resolve<SfNavigationDrawerRegionAdapter>());
        }
    }
}
