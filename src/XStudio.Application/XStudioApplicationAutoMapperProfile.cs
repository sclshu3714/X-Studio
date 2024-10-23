using AutoMapper;
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
        CreateMap<TimePeriodDto, TimePeriod>();
        CreateMap<UpdateTimePeriodDto, TimePeriod>();
        CreateMap<CreateTimePeriodDto, TimePeriod>();
        CreateMap<TimePeriod, TimePeriodDto>();
        CreateMap<TimePeriod, UpdateTimePeriodDto>();
        CreateMap<TimePeriod, CreateTimePeriodDto>();

        // 项目
        CreateMap<Project, ProjectDto>();
        CreateMap<CreateUpdateProjectDto, Project>();
        // 学校场所
        CreateMap<School, SchoolDto>();
        CreateMap<SchoolCampus, SchoolCampusDto>();
        CreateMap<SchoolBuilding, SchoolBuildingDto>();
        CreateMap<BuildingFloor, BuildingFloorDto>();
        CreateMap<Classroom, ClassroomDto>();

        CreateMap<CreateOrUpdateSchoolDto, School>();
        CreateMap<UpdateSchoolCampusDto, SchoolCampus>();
        CreateMap<UpdateSchoolBuildingDto, SchoolBuilding>();
        CreateMap<UpdateBuildingFloorDto, BuildingFloor>();
        CreateMap<UpdateClassroomDto, Classroom>();
    }
}
