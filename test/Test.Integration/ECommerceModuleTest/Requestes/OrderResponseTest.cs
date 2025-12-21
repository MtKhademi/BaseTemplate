namespace Test.Integration.ECommerceModuleTest.Requestes;

public record OrderResponseTest(
    long OrderId,
    Guid UserId,
    decimal TotalAmount,
    OrderStatusTest Status,
    DateTime CreatedAt,
    IList<OrderItemResponseTest> Items
);

public record OrderItemResponseTest(
    long OrderItemId,
    int ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);

public enum OrderStatusTest
{
    Pending = 1,
    Paid = 2,
    Shipped = 3,
    Completed = 4,
    Cancelled = 5
}
