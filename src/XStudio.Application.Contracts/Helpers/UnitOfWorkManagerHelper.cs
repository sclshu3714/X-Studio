using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using XStudio.Schools.Timetable;

namespace XStudio.Helpers
{
    public static class UnitOfWorkManagerHelper
    {
        public static void WithUnitOfWork(this Action action, IAbpLazyServiceProvider LazyServiceProvider, AbpUnitOfWorkOptions? options)
        {
            using (var scope = LazyServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                if(options == null) options = new AbpUnitOfWorkOptions();
                using (var uow = uowManager.Begin(options))
                {
                    action();

                    uow.CompleteAsync();
                }
            }
        }
        public static async Task<T> ExecuteAsync<T>(this Func<Task<T>> func, UnitOfWorkManager manager) {
            try {
                AbpUnitOfWorkOptions options = new AbpUnitOfWorkOptions();
                using (var uow = manager.Begin(options)) {
                    try {
                        return await func();
                    }
                    catch (Exception ex) {
                        uow.Dispose();// 这里不需要显式回滚，因为ABP会在捕获到异常时自动回滚  // Rollback the transaction if an exception occurs
                        throw new InvalidOperationException(ex.Message, ex);
                    }
                }
            }
            catch (HttpRequestException e) {
                // 处理请求异常
                Console.WriteLine($"请求错误: {e.Message}");
                throw new HttpRequestException($"{e.Message}", e); // 可重新抛出异常或进行其他处理
            }
            catch (InvalidOperationException e) {
                // 处理无效操作异常，例如没有正确的serialization设置
                Console.WriteLine($"无效操作错误: {e.Message}");
                throw new InvalidOperationException($"{e.Message}", e); // 可重新抛出异常或进行其他处理
            }
            catch (Exception e) {
                // 处理其他未知异常
                Console.WriteLine($"发生错误: {e.Message}");
                throw new InvalidOperationException($"{e.Message}", e); // 可重新抛出异常或进行其他处理
            }
        }
    }
}
