namespace ECommerceModule.Contract.Catalog.Models;

public record ProductModel(
    int ProductId,
    string Name,
    decimal Price,
    bool IsActive,
    int CategoryId
);
