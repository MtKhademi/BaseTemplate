namespace ECommerceModule.Contract.Catalog.Responses;

public record CategoryResponse(
    int CategoryId,
    string Name,
    int? ParentId
);
