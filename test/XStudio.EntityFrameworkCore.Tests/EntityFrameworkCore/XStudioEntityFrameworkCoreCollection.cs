using Xunit;

namespace XStudio.EntityFrameworkCore;

[CollectionDefinition(XStudioTestConsts.CollectionDefinitionName)]
public class XStudioEntityFrameworkCoreCollection : ICollectionFixture<XStudioEntityFrameworkCoreFixture>
{

}
