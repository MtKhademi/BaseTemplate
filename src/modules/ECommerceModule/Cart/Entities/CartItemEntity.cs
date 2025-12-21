namespace ECommerceModule.Cart.Entities;

internal class CartItemEntity : Entity<int>
{
    public int CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private CartItemEntity() { }
}
