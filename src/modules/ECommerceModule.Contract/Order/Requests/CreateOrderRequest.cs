namespace ECommerceModule.Contract.Order.Requests;

public record CreateOrderRequest(
    Guid UserId,
    IList<CreateOrderItemRequest> Items
)
{
    public CreateOrderCommand ToCreateOrderCommand() => CreateOrderCommand.Create(this);
}

public record CreateOrderItemRequest(
    int ProductId,
    int Quantity,
    decimal UnitPrice
);
