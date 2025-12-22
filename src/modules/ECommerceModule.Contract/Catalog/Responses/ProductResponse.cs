namespace ECommerceModule.Contract.Catalog.Responses;

public record ProductResponse(
    int ProductId,
    string Name,
    decimal Price,
    bool IsActive,
    int CategoryId
);
