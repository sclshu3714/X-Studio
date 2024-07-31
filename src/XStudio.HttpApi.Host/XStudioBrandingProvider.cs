using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace XStudio;

[Dependency(ReplaceServices = true)]
public class XStudioBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "XStudio";
}
