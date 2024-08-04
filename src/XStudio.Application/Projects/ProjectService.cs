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
        public ProjectService(IRepository<Project, Guid> repository, IPermissionChecker permissionChecker)
            : base(repository)
        {
            GetPolicyName = XStudioPermissions.Projects.Default;
            GetListPolicyName = XStudioPermissions.Projects.Default;
            CreatePolicyName = XStudioPermissions.Projects.Create;
            UpdatePolicyName = XStudioPermissions.Projects.Edit;
            DeletePolicyName = XStudioPermissions.Projects.Delete;
            _permissionChecker = permissionChecker;
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
            //var queryableUser = await _authorRepository.GetAsync(queryResult.AuthorId);
            //if (queryableUser == null)
            //{
            //    throw new EntityNotFoundException(typeof(IUser), queryResult.AuthorId);
            //}
            //coursewareDto.AuthorName = queryableUser.Name;
            return coursewareDto;
        }

        [HttpPost] 
        public override Task<ProjectDto> CreateAsync(CreateUpdateProjectDto input)
        {
            return base.CreateAsync(input);
        }

        [HttpDelete]
        public override Task DeleteAsync(Guid id) 
        { 
            return base.DeleteAsync(id); 
        }

        [HttpPut]
        public override Task<ProjectDto> UpdateAsync(Guid id, CreateUpdateProjectDto input)
        {
            return base.UpdateAsync(id, input);
        }

        [HttpPost("list")]
        public override Task<PagedResultDto<ProjectDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return base.GetListAsync(input);
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
