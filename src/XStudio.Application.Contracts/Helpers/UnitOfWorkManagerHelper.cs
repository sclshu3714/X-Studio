using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
