using Volo.Abp.Modularity;

namespace XStudio;

public abstract class XStudioApplicationTestBase<TStartupModule> : XStudioTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
