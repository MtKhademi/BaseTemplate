namespace ECommerceModule.Contract.Catalog.Queries;

public record GetProductsPaginatedQuery(
    int PageNumber = 1,
    int PageSize = 10,
    int? CategoryId = null,
    bool? IsActive = null
) : IQuery<PaginatedList<ProductModel>>, IPagination
{
    int IPagination.CurrentPage { get => PageNumber; set { } }
    int IPagination.PageSize { get => PageSize; set { } }
};
