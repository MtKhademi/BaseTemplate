namespace ECommerceModule.Orders.Extensions;

internal static class OrderQueryExtensions
{
    public static IQueryable<OrderEntity> FilterByUserId(this IQueryable<OrderEntity> query, Guid? userId)
    {
        if (userId.HasValue && userId.Value != Guid.Empty)
        {
            return query.Where(x => x.UserId == userId.Value);
        }

        return query;
    }
}
