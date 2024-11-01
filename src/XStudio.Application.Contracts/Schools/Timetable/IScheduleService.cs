using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace XStudio.Schools.Timetable {
    public interface IScheduleService : ICrudAppService< //Defines CRUD methods
        ScheduleDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateScheduleDto,
        UpdateScheduleDto> //Used to create/update a book
    {
         Task<List<ScheduleDto>> InsertManyAsync(List<CreateScheduleDto> inputs);
         Task<bool> DeleteManyAsync(List<Guid> ids);
    }
}
