using ECommerceModule.Contract.Order.Commands;

namespace ECommerceModule.Orders.Features.MarkOrderAsPaid;

internal class MarkOrderAsPaidHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<MarkOrderAsPaidCommand, OrderModel>
{
    public async Task<OrderModel> Handle(MarkOrderAsPaidCommand command, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIDAsync(command.OrderId)
            ?? throw new EntityNotFoundException<OrderEntity, int>(command.OrderId);

        order.MarkAsPaid();

        await orderRepository.UpdateAsync(order);
        await unitOfWork.SaveAsync();

        return order.ToOrderModel();
    }
}
