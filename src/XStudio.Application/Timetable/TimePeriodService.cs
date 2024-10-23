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
using XStudio.Permissions;
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
        public override Task<TimePeriodDto> CreateAsync(CreateTimePeriodDto input) {
            return base.CreateAsync(input);
        }

        [HttpPost("adds")]
        public async Task<List<TimePeriodDto>> InsertManyAsync(List<CreateTimePeriodDto> inputs) {
            var entities = ObjectMapper.Map<List<CreateTimePeriodDto>, List<TimePeriod>>(inputs);
            await Repository.InsertManyAsync(entities);
            return ObjectMapper.Map<List<TimePeriod>, List<TimePeriodDto>>(entities);
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
        public override Task<TimePeriodDto> GetAsync(Guid id) {
            return base.GetAsync(id);
        }

        [HttpPost("list")]
        public override Task<PagedResultDto<TimePeriodDto>> GetListAsync(PagedAndSortedResultRequestDto input) {
            return base.GetListAsync(input);
        }

        [HttpPut("update")]
        public override Task<TimePeriodDto> UpdateAsync(Guid id, UpdateTimePeriodDto input) {
            return base.UpdateAsync(id, input);
        }
    }
}
