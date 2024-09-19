using Polly;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;
using XStudio.Common.Helper;

namespace XStudio.Common.Kafka
{
    /// <summary>
    /// 消费者
    /// </summary>
    public class KafkaConsumerEventHandler<T> : IDistributedEventHandler<MessagePackage<T>>, ITransientDependency where T : class
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public KafkaConsumerEventHandler(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }
        /// <summary>
        /// 消费到kafka消息后的回调处理事件
        /// </summary>
        public Func<MessagePackage<T>, Task>? OnHandleEventAction { get; set; }

        /// <summary>
        /// 监听到生产者发送的消息
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandleEventAsync(MessagePackage<T> eventData)
        {
            Console.WriteLine("--------> App1 has received the message: " + eventData.Value?.ToJson().TruncateWithPostfix(32));
            Console.WriteLine();
            await ConsumeMessageAsync(eventData);
        }

        public async Task ConsumeMessageAsync(MessagePackage<T> eventData)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .RetryAsync(3, onRetry: (exception, timeSpan) =>
                {
                    Log.Warning($"重试处理消息: {eventData.ToJson()}, 异常: {exception.Message}");
                });

            await retryPolicy.ExecuteAsync(async () =>
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    // 处理消息
                    if (OnHandleEventAction != null)
                    {
                        await OnHandleEventAction(eventData);
                    }
                    await uow.CompleteAsync();
                }
            });
        }
    }
}
