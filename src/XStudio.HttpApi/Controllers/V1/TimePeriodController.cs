using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using XStudio.Models;
using XStudio.Schools.Timetable;

namespace XStudio.Controllers.V1 {
    [Route("api/xstudio/[controller]/v{version:apiVersion}")]
    [ApiVersion(1.0)]
    [ApiController]
    public class TimePeriodController : AbpController {
        private readonly ITimePeriodService _TimePeriodService;
        public TimePeriodController(ITimePeriodService tmePeriodService) {
            _TimePeriodService = tmePeriodService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<TimePeriodDto>> CreateAsync(CreateTimePeriodDto input) {
            TimePeriodDto timePeriodDto = await _TimePeriodService.CreateAsync(input);
            return CommonResult<TimePeriodDto>.Success(timePeriodDto);
        }

        [HttpPost("adds")]
        public async Task<ActionResult<List<TimePeriodDto>>> InsertManyAsync(List<CreateTimePeriodDto> inputs) {
            List<TimePeriodDto> timePeriodDtos = await _TimePeriodService.InsertManyAsync(inputs);
            return (timePeriodDtos != null && timePeriodDtos.Any()) ?
                    CommonResult<List<TimePeriodDto>>.Success(timePeriodDtos) :
                    CommonResult<List<TimePeriodDto>>.NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(Guid id) {
            await _TimePeriodService.DeleteAsync(id);
            return CommonResult<bool>.Success(true);
        }

        [HttpDelete("deletes")]
        public async Task<ActionResult<bool>> DeleteManyAsync(List<Guid> ids) {
           await _TimePeriodService.DeleteManyAsync(ids);
           return new OkObjectResult(true);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TimePeriodDto>> GetAsync(Guid id) {
            return new OkObjectResult(await _TimePeriodService.GetAsync(id));
        }

        [HttpPost("list")]
        public async Task<ActionResult<PagedResultDto<TimePeriodDto>>> GetListAsync(PagedAndSortedResultRequestDto input) {
            PagedResultDto <TimePeriodDto> timePeriodDtos = await _TimePeriodService.GetListAsync(input);
            return (timePeriodDtos != null && timePeriodDtos.Items.Any()) ?
                    CommonResult<PagedResultDto<TimePeriodDto>>.Success(timePeriodDtos) :
                    CommonResult<PagedResultDto<TimePeriodDto>>.NoContent();
        }

        [HttpPut("update")]
        public async Task<ActionResult<TimePeriodDto>> UpdateAsync(Guid id, UpdateTimePeriodDto input) {
            return new OkObjectResult(await _TimePeriodService.UpdateAsync(id, input));
        }
    }
}
