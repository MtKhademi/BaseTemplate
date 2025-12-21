namespace ECommerceModule.Contract.Order.Queries;

public record GetOrderByIdQuery(int OrderId) : IQuery<OrderModel>;
