namespace ECommerceModule.Cart.Entities;

internal class CartEntity : Entity<int>
{
    public Guid UserId { get; private set; }
    private readonly List<CartItemEntity> _items = new();
    public IReadOnlyCollection<CartItemEntity> Items => _items;

    private CartEntity() { }

    internal CartEntity(Guid userId)
    {
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    internal void AddItem(CartItemEntity item)
    {
        var existingItem = _items.FirstOrDefault(x => x.ProductId == item.ProductId);
        if (existingItem != null)
        {
            _items.Remove(existingItem);
        }
        _items.Add(item);
    }

    internal void RemoveItem(int cartItemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == cartItemId);
        if (item != null)
        {
            _items.Remove(item);
        }
    }

    internal void UpdateItemQuantity(int cartItemId, int quantity)
    {
        var item = _items.FirstOrDefault(x => x.Id == cartItemId);
        if (item != null)
        {
            item.UpdateQuantity(quantity);
        }
    }

    internal decimal GetTotalAmount()
    {
        return _items.Sum(x => x.GetTotalPrice());
    }

    public CartModel ToCartModel()
        => new(
            Id,
            UserId,
            CreatedAt: CreatedAt ??,
            _items.Select(x => x.ToCartItemModel()).ToList()
        );
}
