namespace XStudio.Nacos
{
    /// <summary>
    /// 全局Nacos配置，和远程Nacos映射
    /// </summary>
    public class GlobalNacosConfig
    {
        public GlobalNacosConfig() { }

        public MySqlInfo? MySql { get; set; }

        public KafkaInfo? Kafka { get; set; }

        public RedisInfo? Redis { get; set; }
    }


    public class MySqlInfo
    { 
        public string Host { get; set; } = "localhost";
        public long Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class KafkaInfo
    {
        public string Host { get; set; } = "localhost";
        public long Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RedisInfo
    {
        public string Host { get; set; } = "localhost";
        public long Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
