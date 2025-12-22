using Test.Integration.TestScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;
using Test.Integration.Fixtures;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;

internal class ProductUpdateScenarioTest(
    UpdateProductRequestTest request,
    int productId = 0,
    bool isUpdateLastCreation = false) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (isUpdateLastCreation)
        {
            var product = context.Get<ApiResultTest<ProductResponseTest>>(ScenarioDataKey.ProductCreate);
            if (product != null && product.Result != null)
            {
                productId = product.Result.ProductId;
                request = request with { ProductId = productId };
            }
        }

        await context.ApiPutRequestAsync<UpdateProductRequestTest, ProductResponseTest>(
            url: $"/ecommerce/api/v1/catalog/products/{productId}",
            request: request,
            storeKey: ScenarioDataKey.ProductUpdate,
            scenarioName: GetType().Name);
    }
}

internal static class ProductUpdateScenarioTestExtensions
{
    public static ScenarioRunner ProductUpdate(
        this ScenarioRunner runner,
        int productId,
        UpdateProductRequestTest request)
        => runner.AddScenario(
            new ProductUpdateScenarioTest(
                request: request,
                productId: productId,
                isUpdateLastCreation: false));

    public static ScenarioRunner ProductUpdateLastCreation(
        this ScenarioRunner runner,
        UpdateProductRequestTest request)
        => runner.AddScenario(
            new ProductUpdateScenarioTest(
                request: request,
                productId: 0,
                isUpdateLastCreation: true));
}
