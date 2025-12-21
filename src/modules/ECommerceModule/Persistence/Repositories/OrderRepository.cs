

namespace ECommerceModule.Persistence.Repositories;

internal class OrderRepository(ECommerceDbContext dbContext) :
    EFBaseRepository<int, OrderEntity>(dbContext), IOrderRepository
{

    public override IQueryable<OrderEntity> QueryNoTracking()
        => dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items);

    public override Task<OrderEntity?> GetByIDAsync(int id)
    {
        return QueryNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
