using AutoMapper;
using XStudio.Projects;

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
    }
}
