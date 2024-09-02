using Microsoft.AspNetCore.Mvc.Diagnostics;
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
    /// 生产者
    /// </summary>
    public class KafkaProducerEventHandler<T> 
        : IDistributedEventHandler<MessagePackage<T>>, 
        ITransientDependency where T : class
    {
        private readonly IDistributedEventBus _distributedEventBus;

        public KafkaProducerEventHandler(IDistributedEventBus distributedEventBus)
        {
            _distributedEventBus = distributedEventBus;
        }

        public async Task HandleEventAsync(MessagePackage<T> eventData)
        {
            Console.WriteLine("************************ INCOMING MESSAGE ****************************");
            //Console.WriteLine(eventData.Message);
            Console.WriteLine("**********************************************************************");
            Console.WriteLine();

            //await _distributedEventBus.PublishAsync(new ProducerMessagePackage(eventData.Message));
            await Task.CompletedTask;
        }

        public async Task SendMessage(T Message)
        {
            await _distributedEventBus.PublishAsync(new MessagePackage<T>(Message));
        }
    }
}
