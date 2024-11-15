using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using XStudio.Users;

namespace XStudio.Schools.Timetable {
    /// <summary>
    /// 课表聚合服务
    ///     1、学校
    ///     2、年级
    ///     3、班级
    ///     4、课表
    ///         4.1、节次
    ///         4.2、教师
    ///         4.3、课程
    ///         4.4、周次
    ///         4.5、节数
    ///         4.6、上课时间
    ///         4.7、上课地点
    /// </summary>
    public class ScheduleAggregateService : ApplicationService {
        private readonly ILogger<LoginAppService> _logger;
        private readonly IRepository<Schedule, Guid> _repository;
        public ScheduleAggregateService(IRepository<Schedule, Guid> repository) {
            _logger = NullLogger<LoginAppService>.Instance;
            _repository = repository;
        }
    }
}
