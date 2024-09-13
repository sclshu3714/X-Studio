using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Xml.Schema;
using XStudio.Common;

namespace XStudio.Nacos
{
    /// <summary>
    /// 全局Nacos配置，和远程Nacos映射
    /// </summary>
    public class GlobalNacosConfig
    {
        public GlobalNacosConfig() { }

        public AppInfo? App { get; set; }

        public List<DBSqlInfo>? Databases { get; set; }

        public KafkaInfo? Kafka { get; set; }

        public RedisInfo? Redis { get; set; }
    }

    public class AppInfo
    {
        /// <summary>
        /// Development 开发; Staging 预发布; Production 生产;
        /// </summary>
        public string Environment { get; set; } = "Development";
        /// <summary>
        /// 数据库模式：MYSQL 或者 DM (mysql 或者 达梦)
        /// </summary>
        public string DBMode { get; set; } = "MYSQL";

        /// <summary>
        /// 应用程序自身的 URL，用于生成重定向或链接。
        /// </summary>
        public string? SelfUrl { get; set; }

        /// <summary>
        /// 客户端应用程序的 URL，通常用于跨域请求。
        /// </summary>
        public string? ClientUrl { get; set; }

        /// <summary>
        /// 定义允许跨域请求的源（Origin）
        /// </summary>
        public string? CorsOrigins { get; set; }

        /// <summary>
        /// 定义允许重定向的 URL 列表。
        /// </summary>
        public string? RedirectAllowedUrls { get; set; }
    }

    public class DBSqlInfo
    {
        /// <summary>
        /// 数据库模式
        /// </summary>
        public string DbMode { get; set; } = "MySql";

        /// <summary>
        /// Ip或者域名
        /// </summary>
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// 可选参数，端口
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbName { get; set; } = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 启动参数配置
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>(); // 用于存储不固定的数据
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
