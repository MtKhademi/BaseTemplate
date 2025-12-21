namespace ECommerceModule.Contract.Order.Responses;

public record OrderResponse(
    long OrderId,
    Guid UserId,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt,
    IList<OrderItemResponse> Items
);
