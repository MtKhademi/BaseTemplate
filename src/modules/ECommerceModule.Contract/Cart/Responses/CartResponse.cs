namespace ECommerceModule.Contract.Cart.Responses;

public record CartResponse(
    int CartId,
    Guid UserId,
    DateTime CreatedAt,
    List<CartItemResponse> Items,
    decimal TotalAmount
);
