namespace ECommerceModule.Catalog.Features.GetProductsPaginated;

internal class GetProductsPaginatedHandler(IProductRepository repository)
    : IQueryHandler<GetProductsPaginatedQuery, PaginatedList<ProductModel>>
{
    public async Task<PaginatedList<ProductModel>> Handle(GetProductsPaginatedQuery query, CancellationToken cancellationToken)
    {
        var items = await repository.QueryNoTracking()
            .FilterByCategory(query.CategoryId)
            .FilterByActive(query.IsActive)
            .OrderBy(x => x.Id)
            .Select(x => x.ToProductModel())
            .ToPaginatedListAsync(query, cancellationToken);

        return items;
    }
}
