using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace XStudio.Data;

/* This is used if database provider does't define
 * IXStudioDbSchemaMigrator implementation.
 */
public class NullXStudioDbSchemaMigrator : IXStudioDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
