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
        CreateOrUpdateSchoolDto>, //Used to create/update a book
        ISchoolService //implement the IBookAppService
    {
        public SchoolService(IRepository<School, Guid> repository) 
            : base(repository)
        {
        }

        [HttpPost("Add")]
        public override Task<SchoolDto> CreateAsync(CreateOrUpdateSchoolDto input)
        {
            return base.CreateAsync(input);
        }

        [HttpPost("Adds")]
        public async Task<List<SchoolDto>> InsertManyAsync(List<CreateOrUpdateSchoolDto> inputs)
        {
            var entities = ObjectMapper.Map<List<CreateOrUpdateSchoolDto> ,List<School>>(inputs);
            await Repository.InsertManyAsync(entities);
            return ObjectMapper.Map<List<School>, List<SchoolDto>>(entities);
        }

        [HttpDelete("delete/{id}")]
        public override Task DeleteAsync(Guid id)
        {
            return base.DeleteAsync(id);
        }

        [HttpDelete("deletes")]
        public async Task DeleteManyAsync(List<Guid> ids)
        {
           List<School> schools = await (await Repository.GetQueryableAsync())
                                        .Where(x => ids.Contains(x.Id))
                                        .ToListAsync();
            schools.ForEach(s => { s.ValidState = Common.ValidStateType.D; });
            await Repository.DeleteManyAsync(schools);
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
        public override Task<SchoolDto> UpdateAsync(Guid id, CreateOrUpdateSchoolDto input)
        {
            return base.UpdateAsync(id, input);
        }
    }
}
