using System.Threading.Tasks;

namespace XStudio.Data;

public interface IXStudioDbSchemaMigrator
{
    Task MigrateAsync();
}
