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
using XStudio.Common;
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

        /// <summary>
        /// 创建时间段.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult<CommonResult<TimePeriodDto>>> CreateAsync(CreateTimePeriodDto input) {
            TimePeriodDto timePeriodDto = await _TimePeriodService.CreateAsync(input);
            return CommonResult<TimePeriodDto>.Success(timePeriodDto);
        }

        /// <summary>
        /// 批量创建时间段.
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        [HttpPost("adds")]
        public async Task<ActionResult<CommonResult<List<TimePeriodDto>>>> InsertManyAsync(List<CreateTimePeriodDto> inputs) {
            List<TimePeriodDto> timePeriodDtos = await _TimePeriodService.InsertManyAsync(inputs);
            return (timePeriodDtos != null && timePeriodDtos.Any()) ?
                    CommonResult<List<TimePeriodDto>>.Success(timePeriodDtos) :
                    CommonResult<List<TimePeriodDto>>.NoContent();
        }

        /// <summary>
        /// 删除单个时间段.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<CommonResult<bool>>> DeleteAsync(Guid id) {
            await _TimePeriodService.DeleteAsync(id);
            return CommonResult<bool>.Success(true);
        }

        /// <summary>
        /// 删除多个时间段.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("deletes")]
        public async Task<ActionResult<bool>> DeleteManyAsync(List<Guid> ids) {
           await _TimePeriodService.DeleteManyAsync(ids);
           return new OkObjectResult(true);
        }

        /// <summary>
        /// 查询单个时间段.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TimePeriodDto>> GetAsync(Guid id) {
            return new OkObjectResult(await _TimePeriodService.GetAsync(id));
        }

        /// <summary>
        /// 分页获取时间段列表，前端分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ActionResult<CommonResult<PagedResultDto<TimePeriodDto>>>> GetListAsync(PagedAndSortedResultRequestDto input) {
            PagedResultDto<TimePeriodDto> timePeriodDtos = await _TimePeriodService.GetListAsync(input);
            return (timePeriodDtos != null && timePeriodDtos.Items.Any()) ?
                    CommonResult<PagedResultDto<TimePeriodDto>>.Success(timePeriodDtos):
                    CommonResult<PagedResultDto<TimePeriodDto>>.NoContent();
        }


        /// <summary>
        /// 分页获取时间段列表.后端分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("pagelist")]
        public async Task<ActionResult<CommonResult<BasePageModel<TimePeriodDto>>>> GetPageListAsync(QueryPageParam param) {
            string inputSort = string.Empty;
            if(param.SortItem != null && param.SortItem.Any()) {
                inputSort = string.Join(",", param.SortItem.Select(s => $"{s.Column} {(s.Asc ? "ASC" : "DESC") }"));
            }
            PagedAndSortedResultRequestDto input = new PagedAndSortedResultRequestDto() {
                MaxResultCount = param.Size,
                SkipCount = (param.Page - 1) * param.Size,
                Sorting = inputSort,
            };
            PagedResultDto<TimePeriodDto> timePeriodDtos = await _TimePeriodService.GetListAsync(input);
            if(timePeriodDtos != null && timePeriodDtos.Items.Any()) {
                BasePageModel<TimePeriodDto> pageModel = new BasePageModel<TimePeriodDto>(timePeriodDtos.Items.ToList(),
                                                                                          timePeriodDtos.Items.Count(), 
                                                                                          param.Page, 
                                                                                          timePeriodDtos.TotalCount, 
                                                                                          timePeriodDtos.TotalCount / param.Size + 1);
                return CommonResult<BasePageModel<TimePeriodDto>>.Success(pageModel);
            }
            return CommonResult<BasePageModel<TimePeriodDto>>.NoContent();
        }

        /// <summary>
        /// 更新时间段.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult<CommonResult<TimePeriodDto>>> UpdateAsync(Guid id, UpdateTimePeriodDto input) {
            TimePeriodDto timePeriodDto = await _TimePeriodService.UpdateAsync(id, input);
            return CommonResult<TimePeriodDto>.Success(timePeriodDto);
        }
    }
}
