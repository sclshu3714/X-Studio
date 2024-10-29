using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using XStudio.App.Models.Data;
using XStudio.App.ViewModel.Module;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XStudio.App.Common
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            //CreateMap<GetAuditLogsFilter, GetAuditLogsInput>().ReverseMap();
            //CreateMap<GetEntityChangeFilter, GetEntityChangeInput>().ReverseMap();
            //CreateMap<GetTenantsFilter, GetTenantsInput>().ReverseMap();
            //CreateMap<FlatPermissionWithLevelDto, PermissionModel>().ReverseMap();
            CreateMap<TimePeriodViewModel, TimePeriod>().ReverseMap();
        }
    }
}
