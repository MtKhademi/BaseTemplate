using ECommerceModule.Catalog.Repositories;
namespace ECommerceModule.Persistence.Repositories;

internal class CategoryRepository(ECommerceDbContext dbContext)
    : EFBaseRepository<int, CategoryEntity>(dbContext), ICategoryRepository
{
    public override IQueryable<CategoryEntity> QueryNoTracking()
        => dbContext.Categories.AsNoTracking();
}
