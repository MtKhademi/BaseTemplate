using Test.Integration.TestScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;
using Test.Integration.Fixtures;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;

internal class ProductCreateScenarioTest(
    CreateProductRequestTest request,
    bool isUseCategoryCreated = false) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var requestWithCategory = request;
        if (isUseCategoryCreated)
        {
            var category = context.Get<ApiResultTest<CategoryResponseTest>>(ScenarioDataKey.CategoryCreate);
            if (category != null && category.Result != null)
            {
                requestWithCategory = request with { CategoryId = category.Result.CategoryId };
            }
        }

        var apiResult = await context.ApiPostRequestAsync<CreateProductRequestTest, ProductResponseTest>(
            url: "/ecommerce/api/v1/catalog/products",
            storeKey: ScenarioDataKey.ProductCreate,
            request: requestWithCategory,
            scenarioName: GetType().Name);

        context.AddToList(ScenarioDataKey.Products, apiResult);
    }
}

internal static class ProductCreateScenarioTestExtensions
{
    public static ScenarioRunner ProductCreateDefault(this ScenarioRunner runner,
        string name = "Test Product",
        decimal price = 99.99m,
        bool isActive = true,
        bool isUseCategoryCreated = true)
        => ProductCreate(runner, name: name,
            price: price,
            isActive: isActive,
            isUseCategoryCreated: isUseCategoryCreated);

    public static ScenarioRunner ProductCreate(this ScenarioRunner runner,
        string name,
        decimal price,
        int? categoryId = null,
        bool isActive = true,
        bool isUseCategoryCreated = false)
    {
        return ProductCreate(runner, new CreateProductRequestTest(
            Name: name,
            Price: price,
            CategoryId: categoryId,
            IsActive: isActive),
            isUseCategoryCreated: isUseCategoryCreated);
    }

    public static ScenarioRunner ProductCreate(this ScenarioRunner runner,
        CreateProductRequestTest request,
        bool isUseCategoryCreated = false)
    {
        return runner.AddScenario(new ProductCreateScenarioTest(request, isUseCategoryCreated));
    }
}
