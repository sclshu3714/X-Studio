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

        public GlobalNacosConfig? NacosConfig { get; set; }
    }
}
