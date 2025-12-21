namespace ECommerceModule.Contract.Order.Models;

public record OrderModel(
    int OrderId,
    Guid UserId,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt,
    IList<OrderItemModel> Items
);

public record OrderItemModel(
    long OrderItemId,
    int OrderId,
    int ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
