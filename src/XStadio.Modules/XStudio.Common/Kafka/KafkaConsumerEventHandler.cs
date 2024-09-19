using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using XStudio.Common.Helper;

namespace XStudio.Common.Kafka
{
    /// <summary>
    /// 消费者
    /// </summary>
    public class KafkaConsumerEventHandler<T> : IDistributedEventHandler<MessagePackage<T>>, ITransientDependency where T : class
    {
        /// <summary>
        /// 消费到kafka消息后的回调处理事件
        /// </summary>
        public Action<MessagePackage<T>>? OnHandleEventAction { get; set; }
        /// <summary>
        /// 监听到生产者发送的消息
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public Task HandleEventAsync(MessagePackage<T> eventData)
        {
            Console.WriteLine("--------> App1 has received the message: " + eventData.Value?.ToJson().TruncateWithPostfix(32));
            Console.WriteLine();
            OnHandleEventAction?.Invoke(eventData);
            return Task.CompletedTask;
        }
    }
}
