namespace ECommerceModule.Contract.Order.Requests;

public record GetOrdersPaginatedRequest(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? UserId = null
);
