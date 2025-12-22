using Test.Integration.TestScenarios;
using Test.Integration.Fixtures;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;

internal class ProductGetByIdScenarioTest(
    int productId = 0,
    bool isGetLastCreation = false) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (isGetLastCreation)
        {
            var product = context.Get<ApiResultTest<ProductResponseTest>>(ScenarioDataKey.ProductCreate);
            if (product != null && product.Result != null)
                productId = product.Result.ProductId;
        }

        await context.ApiGetRequestAsync<ProductResponseTest>(
            url: $"/ecommerce/api/v1/catalog/products/{productId}",
            storeKey: ScenarioDataKey.ProductGetByIdResponse,
            scenarioName: GetType().Name);
    }
}

internal static class ProductGetByIdScenarioTestExtensions
{
    public static ScenarioRunner ProductGetById(this ScenarioRunner runner, int productId)
    => runner.AddScenario(new ProductGetByIdScenarioTest(productId: productId));

    public static ScenarioRunner ProductGetByIdFromLastCreation(this ScenarioRunner runner)
    => runner.AddScenario(new ProductGetByIdScenarioTest(isGetLastCreation: true));
}
