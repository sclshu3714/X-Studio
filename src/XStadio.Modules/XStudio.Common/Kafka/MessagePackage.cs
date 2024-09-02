using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Volo.Abp.EventBus;

namespace XStudio.Common.Kafka
{
    [EventName("XStudio.Kafka.Message")]
    public class MessagePackage<T> where T : class
    {
        public string Topic { get; set; } = string.Empty;       // 消息主题
        public string  Title {get;set;} = string.Empty;         //消费标题
        public string Key { get; set; } = string.Empty;         // 消息键
        public T? Value { get; set; }                           // 消息内容
        public long Offset { get; set; } = 0;                   // 消息偏移量
        public DateTime Timestamp { get; set; } = DateTime.Now; // 消息时间戳

        public MessagePackage(T value)
        {
            Value = value;
        }
    }
}
