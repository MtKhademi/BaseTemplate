
namespace IAMModule.Data.Repositories;

internal class ModuleRepository(IAMModuleDbContext dbContext) :
    EFBaseRepository<int, ModuleEntity>(dbContext), IModuleRepository
{
}

internal static class ModuleRepositoryExtensions
{
    public static IQueryable<ModuleEntity> FilterById(this IQueryable<ModuleEntity> query, int? id)
    {
        if (id.HasValue)
        {
            query = query.Where(x => x.Id == id.Value);
        }
        return query;
    }

    public static IQueryable<ModuleEntity> FilterByName(this IQueryable<ModuleEntity> query, string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(x => x.Name.Contains(name));
        }
        return query;
    }
}
