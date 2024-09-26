using AutoMapper;
using XStudio.Projects;
using XStudio.Schools.Places;

namespace XStudio;

public class XStudioApplicationAutoMapperProfile : Profile
{
    public XStudioApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
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
