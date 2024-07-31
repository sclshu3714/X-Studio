using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using XStudio.Projects;

namespace XStudio.Controllers.V2
{
    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(2.0)]
    [ApiController]
    public class ProjectController : AbpController
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<PagedResultDto<ProjectDto>> GetListAsync(PagedAndSortedResultRequestDto paged)
        {
            return  await _projectService.GetListAsync(paged);
        }
    }
}
