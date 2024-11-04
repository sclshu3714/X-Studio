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
            ScheduleDto scheduleDto = await _ScheduleService.CreateAsync(input);
            return new OkObjectResult(scheduleDto);
        }

        [HttpPost("adds")]
        public async Task<ActionResult<List<ScheduleDto>>> InsertManyAsync(List<CreateScheduleDto> inputs) {
            List<ScheduleDto> schedules = await _ScheduleService.InsertManyAsync(inputs);
            return new OkObjectResult(schedules);
        }

        [HttpDelete("delete/{id}")]
        public async Task DeleteAsync(Guid id) {
            await _ScheduleService.DeleteAsync(id);
        }

        [HttpDelete("deletes")]
        public async Task DeleteManyAsync(List<Guid> ids) {
            await _ScheduleService.DeleteManyAsync(ids);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDto>> GetAsync(Guid id) {
            ScheduleDto scheduleDto = await _ScheduleService.GetAsync(id);
            return new OkObjectResult(scheduleDto);
        }

        [HttpPost("list")]
        public async Task<ActionResult<PagedResultDto<ScheduleDto>>> GetListAsync(PagedAndSortedResultRequestDto input) {
            PagedResultDto<ScheduleDto> scheduleDto = await _ScheduleService.GetListAsync(input);
            return new OkObjectResult(scheduleDto);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ScheduleDto>> UpdateAsync(Guid id, UpdateScheduleDto input) {
            ScheduleDto scheduleDto = await _ScheduleService.UpdateAsync(id, input);
            return new OkObjectResult(scheduleDto);
        }
    }
}
