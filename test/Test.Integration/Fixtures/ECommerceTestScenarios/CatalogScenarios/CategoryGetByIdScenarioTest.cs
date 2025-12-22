using Test.Integration.TestScenarios;
using Test.Integration.Fixtures;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;

internal class CategoryGetByIdScenarioTest(
    int? categoryId = null,
    bool getFromLastCreation = false) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        int _categoryId = categoryId ?? 0;
        if (getFromLastCreation)
        {
            var category = context.Get<ApiResultTest<CategoryResponseTest>>(ScenarioDataKey.CategoryCreate);
            if (category != null && category.Result != null)
            {
                _categoryId = category.Result.CategoryId;
            }
        }

        await context.ApiGetRequestAsync<CategoryResponseTest>(
            url: $"/ecommerce/api/v1/catalog/categories/{_categoryId}",
            storeKey: ScenarioDataKey.CategoryGetByIdResponse,
            scenarioName: GetType().Name);
    }
}

internal static class CategoryGetByIdScenarioTestExtensions
{
    public static ScenarioRunner CategoryGetById(this ScenarioRunner runner, int categoryId)
    {
        return runner.AddScenario(new CategoryGetByIdScenarioTest(categoryId));
    }

    public static ScenarioRunner CategoryGetByIdFromLastCreation(this ScenarioRunner runner)
    {
        return runner.AddScenario(new CategoryGetByIdScenarioTest(getFromLastCreation: true));
    }
}
