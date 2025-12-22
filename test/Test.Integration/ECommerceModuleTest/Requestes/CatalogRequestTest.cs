namespace Test.Integration.ECommerceModuleTest.Requestes;

public record CreateProductRequestTest(
    string? Name = null,
    decimal? Price = null,
    int? CategoryId = null,
    bool IsActive = true);

public record UpdateProductRequestTest(
    int? ProductId = null,
    string? Name = null,
    decimal? Price = null,
    int? CategoryId = null,
    bool? IsActive = null
);

public record CreateCategoryRequestTest(
    string? Name = null,
    int? ParentId = null
);

public record UpdateCategoryRequestTest(
    int? CategoryId = null,
    string? Name = null,
    int? ParentId = null
);
