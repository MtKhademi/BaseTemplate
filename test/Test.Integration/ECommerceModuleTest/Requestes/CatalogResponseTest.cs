namespace Test.Integration.ECommerceModuleTest.Requestes;

public record ProductResponseTest(
    int ProductId,
    string Name,
    decimal Price,
    bool IsActive,
    int CategoryId
);

public record CategoryResponseTest(
    int CategoryId,
    string Name,
    int? ParentId
);
