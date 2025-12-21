namespace ECommerceModule.Catalog.Entities;

internal class CategoryEntity : Entity<int>
{
    public string Name { get; private set; } = default!;
    public int? ParentId { get; private set; }

    private CategoryEntity() { }
}
