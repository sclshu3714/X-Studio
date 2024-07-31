using Volo.Abp.Modularity;

namespace XStudio;

[DependsOn(
    typeof(XStudioDomainModule),
    typeof(XStudioTestBaseModule)
)]
public class XStudioDomainTestModule : AbpModule
{

}
