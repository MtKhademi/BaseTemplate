namespace ECommerceModule.Cart.Entities;

internal class CartItemEntity : Entity<int>
{
    public int CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private CartItemEntity() { }

    internal CartItemEntity(int cartId, Guid productId, int quantity, decimal unitPrice)
    {
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    internal void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }

    internal decimal GetTotalPrice()
    {
        return UnitPrice * Quantity;
    }

    public CartItemModel ToCartItemModel()
        => new(
            Id,
            CartId,
            ProductId,
            Quantity,
            UnitPrice,
            GetTotalPrice()
        );
}
