using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XStudio.Common.Nacos
{
    /// <summary>
    /// 全局Nacos配置，和远程Nacos映射
    /// </summary>
    public class GlobalNacosConfig
    {
        public GlobalNacosConfig() { }

        /// <summary>
        /// appsetting
        /// </summary>
        public AppInfo? App { get; set; }

        /// <summary>
        /// 数据库连接信息
        /// </summary>
        public List<DatabaseInfo>? Databases { get; set; }

        /// <summary>
        /// 连接地址
        /// </summary>
        public Dictionary<string, string> ConnectionStrings
        {
            get
            {
                if (Databases == null || !Databases.Any()) return new Dictionary<string, string>();
                return Databases.ToDictionary(k => k.ConnectionName, v => v.ConnectionString);
            }
        }

        /// <summary>
        /// kafka
        /// </summary>
        public KafkaInfo? Kafka { get; set; }

        /// <summary>
        /// Redis
        /// </summary>
        public RedisInfo? Redis { get; set; }

        /// <summary>
        /// 限制接口访问速率 限制配置 包含了全局速率限制的设置
        /// </summary>
        public IpRateLimitingInfo? IpRateLimiting { get; set; }

        /// <summary>
        /// 特定策略的设置
        /// </summary>
        public IpRateLimitPoliciesInfo? IpRateLimitPolicies { get; set; }
    }

    /// <summary>
    /// App
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// Development 开发; Staging 预发布; Production 生产;
        /// </summary>
        public string Environment { get; set; } = "Development";

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

    /// <summary>
    /// 数据库信息
    /// </summary>
    public class DatabaseInfo
    {
        /// <summary>
        /// 连接名称
        /// </summary>
        public string ConnectionName { get; set; } = string.Empty;

        /// <summary>
        /// 数据库模式
        /// </summary>
        public DatabaseType DbMode { get; set; } = DatabaseType.MySql;

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
        public string DatabaseName { get; set; } = string.Empty;

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

        /// <summary>
        /// 连接地址
        /// </summary>
        public string ConnectionString
        {
            get
            {
                switch (DbMode)
                {
                    case DatabaseType.Dmdbms:
                        string ConnectionDmdbmsString = $"Server={Host};Port={Port};Database={DatabaseName};User Id={UserId};Password={Password}";
                        foreach (var item in Metadata)
                        {
                            ConnectionDmdbmsString += $";{item.Key}={item.Value}";
                        }
                        return ConnectionDmdbmsString;
                    case DatabaseType.MySql:
                    default:
                        string ConnectionMySqlString = $"Server={Host};Port={Port};Database={DatabaseName};User Id={UserId};Password={Password}";
                        foreach (var item in Metadata)
                        {
                            ConnectionMySqlString += $";{item.Key}={item.Value}";
                        }
                        return ConnectionMySqlString;
                }
            }
        }
    }

    /// <summary>
    /// kafka
    /// </summary>
    public class KafkaInfo
    {
        public bool IsEnabled { get; set; } = true;
        public ConnectionInfo? Connections { get; set; }
        public EventBusInfo? EventBus { get; set; }
    }

    public class ConnectionInfo
    {
        public string ConnectionName { get; set; } = "Default";
        public string Host { get; set; } = "localhost";
        public long Port { get; set; } = 9092;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string BootstrapServers => $"{Host}:{Port}";
    }

    public class EventBusInfo
    {
        public string GroupId { get; set; } = "Default";
        public string TopicName { get; set; } = "localhost";
        public string ConnectionName { get; set; } = string.Empty;
    }

    /// <summary>
    /// redis
    /// </summary>
    public class RedisInfo
    {
        public bool IsEnabled { get; set; } = true;
        public string Configuration { get; set; } = string.Empty;
    }

    /// <summary>
    /// 限制接口访问速率 限制配置 包含了全局速率限制的设置
    /// </summary>
    public class IpRateLimitingInfo
    {
        /// <summary>
        /// 启用|关闭 端点的速率限制
        /// </summary>
        public bool EnableEndpointRateLimiting { get; set; } = true;

        /// <summary>
        /// 如果设置为true，被限制的请求会被放入一个等待队列中，而不是直接返回429状态码。
        /// 这里设置为false，意味着超出限制的请求将直接收到429 Too Many Requests的响应。
        /// </summary>
        public bool StackBlockedRequests { get; set; } = false;

        /// <summary>
        /// 用于确定客户端的真实IP地址的HTTP头。这在代理或负载均衡器后面使用时很有用， 用于识别真实的客户端IP和客户端ID。。
        /// </summary>
        public string RealIpHeader { get; set; } = "X-Real-IP";

        /// <summary>
        /// 用于识别客户端的HTTP头。这可以用来区分不同的客户端或用户, 用于识别真实的客户端IP和客户端ID。
        /// </summary>
        public string ClientIdHeader { get; set; } = "X-ClientId";

        /// <summary>
        /// 当请求超出限制时返回的HTTP状态码。429表示Too Many Requests，即请求过多。
        /// </summary>
        public int HttpStatusCode { get; set; } = 429;

        /// <summary>
        /// 包含一组通用规则，用于定义限制条件。
        /// </summary>
        List<GeneralRule> GeneralRules { get; set; } = new List<GeneralRule>();

        /// <summary>
        /// 不受速率限制的客户端IP列表
        /// </summary>
        public List<string> ClientWhitelist { get; set; } = new List<string>();

        /// <summary>
        /// 不受速率限制的端点列表
        /// </summary>
        public List<string> EndpointWhitelist { get; set; } = new List<string>();

        /// <summary>
        /// 不受速率限制的
        /// </summary>
        public List<string> IpWhitelist { get; set; } = new List<string>() {
            "127.0.0.1",
            "10.10.0.60",
            "10.10.1.15",
            "192.168.137.1"
        };
    }

    /// <summary>
    /// 用于定义限制条件
    /// </summary>
    public class GeneralRule
    {
        /// <summary>
        /// 规则应用于所有端点
        /// </summary>
        public string Endpoint { get; set; } = "*";

        /// <summary>
        /// 限制的时间周期为1秒
        /// </summary>
        public string Period { get; set; } = "1s";

        /// <summary>
        /// 在上述时间周期内，每个客户端可以发出的最大请求数为3
        /// </summary>
        public int Limit { get; set; } = 3;
    }

    /// <summary>
    /// 特定策略的设置
    /// </summary>
    public class IpRateLimitPoliciesInfo
    {
        public List<PolicyEntrie> PolicyEntries { get; set; } = new List<PolicyEntrie>();
    }

    /// <summary>
    /// 策略列表，每个策略可以定义不同的端点、时间段和限制
    /// </summary>
    public class PolicyEntrie
    {
        /// <summary>
        /// 指定策略应用的API端点。这可以是一个具体的API路径，如api/products，或者使用通配符来匹配多个端点，如api/*。
        /// </summary>
        public string Endpoint { get; set; } = "*";

        /// <summary>
        /// 定义限制的时间周期。这个值可以是1s（1秒）、1m（1分钟）、1h（1小时）等
        /// </summary>
        public string Period { get; set; } = "1s";

        /// <summary>
        /// 在指定的Period内允许的最大请求数。例如，如果Limit设置为5，Period设置为1m，则用户在1分钟内最多可以向该端点发送5个请求
        /// </summary>
        public int Limit { get; set; } = 3;
    }
}
