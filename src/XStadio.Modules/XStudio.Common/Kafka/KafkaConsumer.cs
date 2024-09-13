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
    /// kafka消费者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KafkaConsumer<T> : ITransientDependency
    {
        private readonly IConsumer<string, T> _consumer;

        public KafkaConsumer(string bootstrapServers, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, T>(config).Build();
        }

        public void Consume(string topic, CancellationToken cancellationToken)
        {
            _consumer.Subscribe(topic);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = _consumer.Consume(cancellationToken);
                    // 处理消息
                    Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
        }
    }
}
