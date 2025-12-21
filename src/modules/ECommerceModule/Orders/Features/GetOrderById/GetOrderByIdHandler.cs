namespace ECommerceModule.Orders.Features.GetOrderById;

internal class GetOrderByIdHandler(IOrderRepository orderRepository)
    : IQueryHandler<GetOrderByIdQuery, OrderModel>
{
    public async Task<OrderModel> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIDAsync(query.OrderId)
            ?? throw new EntityNotFoundException<OrderEntity, int>(query.OrderId);

        return order.ToOrderModel();
    }
}
