using Asp.Versioning;
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
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
using XStudio.Permissions;
using XStudio.Projects;
using XStudio.Schools.Places;
using XStudio.Schools.Timetable;

namespace XStudio.Timetable {

    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    [RemoteService(true)]
    [Authorize(Policy = XStudioPermissions.TimePeriods.Default)]
    public class TimePeriodService :
        CrudAppService<
        TimePeriod, //The Book entity
        TimePeriodDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateTimePeriodDto,
        UpdateTimePeriodDto>, //Used to create/update a book
        ITimePeriodService //implement the IBookAppService
    {
        public TimePeriodService(IRepository<TimePeriod, Guid> repository) 
            : base(repository) {

        }

        [HttpPost("add")]
        public override async Task<TimePeriodDto> CreateAsync(CreateTimePeriodDto input) {
            //return await base.CreateAsync(input);
            AbpUnitOfWorkOptions options = new AbpUnitOfWorkOptions();
            using (var uow = UnitOfWorkManager.Begin(options)) {
                try {
                    return await base.CreateAsync(input);
                }
                catch (Exception ex) {
                    //await uow.RollbackAsync();//手动回滚
                    uow.Dispose();// // 这里不需要显式回滚，因为ABP会在捕获到异常时自动回滚  // Rollback the transaction if an exception occurs
                    throw new DbUpdateException("添加失败，已经回滚", ex);
                }
            }
        }

        [HttpPost("adds")]
        public async Task<List<TimePeriodDto>> InsertManyAsync(List<CreateTimePeriodDto> inputs) {
            //var entities = ObjectMapper.Map<List<CreateTimePeriodDto>, List<TimePeriod>>(inputs);
            //await Repository.InsertManyAsync(entities, autoSave: true);
            //return ObjectMapper.Map<List<TimePeriod>, List<TimePeriodDto>>(entities);
            AbpUnitOfWorkOptions options = new AbpUnitOfWorkOptions();
            using (var uow = UnitOfWorkManager.Begin(options)) {
                try {
                    List<TimePeriod> entities = await Repository.GetListAsync();
                    await Repository.DeleteManyAsync(entities);
                    entities = ObjectMapper.Map<List<CreateTimePeriodDto>, List<TimePeriod>>(inputs);
                    await Repository.InsertManyAsync(entities, autoSave: true);
                    return ObjectMapper.Map<List<TimePeriod>, List<TimePeriodDto>>(entities);
                }
                catch (Exception ex) {
                    //await uow.RollbackAsync();//手动回滚
                    uow.Dispose();// // 这里不需要显式回滚，因为ABP会在捕获到异常时自动回滚  // Rollback the transaction if an exception occurs
                    throw new DbUpdateException("插入失败，已经回滚", ex);
                }
            }
        }

        [HttpDelete("delete/{id}")]
        public override Task DeleteAsync(Guid id) {
            return base.DeleteAsync(id);
        }

        [HttpDelete("deletes")]
        public async Task DeleteManyAsync(List<Guid> ids) {
            List<TimePeriod> schools = await (await Repository.GetQueryableAsync())
                                         .Where(x => ids.Contains(x.Id))
                                         .ToListAsync();
            schools.ForEach(s => { s.ValidState = Common.ValidStateType.D; });
            await Repository.DeleteManyAsync(schools);
        }

        [HttpGet("{id}")]
        public override async Task<TimePeriodDto> GetAsync(Guid id) {
            return await base.GetAsync(id);
        }

        [HttpPost("list")]
        public override async Task<PagedResultDto<TimePeriodDto>> GetListAsync(PagedAndSortedResultRequestDto input) {
            return await base.GetListAsync(input);
        }

        [HttpPut("update")]
        public override async Task<TimePeriodDto> UpdateAsync(Guid id, UpdateTimePeriodDto input) {
            //return await base.UpdateAsync(id, input);
            AbpUnitOfWorkOptions options = new AbpUnitOfWorkOptions();
            using (var uow = UnitOfWorkManager.Begin(options)) {
                try {
                    var timePeriod = await Repository.GetAsync(id);
                    ObjectMapper.Map(input, timePeriod); // Update project with input data
                    await Repository.UpdateAsync(timePeriod);
                    await uow.CompleteAsync(); // Commit the transaction if everything is successful
                    return ObjectMapper.Map<TimePeriod, TimePeriodDto>(timePeriod);
                }
                catch (Exception ex) {
                    //await uow.RollbackAsync();//手动回滚
                    uow.Dispose();// // 这里不需要显式回滚，因为ABP会在捕获到异常时自动回滚  // Rollback the transaction if an exception occurs
                    throw new DbUpdateException("更新失败，已经回滚", ex);
                }
            }
        }
    }
}
