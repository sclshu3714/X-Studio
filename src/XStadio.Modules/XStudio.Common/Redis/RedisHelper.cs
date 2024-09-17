using Castle.Components.DictionaryAdapter;
using Grpc.Core;
using Nacos.V2.Naming.Dtos;
using Serilog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using XStudio.Common.Helper;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XStudio.Common.Redis
{
    public class RedisHelper
    {
        private static ConnectionMultiplexer? Instance { get; set; }
        private static IDatabase? _redisDatabase { get; set; }
        private static IServer? _redisServer { get; set; }
        private static object locker = new object();

        public RedisHelper()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        public static async Task<bool> Initialization(string connectionString, int databaseId = -1)
        {
            if (Instance == null)
            {
                lock (locker)
                {
                    if (Instance == null || !Instance.IsConnected)
                    {
                        Instance = ConnectionMultiplexer.Connect(connectionString);
                        _redisDatabase = Instance.GetDatabase(databaseId);
                        _redisServer = Instance.GetServer(Instance.GetEndPoints()[0]);
                    }
                }
            }
            //注册如下事件
            Instance.ConnectionFailed += MuxerConnectionFailed;
            Instance.ConnectionRestored += MuxerConnectionRestored;
            Instance.ErrorMessage += MuxerErrorMessage;
            Instance.HashSlotMoved += MuxerHashSlotMoved;
            Instance.InternalError += MuxerInternalError;
            await Task.CompletedTask;
            return true;
        }

        #region 事件

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object? sender, ConnectionFailedEventArgs e)
        {
            Log.Error($"Redis日志 => 重新连接：Endpoint failed: {e.EndPoint},{e.FailureType},{(e.Exception == null ? "" : (", " + e.Exception.Message))}");
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object? sender, ConnectionFailedEventArgs e)
        {
            Log.Error($"Redis日志 => ConnectionRestored: {e.EndPoint}");
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object? sender, RedisErrorEventArgs e)
        {
            Log.Error($"Redis日志 => ErrorMessage: " + e.Message);
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object? sender, HashSlotMovedEventArgs e)
        {
            Log.Error($"Redis日志 => HashSlotMoved:NewEndPoint {e.NewEndPoint}, OldEndPoint {e.OldEndPoint}");
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object? sender, InternalErrorEventArgs e)
        {
            Log.Error($"Redis日志 => InternalError:Message {e.Exception.Message}");
        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Instance?.Close();
        }

        /// <summary>
        /// 异步设置键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> SetAsync(string key, object value, TimeSpan? expiry = null)
        {
            if (_redisDatabase == null) return default;
            return await _redisDatabase.StringSetAsync(key, value.ToString(), expiry);
        }

        /// <summary>
        /// 异步获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T?> GetAsync<T>(string key)
        {
            if (_redisDatabase == null) return default;
            var value = await _redisDatabase.StringGetAsync(key);
            return value.IsNullOrEmpty ? default(T) : (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 异步删除键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            if (_redisDatabase == null) return default;
            return await _redisDatabase.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 异步判断键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            if (_redisDatabase == null) return default;
            return await _redisDatabase.KeyExistsAsync(key);
        }

        /// <summary>
        /// 异步获取所有匹配特定模式的键
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetAllKeysAsync(int database = -1, RedisValue pattern = default, int pageSize = 100, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            if (_redisServer == null)
            { 
                return new List<string>();
            }
            var keys = new List<string>();
            IAsyncEnumerable<RedisKey> scanResult = _redisServer.KeysAsync(database, pattern, pageSize, cursor, pageOffset, flags);
            await foreach (var item in scanResult)
            {
                keys.Add(item.ToString());
            }
            return keys;
        }

        /// <summary>
        /// 异步设置哈希
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync(string key, string hashField, object value)
        {
            if (_redisDatabase == null) return default;
            return await _redisDatabase.HashSetAsync(key, hashField, value.ToString());
        }

        /// <summary>
        /// 异步获取哈希值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<T?> HashGetAsync<T>(string key, string hashField)
        {
            if (_redisDatabase == null) return default;
            var value = await _redisDatabase.HashGetAsync(key, hashField);
            return value.IsNullOrEmpty ? default(T) : (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 异步删除哈希字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<bool> HashDeleteAsync(string key, string hashField)
        {
            if (_redisDatabase == null) return default;
            return await _redisDatabase.HashDeleteAsync(key, hashField);
        }

        #region  当作消息代理中间件使用 一般使用更专业的消息队列来处理这种业务场景
        /// <summary>
        /// 当作消息代理中间件使用
        /// 消息组建中,重要的概念便是生产者,消费者,消息中间件。
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static long Publish(string channel, string message)
        {
            if (Instance == null || string.IsNullOrWhiteSpace(message))
            {
                return -1;
            }
            ISubscriber sub = Instance.GetSubscriber();
            string jsonStr = message.ToJson();
            RedisChannel theChannel = RedisChannel.Literal(channel);
            return sub.Publish(theChannel, message);
        }

        /// <summary>
        /// 在消费者端得到该消息并输出
        /// </summary>
        /// <param name="channelFrom"></param>
        /// <returns></returns>
        public static void Subscribe(string channelFrom)
        {
            if (Instance == null) return;
            ISubscriber sub = Instance.GetSubscriber();
            RedisChannel theChannel = RedisChannel.Literal(channelFrom);
            sub.Subscribe(theChannel, (channel, message) =>
            {
                Console.WriteLine((string)message);
            });
        }
        #endregion
    }
}
