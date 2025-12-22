namespace ECommerceModule.Catalog.Entities;

internal class ProductEntity : Entity<int>
{
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }
    public int CategoryId { get; private set; }
    public virtual CategoryEntity? Category { get; private set; }

    private ProductEntity() { }

    internal ProductEntity(string name, decimal price, int categoryId, bool isActive = true)
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
        IsActive = isActive;
    }

    internal void Update(string name, decimal price, int categoryId, bool isActive)
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
        IsActive = isActive;
    }

    internal void Deactivate() => IsActive = false;

    internal void Activate() => IsActive = true;
}
