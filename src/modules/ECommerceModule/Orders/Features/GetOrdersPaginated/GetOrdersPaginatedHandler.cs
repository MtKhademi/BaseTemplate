namespace ECommerceModule.Orders.Features.GetOrdersPaginated;

internal class GetOrdersPaginatedHandler(IOrderRepository orderRepository)
    : IQueryHandler<GetOrdersPaginatedQuery, PaginatedList<OrderModel>>
{
    public async Task<PaginatedList<OrderModel>> Handle(
        GetOrdersPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        var paginatedOrders = await orderRepository
            .QueryNoTracking()
            .FilterByUserId(query.UserId)
            .OrderByDescending(x => x.CreatedAt)
            .ToPaginatedListAsync(query, cancellationToken);

        return paginatedOrders.ToPaginatedList(order => order.ToOrderModel());
    }
}
