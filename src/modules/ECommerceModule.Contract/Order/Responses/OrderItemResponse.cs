namespace ECommerceModule.Contract.Order.Responses;

public record OrderItemResponse(
    long OrderItemId,
    int ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
