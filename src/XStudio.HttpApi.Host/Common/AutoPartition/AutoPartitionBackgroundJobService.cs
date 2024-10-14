using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using XStudio.EntityFrameworkCore;

namespace XStudio.Common
{
    public static class UseAutoPartition
    {
        public static void UseAutoPartitionBackgroundJob(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddHostedService<AutoPartitionBackgroundJobService>();
            });
        }

        public static void UseAutoPartitionBackgroundJob(this IServiceCollection services) {
            services.AddHostedService<AutoPartitionBackgroundJobService>();
        }
    }

    /// <summary>
    /// 数据库分区后台任务
    /// 说明:
    ///     当前执行MySql方式,达梦数据库还没有测试
    /// </summary>
    public class AutoPartitionBackgroundJobService : IHostedService, ITransientDependency
    {
        private readonly ILogger<AutoPartitionBackgroundJobService> _logger;
        private Timer? _timer;
        private XStudioDbContext _dbContext;
        public const long MAX_PARTITION_SIZE = 50000;

        public AutoPartitionBackgroundJobService(ILogger<AutoPartitionBackgroundJobService> logger,
                                                 XStudioDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // 每月的第一天运行
            var firstRun = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(1);
            var delay = firstRun - DateTime.UtcNow;

            _timer = new Timer(DoWork, null, delay, TimeSpan.FromDays(30)); // 每30天执行一次
            return Task.CompletedTask;
        }

        /// <summary>
        /// 每月执行一次，当数据大于50000时，分区
        /// </summary>
        /// <param name="state"></param>
        private async void DoWork(object? state)
        {
            try
            {
                // 遍历查询所有数据库，检查到数据库数据量大于5万条数据，执行分区
                var tables = await GetAllTablesAsync(); // 获取所有表名
                foreach (var table in tables)
                {
                    if (!table.StartsWith(XStudioConsts.DbTablePrefix))
                    { //不是项目表，不处理
                        continue;
                    }
                    var count = await GetRecordCountAsync(table); // 获取记录数
                    if (count > MAX_PARTITION_SIZE)
                    {
                        await AddMonthlyPartitionAsync(table); // 调用添加分区的方法
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding monthly partition.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 查询所有表格名称
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetAllTablesAsync()
        {
            var tables = new List<string>();
            var sql = "SHOW TABLES;"; // MySQL 查询所有表名
            return await _dbContext.Database.SqlQueryRaw<string>(sql)
                                   .ToListAsync();
            //using (var connection = _dbContext.Database.GetDbConnection())
            //{
            //    await connection.OpenAsync();
            //    using (var command = connection.CreateCommand())
            //    {
            //        command.CommandText = sql;
            //        using (var reader = await command.ExecuteReaderAsync())
            //        {
            //            while (await reader.ReadAsync())
            //            {
            //                tables.Add(reader.GetString(0)); // 获取表名
            //            }
            //        }
            //    }
            //}

            //return tables;
        }

        private async Task<int> GetRecordCountAsync(string tableName)
        {
            var sql = $"SELECT COUNT(*) FROM {tableName};"; // 查询记录数
            var count = await _dbContext.Database.ExecuteSqlRawAsync(sql);
            return count; // 返回记录数
        }

        /// <summary>
        /// 分区
        /// </summary>
        /// <returns></returns>
        private async Task AddMonthlyPartitionAsync(string tableName)
        {
            var nextMonth = DateTime.UtcNow.AddMonths(1);
            var partitionName = $"p{nextMonth:yyyyMM}";

            var sql = $"ALTER TABLE {tableName} ADD PARTITION (PARTITION {partitionName} VALUES LESS THAN ({nextMonth:yyyyMM}));";

            // 执行 SQL 语句（使用您的数据库上下文）
            await _dbContext.Database.ExecuteSqlRawAsync(sql);

            _logger.LogInformation($"Added partition: {partitionName}");
        }
    }
}
