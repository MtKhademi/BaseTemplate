namespace ECommerceModule.Contract.Order.Queries;

public record GetOrdersPaginatedQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? UserId = null
) : IQuery<PaginatedList<OrderModel>>, IPagination
{
    int IPagination.CurrentPage { get => PageNumber; set { } }
    int IPagination.PageSize { get => PageSize; set { } }
};
