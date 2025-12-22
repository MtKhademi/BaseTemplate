namespace ECommerceModule.Contract.Catalog.Models;

public record CategoryModel(
    int CategoryId,
    string Name,
    int? ParentId
);
