namespace ECommerceModule.Inventory.Entities;

internal class InventoryItemEntity:Entity<long>
{

    public int ProductId { get; private set; }

    public int AvailableQuantity { get; private set; }
    public int ReservedQuantity { get; private set; }

    private InventoryItemEntity() { }
}
