namespace ECommerceModule.Contract.Catalog.Requests;

public record CreateProductRequest(
    string? Name = null,
    decimal? Price = null,
    int? CategoryId = null,
    bool IsActive = true
)
{
    public CreateProductCommand ToCommand() => CreateProductCommand.Create(this);
}
