using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace XStudio.Web;

[Dependency(ReplaceServices = true)]
public class XStudioBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "XStudio";
}
