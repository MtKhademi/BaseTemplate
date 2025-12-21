using ECommerceModule.Contract.Order.Commands;

namespace ECommerceModule.Orders.Features.CreateOrder;

internal class CreateOrderHandler(IOrderRepository orderRepository)
    : ICommandHandler<CreateOrderCommand, OrderModel>
{
    public async Task<OrderModel> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new OrderEntity(command.UserId);

        foreach (var item in command.Items)
        {
            order.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        }

        await orderRepository.CreateAsync(order, cancellationToken);

        return order.ToOrderModel();
    }
}
