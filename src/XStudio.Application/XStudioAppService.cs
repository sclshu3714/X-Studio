using System;
using System.Collections.Generic;
using System.Text;
using XStudio.Localization;
using Volo.Abp.Application.Services;

namespace XStudio;

/* Inherit your application services from this class.
 */
public abstract class XStudioAppService : ApplicationService
{
    protected XStudioAppService()
    {
        LocalizationResource = typeof(XStudioResource);
    }
}
