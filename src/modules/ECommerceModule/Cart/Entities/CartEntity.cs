namespace ECommerceModule.Cart.Entities;

internal class CartEntity : Entity<int>
{
    public Guid UserId { get; private set; }

    private CartEntity() { }
}
