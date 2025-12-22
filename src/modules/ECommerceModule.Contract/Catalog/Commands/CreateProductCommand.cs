namespace ECommerceModule.Contract.Catalog.Commands;

public record CreateProductCommand : ICommand<ProductModel>
{
    public string Name { get; init; } = default!;
    public decimal Price { get; init; }
    public int CategoryId { get; init; }
    public bool IsActive { get; init; } = true;

    private CreateProductCommand(string name, decimal price, int categoryId, bool isActive)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(name)) errors.Add("Name is required.");
        if (price <= 0) errors.Add("Price must be greater than zero.");
        if (categoryId <= 0) errors.Add("CategoryId must be greater than zero.");

        if (errors.Any()) throw new CreateProductCommandException(errors);

        Name = name;
        Price = price;
        CategoryId = categoryId;
        IsActive = isActive;
    }

    public static CreateProductCommand Create(string name, decimal price, int categoryId, bool isActive = true)
        => new(name, price, categoryId, isActive);

    public static CreateProductCommand Create(CreateProductRequest request)
    {
        var errors = new List<string>();
        if (request.Name is null) errors.Add("Name is required.");
        if (request.Price is null) errors.Add("Price is required.");
        if (request.CategoryId is null) errors.Add("CategoryId is required.");
        if (errors.Any()) throw new CreateProductCommandException(errors);

        return new(request.Name!, request.Price!.Value, request.CategoryId!.Value, request.IsActive);
    }


    internal class CreateProductCommandException : NotValidDataException<CreateProductCommandException>
    {
        public CreateProductCommandException(IEnumerable<string> errors) : base(errors) { }
    }
}
