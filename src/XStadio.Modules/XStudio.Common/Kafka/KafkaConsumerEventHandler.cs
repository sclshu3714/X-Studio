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
    public class KafkaConsumerEventHandler<T, TResult> : IDistributedEventHandler<MessagePackage<T>>, ITransientDependency where T : class
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public KafkaConsumerEventHandler(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }
        /// <summary>
        /// 消费到kafka消息后的回调处理事件
        /// </summary>
        public Func<MessagePackage<T>, Task<TResult>>? OnHandleEventAction { get; set; }

        /// <summary>
        /// 监听到生产者发送的消息
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandleEventAsync(MessagePackage<T> eventData)
        {
            Console.WriteLine("消费到消息: 截取部分展示 ->" + eventData.Value?.ToJson().TruncateWithPostfix(32));
            Log.Information($"消费到消息: {eventData.Value?.ToJson()}");
            await ConsumeMessageAsync(eventData);
        }

        public async Task ConsumeMessageAsync(MessagePackage<T> eventData)
        {
            if (eventData == null || eventData.Value == null)
            {
                Log.Warning("消息内容为空，无需处理。");
                return;
            }

            if (OnHandleEventAction == null)
            {
                Log.Warning("消息处理委托为空，无法处理消息。");
                return;
            }

            var retryPolicy = Policy
                .Handle<Exception>() //(ex => !(ex is SpecificExceptionTypeThatShouldNotRetry)) // 替换为具体不需要重试的异常类型  
                .RetryAsync(3, async (exception, retryCount) =>
                {
                    Log.Warning($"重试处理消息 (尝试 {retryCount}): {eventData.ToJson()}, 异常: {exception.Message}");
                    await Task.CompletedTask;
                });

            await retryPolicy.ExecuteAsync(async () =>
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    try
                    {
                        TResult result = await OnHandleEventAction(eventData);
                        Log.Information($"Kafka消息处理结果: {result?.ToJson()}");
                        await uow.CompleteAsync();
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"处理消息时发生错误，回滚事务: {ex.Message}");
                        await uow.RollbackAsync(); // 确保在出错时回滚事务  
                        throw; // 可选：重新抛出异常以便上层处理  
                    }
                }
            });
        }


        //public async Task ConsumeMessageAsync(MessagePackage<T> eventData)
        //{
        //    var retryPolicy = Policy
        //        .Handle<Exception>()
        //        .RetryAsync(3, onRetry: (exception, timeSpan) =>
        //        {
        //            Log.Warning($"重试处理消息: {eventData.ToJson()}, 异常: {exception.Message}");
        //        });
        //    await retryPolicy.ExecuteAsync(async () =>
        //    {
        //        using (var uow = _unitOfWorkManager.Begin())
        //        {
        //            // 处理消息
        //            if (OnHandleEventAction != null)
        //            {
        //                // 处理消息，返回处理结果
        //                object result =  await OnHandleEventAction.Invoke(eventData);
        //                Log.Information($"kafka消息处理结果: {result?.ToJson()}");
        //            }
        //            await uow.CompleteAsync();
        //        }
        //    });
        //}
    }
}
