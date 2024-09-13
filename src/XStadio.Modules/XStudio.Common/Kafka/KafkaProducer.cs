using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace XStudio.Common.Kafka
{
    /// <summary>
    /// kafka生产者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KafkaProducer<T> : ITransientDependency
    {
        private readonly IProducer<string, T> _producer;

        public KafkaProducer(string bootstrapServers)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            _producer = new ProducerBuilder<string, T>(config).Build();
        }

        public async Task SendMessageAsync(string topic, string key, T value)
        {
            await _producer.ProduceAsync(topic, new Message<string, T> { Key = key, Value = value });
        }
    }
}
