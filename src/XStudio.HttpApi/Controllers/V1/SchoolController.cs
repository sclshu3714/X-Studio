using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using XStudio.Projects;
using XStudio.Schools.Places;

namespace XStudio.Controllers.V1
{
    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class SchoolController : AbpController
    {
        private readonly ISchoolService _schoolService;
        private readonly ISchoolCampusService _schoolCampusService;
        public SchoolController(ISchoolService schoolService,
                                ISchoolCampusService schoolCampusService)
        {
            _schoolService = schoolService;
            _schoolCampusService = schoolCampusService;
        }

        /// <summary>
        /// 查询学校校区(仅校区，不包括)
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        //public async Task<IActionResult<List<SchoolCampusDto>>> GetSchoolCampusList(Guid schoolId)
        //{
        //    (await _schoolCampusService..get()).
        //}
    }
}
