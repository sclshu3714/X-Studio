using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using XStudio.Projects;

namespace XStudio.Controllers.V3
{
    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(3.0)]
    [ApiController]
    public class ProjectController : AbpController
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpGet]
        public async Task<ProjectDto> GetAsync(Guid id)
        {
            return await _projectService.GetAsync(id);
        }
    }
}
