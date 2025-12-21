namespace ECommerceModule.Catalog.Entities;

internal class ProductEntity : Entity<int>
{
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }
    public int CategoryId { get; private set; }

    private ProductEntity() { }
}
