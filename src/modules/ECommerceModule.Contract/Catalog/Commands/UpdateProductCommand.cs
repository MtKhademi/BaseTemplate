namespace ECommerceModule.Contract.Catalog.Commands;

public record UpdateProductCommand : ICommand<ProductModel>
{
    public int ProductId { get; init; }
    public string Name { get; init; } = default!;
    public decimal Price { get; init; }
    public int CategoryId { get; init; }
    public bool IsActive { get; init; }

    private UpdateProductCommand(int productId, string name, decimal price, int categoryId, bool isActive)
    {
        var errors = new List<string>();
        if (productId <= 0) errors.Add("ProductId must be greater than zero.");
        if (string.IsNullOrWhiteSpace(name)) errors.Add("Name is required.");
        if (price <= 0) errors.Add("Price must be greater than zero.");
        if (categoryId <= 0) errors.Add("CategoryId must be greater than zero.");

        if (errors.Any()) throw new UpdateProductCommandException(errors);

        ProductId = productId;
        Name = name;
        Price = price;
        CategoryId = categoryId;
        IsActive = isActive;
    }

    public static UpdateProductCommand Create(int productId, string name, decimal price, int categoryId, bool isActive)
        => new(productId, name, price, categoryId, isActive);

    public static UpdateProductCommand Create(UpdateProductRequest request)
        => new(request.ProductId, request.Name, request.Price, request.CategoryId, request.IsActive);

    internal class UpdateProductCommandException : NotValidDataException<UpdateProductCommandException>
    {
        public UpdateProductCommandException(IEnumerable<string> errors) : base(errors) { }
    }
}
