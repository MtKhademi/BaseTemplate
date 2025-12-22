using Test.Integration.TestScenarios;
using Test.Integration.Fixtures;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;

internal class ProductDeleteScenarioTest(
    int productId = 0,
    bool isDeleteLastCreation = false) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (isDeleteLastCreation)
        {
            var product = context.Get<ApiResultTest<ProductResponseTest>>(ScenarioDataKey.ProductCreate);
            if (product != null && product.Result != null)
                productId = product.Result.ProductId;
        }

        await context.ApiDeleteRequestAsync<bool>(
            url: $"/ecommerce/api/v1/catalog/products/{productId}",
            storeKey: ScenarioDataKey.ProductDelete,
            scenarioName: GetType().Name);
    }
}

internal static class ProductDeleteScenarioTestExtensions
{
    public static ScenarioRunner ProductDelete(this ScenarioRunner runner, int productId)
    {
        return runner.AddScenario(new ProductDeleteScenarioTest(productId));
    }

    public static ScenarioRunner ProductDeleteLastCreation(this ScenarioRunner runner)
    => runner.AddScenario(new ProductDeleteScenarioTest(isDeleteLastCreation: true));
}
