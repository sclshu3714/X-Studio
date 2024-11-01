using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using XStudio.Permissions;
using XStudio.Schools.Timetable;

namespace XStudio.Timetable {
    [Authorize(Policy = XStudioPermissions.TimePeriods.Default)]
    [RemoteService(false)]
    public class ScheduleService
        : CrudAppService<
        Schedule, //The Book entity
        ScheduleDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateScheduleDto,
        UpdateScheduleDto>, //Used to create/update a book
        IScheduleService //implement the IBookAppService
                        {
        public ScheduleService(IRepository<Schedule, Guid> repository) 
            : base(repository) {
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        /// <exception cref="DbUpdateException"></exception>
        public async Task<List<ScheduleDto>> InsertManyAsync(List<CreateScheduleDto> inputs) {
            //var entities = ObjectMapper.Map<List<CreateTimePeriodDto>, List<TimePeriod>>(inputs);
            //await Repository.InsertManyAsync(entities, autoSave: true);
            //return ObjectMapper.Map<List<TimePeriod>, List<TimePeriodDto>>(entities);
            AbpUnitOfWorkOptions options = new AbpUnitOfWorkOptions();
            using (var uow = UnitOfWorkManager.Begin(options)) {
                try {
                    List<Schedule> entities = await Repository.GetListAsync();
                    await Repository.DeleteManyAsync(entities);
                    entities = ObjectMapper.Map<List<CreateScheduleDto>, List<Schedule>>(inputs);
                    await Repository.InsertManyAsync(entities, autoSave: true);
                    return ObjectMapper.Map<List<Schedule>, List<ScheduleDto>>(entities);
                }
                catch (Exception ex) {
                    //await uow.RollbackAsync();//手动回滚
                    uow.Dispose();// // 这里不需要显式回滚，因为ABP会在捕获到异常时自动回滚  // Rollback the transaction if an exception occurs
                    throw new DbUpdateException("插入失败，已经回滚", ex);
                }
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteManyAsync(List<Guid> ids) {
            List<Schedule> schools = await (await Repository.GetQueryableAsync())
                                         .Where(x => ids.Contains(x.Id))
                                         .ToListAsync();
            schools.ForEach(s => { s.ValidState = Common.ValidStateType.D; });
            await Repository.DeleteManyAsync(schools);
            return true;
        }
    }
}
