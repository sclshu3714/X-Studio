using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using XStudio.Schools.Places;
using XStudio.Schools.Timetable;

namespace XStudio.Controllers.V1 {
    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class ScheduleController : AbpController {
        private readonly IScheduleService _ScheduleService;
        public ScheduleController(IScheduleService scheduleService) {
            _ScheduleService = scheduleService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<ScheduleDto>> CreateAsync(CreateScheduleDto input) {
            AbpUnitOfWorkOptions options = new AbpUnitOfWorkOptions();
            using (var uow = UnitOfWorkManager.Begin(options)) {
                try {
                    ScheduleDto scheduleDto = await _ScheduleService.CreateAsync(input);
                    return new OkObjectResult(scheduleDto);
                }
                catch (Exception ex) {
                    uow.Dispose();// // 这里不需要显式回滚，因为ABP会在捕获到异常时自动回滚  // Rollback the transaction if an exception occurs
                    Log.Error(ex, ex.Message);
                    return new BadRequestObjectResult(new { Message = "添加失败", Details = ex.Message });
                }
            }
        }

        [HttpPost("adds")]
        public async Task<List<ScheduleDto>> InsertManyAsync(List<CreateScheduleDto> inputs) {
            AbpUnitOfWorkOptions options = new AbpUnitOfWorkOptions();
            //using (var uow = UnitOfWorkManager.Begin(options)) {
            //    try {
            //        List<PagedResultDto<ScheduleDto>> entities = await _ScheduleService.GetListAsync();
            //        await _ScheduleService.DeleteManyAsync(entities);
            //        entities = ObjectMapper.Map<List<CreateScheduleDto>, List<TimePeriod>>(inputs);
            //        await _ScheduleService(entities, autoSave: true);
            //        return ObjectMapper.Map<List<TimePeriod>, List<ScheduleDto>>(entities);
            //    }
            //    catch (Exception ex) {
            //        //await uow.RollbackAsync();//手动回滚
            //        uow.Dispose();// // 这里不需要显式回滚，因为ABP会在捕获到异常时自动回滚  // Rollback the transaction if an exception occurs
            //        throw new DbUpdateException("插入失败，已经回滚", ex);
            //    }
            //}
            return null;
        }

        //[HttpDelete("delete/{id}")]
        //public override Task DeleteAsync(Guid id) {
        //    return base.DeleteAsync(id);
        //}

        //[HttpDelete("deletes")]
        //public async Task DeleteManyAsync(List<Guid> ids) {
        //    List<TimePeriod> schools = await (await Repository.GetQueryableAsync())
        //                                 .Where(x => ids.Contains(x.Id))
        //                                 .ToListAsync();
        //    schools.ForEach(s => { s.ValidState = Common.ValidStateType.D; });
        //    await Repository.DeleteManyAsync(schools);
        //}

        //[HttpGet("{id}")]
        //public override async Task<TimePeriodDto> GetAsync(Guid id) {
        //    return await base.GetAsync(id);
        //}

        //[HttpPost("list")]
        //public override async Task<PagedResultDto<TimePeriodDto>> GetListAsync(PagedAndSortedResultRequestDto input) {
        //    return await base.GetListAsync(input);
        //}

        //[HttpPut("update")]
        //public override async Task<TimePeriodDto> UpdateAsync(Guid id, UpdateTimePeriodDto input) {
        //    //return await base.UpdateAsync(id, input);
        //    AbpUnitOfWorkOptions options = new AbpUnitOfWorkOptions();
        //    using (var uow = UnitOfWorkManager.Begin(options)) {
        //        try {
        //            var timePeriod = await Repository.GetAsync(id);
        //            ObjectMapper.Map(input, timePeriod); // Update project with input data
        //            await Repository.UpdateAsync(timePeriod);
        //            await uow.CompleteAsync(); // Commit the transaction if everything is successful
        //            return ObjectMapper.Map<TimePeriod, TimePeriodDto>(timePeriod);
        //        }
        //        catch (Exception ex) {
        //            //await uow.RollbackAsync();//手动回滚
        //            uow.Dispose();// // 这里不需要显式回滚，因为ABP会在捕获到异常时自动回滚  // Rollback the transaction if an exception occurs
        //            throw new DbUpdateException("更新失败，已经回滚", ex);
        //        }
        //    }
        //}
    }
}
