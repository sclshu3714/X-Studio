using XStudio.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace XStudio.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class XStudioController : AbpControllerBase
{
    protected XStudioController()
    {
        LocalizationResource = typeof(XStudioResource);
    }
}
