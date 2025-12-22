namespace ECommerceModule.Catalog.Features.GetCategoriesPaginated;

internal class GetCategoriesPaginatedHandler(ICategoryRepository repository)
    : IQueryHandler<GetCategoriesPaginatedQuery, PaginatedList<CategoryModel>>
{
    public async Task<PaginatedList<CategoryModel>> Handle(GetCategoriesPaginatedQuery query, CancellationToken cancellationToken)
    {
        var items = await repository.QueryNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.ToCategoryModel())
            .ToPaginatedListAsync(query, cancellationToken);

        return items;
    }
}
