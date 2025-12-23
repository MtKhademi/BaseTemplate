namespace ECommerceModule.Contract.Cart.Models;

public record CartModel(
    int CartId,
    Guid UserId,
    DateTime CreatedAt,
    List<CartItemModel> Items
);
