using XStudio.Common.Nacos;

namespace XStudio.Common
{
    public class GlobalConfig
    {
        public static GlobalConfig Default;

        static GlobalConfig()
        {
            Default = new GlobalConfig();
        }

        /// <summary>
        /// 是否启用Nacos
        /// </summary>
        public bool NacosEnabled { get; set; } = false;

        public GlobalNacosConfig? NacosConfig { get; set; }
    }
}
