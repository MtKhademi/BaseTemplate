namespace ECommerceModule.Contract.Catalog.Requests;

public record UpdateProductRequest(
    int ProductId,
    string Name,
    decimal Price,
    int CategoryId,
    bool IsActive
)
{
    public UpdateProductCommand ToCommand() => UpdateProductCommand.Create(this);
}
