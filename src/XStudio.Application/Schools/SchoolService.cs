using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

namespace XStudio.Schools
{
    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    [RemoteService(true)]
    [Authorize(Policy = XStudioPermissions.Schools.Default)]
    public class SchoolService :
        CrudAppService<
        School, //The Book entity
        SchoolDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        UpdateSchoolDto>, //Used to create/update a book
        ISchoolService //implement the IBookAppService
    {
        public SchoolService(IRepository<School, Guid> repository) 
            : base(repository)
        {
        }

        [HttpPost("Add")]
        public override Task<SchoolDto> CreateAsync(UpdateSchoolDto input)
        {
            return base.CreateAsync(input);
        }

        [HttpDelete("delete")]
        public override Task DeleteAsync(Guid id)
        {
            return base.DeleteAsync(id);
        }

        [HttpGet("{id}")]
        public override Task<SchoolDto> GetAsync(Guid id)
        {
            return base.GetAsync(id);
        }

        [HttpPost("list")]
        public override Task<PagedResultDto<SchoolDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return base.GetListAsync(input);
        }

        [HttpPut("update")]
        public override Task<SchoolDto> UpdateAsync(Guid id, UpdateSchoolDto input)
        {
            return base.UpdateAsync(id, input);
        }
    }
}
