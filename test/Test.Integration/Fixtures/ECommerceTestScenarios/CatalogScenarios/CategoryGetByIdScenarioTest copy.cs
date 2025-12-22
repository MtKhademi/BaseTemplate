using Test.Integration.TestScenarios;
using Test.Integration.Fixtures;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;

internal class CategoryGetsPaginatedScenarioTest() : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        await context.ApiGetRequestAsync<PaginatedListTest<CategoryResponseTest>>(
            url: $"/ecommerce/api/v1/catalog/categories",
            storeKey: ScenarioDataKey.CategoryGetPaginated,
            storeListKey: ScenarioDataKey.Categories,
            scenarioName: GetType().Name);
    }
}

internal static class CategoryGetsPaginatedScenarioTestExtensions
{
    public static ScenarioRunner CategoryGetPaginated(this ScenarioRunner runner)
        => runner.AddScenario(new CategoryGetsPaginatedScenarioTest());
}
