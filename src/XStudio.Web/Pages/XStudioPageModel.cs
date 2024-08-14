using XStudio.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace XStudio.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class XStudioPageModel : AbpPageModel
{
    protected XStudioPageModel()
    {
        LocalizationResourceType = typeof(XStudioResource);
    }
}
