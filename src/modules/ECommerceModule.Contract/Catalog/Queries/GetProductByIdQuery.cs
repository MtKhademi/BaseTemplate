namespace ECommerceModule.Contract.Catalog.Queries;

public record GetProductByIdQuery(int ProductId) : IQuery<ProductModel>;
