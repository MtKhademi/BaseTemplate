namespace ECommerceModule.Contract.Catalog.Commands;

public record CreateCategoryCommand : ICommand<CategoryModel>
{
    public string Name { get; init; } = default!;
    public int? ParentId { get; init; }

    private CreateCategoryCommand(string name, int? parentId)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(name)) errors.Add("Name is required.");
        if (errors.Any()) throw new CreateCategoryCommandException(errors);
        Name = name;
        ParentId = parentId;
    }

    public static CreateCategoryCommand Create(string name, int? parentId) => new(name, parentId);
    public static CreateCategoryCommand Create(CreateCategoryRequest request) => new(request.Name, request.ParentId);

    internal class CreateCategoryCommandException : NotValidDataException<CreateCategoryCommandException>
    {
        public CreateCategoryCommandException(IEnumerable<string> errors) : base(errors) { }
    }
}
