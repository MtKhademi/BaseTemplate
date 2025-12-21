namespace ECommerceModule.Orders.Entities;

internal class OrderEntity : Entity<int>
{
    public Guid UserId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderItemEntity> _items = new();
    public IReadOnlyCollection<OrderItemEntity> Items => _items;

    private OrderEntity() { }

    public OrderEntity(Guid userId)
    {
        UserId = userId;
        Status = OrderStatus.Pending;
    }

    public void AddItem(int productId, int quantity, decimal unitPrice)
    {
        _items.Add(new OrderItemEntity(productId, quantity, unitPrice));
        TotalAmount += quantity * unitPrice;
    }

    public void MarkAsPaid()
        => Status = OrderStatus.Paid;
}
