namespace ECommerceModule.Contract.Cart.Responses;

public record CartItemResponse(
    int CartItemId,
    int CartId,
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
