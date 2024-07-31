using Volo.Abp.Modularity;

namespace XStudio;

/* Inherit from this class for your domain layer tests. */
public abstract class XStudioDomainTestBase<TStartupModule> : XStudioTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
