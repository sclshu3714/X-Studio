using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace XStudio.Common.Kafka
{
    public class KafkaProducerEventHandler : IDistributedEventHandler<KafkaMessagePackage>, ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;

        public KafkaProducerEventHandler(IDistributedEventBus distributedEventBus)
        {
            _distributedEventBus = distributedEventBus;
        }

        public async Task HandleEventAsync(KafkaMessagePackage eventData)
        {
            Console.WriteLine("************************ INCOMING MESSAGE ****************************");
            Console.WriteLine(eventData.Message);
            Console.WriteLine("**********************************************************************");
            Console.WriteLine();

            await _distributedEventBus.PublishAsync(new KafkaMessagePackage(eventData.Message));
        }
    }
}
