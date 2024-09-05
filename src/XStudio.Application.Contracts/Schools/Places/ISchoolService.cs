using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using XStudio.Projects;

namespace XStudio.Schools.Places
{
    public interface ISchoolService : ICrudAppService< //Defines CRUD methods
        SchoolDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        UpdateSchoolDto> //Used to create/update a book
    {
        
    }
}
