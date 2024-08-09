using Microsoft.EntityFrameworkCore;
using ShardingCore.Extensions;
using ShardingCore.Sharding.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace XStudio.EntityFrameworkCore
{
    public abstract class AbstractShardingAbpDbContext<TDbContext> 
        : AbpDbContext<TDbContext>, IShardingDbContext where TDbContext : DbContext
    {
        private bool _createExecutor = false;
        protected AbstractShardingAbpDbContext(DbContextOptions<TDbContext> options) : base(options)
        {
        }


        private IShardingDbContextExecutor? _shardingDbContextExecutor;
        public IShardingDbContextExecutor? GetShardingExecutor()
        {
            if (!_createExecutor)
            {
                _shardingDbContextExecutor = this.DoCreateShardingDbContextExecutor();
                _createExecutor = true;
            }
            return _shardingDbContextExecutor;
        }

        private IShardingDbContextExecutor? DoCreateShardingDbContextExecutor()
        {
            var shardingDbContextExecutor = this.CreateShardingDbContextExecutor();
            if (shardingDbContextExecutor != null)
            {

                shardingDbContextExecutor.EntityCreateDbContextBefore += (sender, args) =>
                {
                    CheckAndSetShardingKeyThatSupportAutoCreate(args.Entity);
                };
                shardingDbContextExecutor.CreateDbContextAfter += (sender, args) =>
                {
                    var dbContext = args.DbContext;
                    if (dbContext is AbpDbContext<TDbContext> abpDbContext && abpDbContext.LazyServiceProvider == null)
                    {
                        abpDbContext.LazyServiceProvider = this.LazyServiceProvider;
                        if (dbContext is IAbpEfCoreDbContext abpEfCoreDbContext && this.UnitOfWorkManager.Current != null)
                        {
                            abpEfCoreDbContext.Initialize(
                                new AbpEfCoreDbContextInitializationContext(
                                    this.UnitOfWorkManager.Current
                                )
                            );
                        }
                    }
                };
            }
            return shardingDbContextExecutor;
        }


        private void CheckAndSetShardingKeyThatSupportAutoCreate<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity is IShardingKeyId)
            {

                if (entity is IEntity<Guid> guidEntity)
                {
                    if (guidEntity.Id != default)
                    {
                        return;
                    }
                    var idProperty = entity.GetObjectProperty(nameof(IEntity<Guid>.Id));

                    var dbGeneratedAttr = Volo.Abp.Reflection.ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                            idProperty
                        );

                    if (dbGeneratedAttr != null && dbGeneratedAttr.DatabaseGeneratedOption != DatabaseGeneratedOption.None)
                    {
                        return;
                    }

                    EntityHelper.TrySetId(
                        guidEntity,
                        () => GuidGenerator.Create(),
                        true
                    );
                }
            }
            else if (entity is IShardingKeyIsCreationTime)
            {
                AuditPropertySetter?.SetCreationProperties(entity);
            }
        }


        // /// <summary>
        // /// abp 5.x+ 如果存在并发字段那么需要添加这段代码
        // /// </summary>
        // protected override void HandlePropertiesBeforeSave()
        // {
        //     if (GetShardingExecutor() == null)
        //     {
        //         base.HandlePropertiesBeforeSave();
        //     }
        // }


        // /// <summary>
        // /// abp 4.x+ 如果存在并发字段那么需要添加这段代码
        // /// </summary>
        // /// <returns></returns>
        //
        // protected override void ApplyAbpConcepts(EntityEntry entry, EntityChangeReport changeReport)
        // {
        //     if (GetShardingExecutor() == null)
        //     {
        //         base.ApplyAbpConcepts(entry, changeReport);
        //     }
        // }

        public override void Dispose()
        {
            _shardingDbContextExecutor?.Dispose();
            base.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            if (_shardingDbContextExecutor != null)
            {
                await _shardingDbContextExecutor.DisposeAsync();
            }
            await base.DisposeAsync();
        }
    }
}
