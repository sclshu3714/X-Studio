using Volo.Abp.Modularity;

namespace XStudio;

[DependsOn(
    typeof(XStudioApplicationModule),
    typeof(XStudioDomainTestModule)
)]
public class XStudioApplicationTestModule : AbpModule
{

}
