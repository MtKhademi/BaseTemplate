namespace ECommerceModule.Catalog.Entities;

internal class CategoryEntity : Entity<int>
{
    public string Name { get; private set; } = default!;
    public int? ParentId { get; private set; }
    public virtual CategoryEntity? Parent { get; private set; }


    private readonly List<ProductEntity> _products = new();
    public IReadOnlyCollection<ProductEntity> Products => _products;

    private CategoryEntity() { }

    internal CategoryEntity(string name, int? parentId)
    {
        Name = name;
        ParentId = parentId;
    }

    internal void Update(string name, int? parentId)
    {
        Name = name;
        ParentId = parentId;
    }


    public CategoryModel ToCategoryModel()
        => new(Id, Name, ParentId);
}
