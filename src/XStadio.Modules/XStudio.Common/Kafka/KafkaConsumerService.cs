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
    public class KafkaConsumerService<T> : ITransientDependency
    {
        private readonly IConsumer<Ignore, string> _consumer;

        public KafkaConsumerService(string bootstrapServers, string topic, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false, // 手动提交偏移量
                FetchMaxBytes = 10485760, // 增加拉取消息的批量大小
                SessionTimeoutMs = 60000, // 增加超时时间
                MaxPartitionFetchBytes = 10485760 // 增加每个分区的拉取大小
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe(topic);
        }

        public async Task StartConsuming(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    // 异步处理消息
                    await ProcessMessageAsync(consumeResult.Message.Value);

                    // 手动提交偏移量
                    _consumer.Commit(consumeResult);
                }
            }
            catch (OperationCanceledException)
            {
                // 处理取消操作
            }
            finally
            {
                _consumer.Close();
            }
        }

        private Task ProcessMessageAsync(string message)
        {
            // 模拟异步处理
            return Task.Run(() =>
            {
                Console.WriteLine($"Processing message: {message}");
                Thread.Sleep(100); // 模拟处理时间
            });
        }
    }
}
