using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace XStudio.Common.Kafka
{
    /// <summary>
    /// 消费者
    /// </summary>
    public class KafkaConsumerEventHandler : IDistributedEventHandler<KafkaMessagePackage>, ITransientDependency
    {
        public Task HandleEventAsync(KafkaMessagePackage eventData)
        {
            Console.WriteLine("--------> App1 has received the message: " + eventData.Message.TruncateWithPostfix(32));
            Console.WriteLine();

            return Task.CompletedTask;
        }
    }
}
