namespace ECommerceModule.Contract.Catalog.Queries;

public record GetCategoriesPaginatedQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IQuery<PaginatedList<CategoryModel>>, IPagination
{
    int IPagination.CurrentPage { get => PageNumber; set { } }
    int IPagination.PageSize { get => PageSize; set { } }
};
