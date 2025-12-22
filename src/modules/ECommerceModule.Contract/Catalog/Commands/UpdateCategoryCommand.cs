namespace ECommerceModule.Contract.Catalog.Commands;

public record UpdateCategoryCommand : ICommand<CategoryModel>
{
    public int CategoryId { get; init; }
    public string Name { get; init; } = default!;
    public int? ParentId { get; init; }

    private UpdateCategoryCommand(int categoryId, string name, int? parentId)
    {
        var errors = new List<string>();
        if (categoryId <= 0) errors.Add("CategoryId must be greater than zero.");
        if (string.IsNullOrWhiteSpace(name)) errors.Add("Name is required.");
        if (errors.Any()) throw new UpdateCategoryCommandException(errors);

        CategoryId = categoryId;
        Name = name;
        ParentId = parentId;
    }

    public static UpdateCategoryCommand Create(int categoryId, string name, int? parentId)
        => new(categoryId, name, parentId);

    public static UpdateCategoryCommand Create(UpdateCategoryRequest request)
        => new(request.CategoryId, request.Name, request.ParentId);

    internal class UpdateCategoryCommandException : NotValidDataException<UpdateCategoryCommandException>
    {
        public UpdateCategoryCommandException(IEnumerable<string> errors) : base(errors) { }
    }
}
