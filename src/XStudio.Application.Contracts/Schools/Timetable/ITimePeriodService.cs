using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using XStudio.Schools.Places;

namespace XStudio.Schools.Timetable {
    public interface ITimePeriodService : ICrudAppService< //Defines CRUD methods
        TimePeriodDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateTimePeriodDto,
        UpdateTimePeriodDto> //Used to create/update a book
    {

    }
}
