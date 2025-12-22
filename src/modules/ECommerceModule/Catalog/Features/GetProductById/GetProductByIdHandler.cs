namespace ECommerceModule.Catalog.Features.GetProductById;

internal class GetProductByIdHandler(IProductRepository repository)
    : IQueryHandler<GetProductByIdQuery, ProductModel>
{
    public async Task<ProductModel> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIDAsync(query.ProductId)
            ?? throw new EntityNotFoundException<ProductEntity, int>(query.ProductId);
        return entity.ToProductModel();
    }
}
