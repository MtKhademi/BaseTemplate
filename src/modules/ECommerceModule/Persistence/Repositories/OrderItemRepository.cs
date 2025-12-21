namespace ECommerceModule.Persistence.Repositories;

internal class OrderItemRepository(ECommerceDbContext dbContext) :
    EFBaseRepository<long, OrderItemEntity>(dbContext), IOrderItemRepository
{ }
