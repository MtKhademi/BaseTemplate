using ECommerceModule.Catalog.Repositories;
namespace ECommerceModule.Persistence.Repositories;

internal class ProductRepository(ECommerceDbContext dbContext)
    : EFBaseRepository<int, ProductEntity>(dbContext), IProductRepository
{
    public override IQueryable<ProductEntity> QueryNoTracking()
        => dbContext.Products.AsNoTracking();
}
