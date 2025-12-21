namespace Test.Integration.ECommerceModuleTest.Requestes;

public record CreateOrderRequestTest(
    Guid UserId,
    IList<CreateOrderItemRequestTest> Items
);

public record CreateOrderItemRequestTest(
    int ProductId,
    int Quantity,
    decimal UnitPrice
);
