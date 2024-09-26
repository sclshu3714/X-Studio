using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace XStudio.Schools.Places
{
    public interface ISchoolCampusService : ICrudAppService< //Defines CRUD methods
        SchoolCampusDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        UpdateSchoolCampusDto> //Used to create/update a book
    {

    }
}
