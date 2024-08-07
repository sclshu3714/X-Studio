using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using XStudio.Permissions;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using static XStudio.Permissions.XStudioPermissions;

namespace XStudio.Projects
{
    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    [RemoteService(true)]
    [Authorize(XStudioPermissions.Projects.Default)]
    public class ProjectService :
    CrudAppService<
        Project, //The Book entity
        ProjectDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateProjectDto>, //Used to create/update a book
        IProjectService //implement the IBookAppService
    {
        private readonly IPermissionChecker _permissionChecker; 
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IdentityUserManager _userManager;
        public ProjectService(
            IRepository<Project, Guid> repository, 
            IPermissionChecker permissionChecker,
            IBackgroundJobManager backgroundJobManager,
            IdentityUserManager userManager)
            : base(repository)
        {
            GetPolicyName = XStudioPermissions.Projects.Default;
            GetListPolicyName = XStudioPermissions.Projects.Default;
            CreatePolicyName = XStudioPermissions.Projects.Create;
            UpdatePolicyName = XStudioPermissions.Projects.Edit;
            DeletePolicyName = XStudioPermissions.Projects.Delete;
            _permissionChecker = permissionChecker;
            _backgroundJobManager = backgroundJobManager;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public override async Task<ProjectDto> GetAsync(Guid id)
        {
            var queryable = await Repository.GetQueryableAsync();
            IQueryable<Project> query = queryable.Where(x => x.Id == id);
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Project), id);
            }
            var coursewareDto = ObjectMapper.Map<Project, ProjectDto>(queryResult);
            var queryableUser = await _userManager.FindByIdAsync($"{queryResult.AuthorId}");
            if (queryableUser == null)
            {
                throw new EntityNotFoundException(typeof(IUser), queryResult.AuthorId);
            }
            coursewareDto.AuthorName = queryableUser.Name;
            return coursewareDto;
        }

        [HttpPost] 
        public override async Task<ProjectDto> CreateAsync(CreateUpdateProjectDto input)
        {
            return await base.CreateAsync(input);
        }

        [HttpDelete]
        public override Task DeleteAsync(Guid id)
        {
            return base.DeleteAsync(id); 
        }

        [HttpPut]
        public override async Task<ProjectDto> UpdateAsync(Guid id, CreateUpdateProjectDto input)
        {
           return await base.UpdateAsync(id, input);
        }

        [HttpPost("list")]
        public override async Task<PagedResultDto<ProjectDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            PagedResultDto<ProjectDto> ProjectDtos = await base.GetListAsync(input);
            ProjectDtos.Items = await UpdateAuthorNames(ProjectDtos.Items.ToList());
            return ProjectDtos;
        }

        private async Task<List<ProjectDto>> UpdateAuthorNames(List<ProjectDto> projects)
        {
            foreach (var project in projects)
            {
                var queryableUser = await _userManager.FindByIdAsync($"{project.AuthorId}");
                if (queryableUser != null)
                {
                    project.AuthorName = queryableUser.Name;
                }
            }
            return projects;
        }


        private static string NormalizeSorting(string sorting)
        {
            if (sorting.IsNullOrEmpty())
            {
                return $"project.{nameof(Project.Name)}";
            }

            if (sorting.Contains("authorName", StringComparison.OrdinalIgnoreCase))
            {
                return sorting.Replace(
                    "authorName",
                    "author.Name",
                    StringComparison.OrdinalIgnoreCase
                );
            }

            return $"project.{sorting}";
        }
    }
}
