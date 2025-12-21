namespace ECommerceModule.Orders.Entities;

internal class OrderItemEntity : Entity<long>
{
    public int OrderId { get; private set; }
    public OrderEntity Order { get; set; }

    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private OrderItemEntity() { }

    internal OrderItemEntity(int productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
