using AutoMapper;
using XStudio.Clouds;
using XStudio.Projects;
using XStudio.Schools.Places;
using XStudio.Schools.Timetable;

namespace XStudio;

public class XStudioApplicationAutoMapperProfile : Profile
{
    public XStudioApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        // 时段
        CreateMap<TimePeriodDto, TimePeriod>().ReverseMap();
        CreateMap<UpdateTimePeriodDto, TimePeriod>().ReverseMap();
        CreateMap<CreateTimePeriodDto, TimePeriod>().ReverseMap();

        // 项目
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<CreateUpdateProjectDto, Project>().ReverseMap();
        // 学校场所
        CreateMap<School, SchoolDto>().ReverseMap();
        CreateMap<SchoolCampus, SchoolCampusDto>().ReverseMap();
        CreateMap<SchoolBuilding, SchoolBuildingDto>().ReverseMap();
        CreateMap<BuildingFloor, BuildingFloorDto>().ReverseMap();
        CreateMap<Classroom, ClassroomDto>().ReverseMap();

        CreateMap<CreateOrUpdateSchoolDto, School>().ReverseMap();
        CreateMap<UpdateSchoolCampusDto, SchoolCampus>().ReverseMap();
        CreateMap<UpdateSchoolBuildingDto, SchoolBuilding>().ReverseMap();
        CreateMap<UpdateBuildingFloorDto, BuildingFloor>().ReverseMap();
        CreateMap<UpdateClassroomDto, Classroom>().ReverseMap();

        // 节次方案
        CreateMap<Schedule, ScheduleDto>().ReverseMap();
        CreateMap<Schedule, UpdateScheduleDto>().ReverseMap();
        CreateMap<Schedule, CreateScheduleDto>().ReverseMap();

        // 腾讯云
        CreateMap<UploadFileDto, UploadFileResultDto>();
    }
}
