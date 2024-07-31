using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace XStudio.Projects
{
    public interface IProjectService : ICrudAppService< //Defines CRUD methods
        ProjectDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateProjectDto> //Used to create/update a book
    {
        
    }
}
