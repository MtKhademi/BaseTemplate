namespace ECommerceModule.Contract.Cart.Models;

public record CartItemModel(
    int CartItemId,
    int CartId,
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
