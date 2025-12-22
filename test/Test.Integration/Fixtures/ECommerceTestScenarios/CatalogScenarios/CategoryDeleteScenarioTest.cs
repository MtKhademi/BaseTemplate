using Test.Integration.TestScenarios;
using Test.Integration.Fixtures;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;

internal class CategoryDeleteScenarioTest(
    int? categoryId = null,
    bool getDataFromLastCreation = false) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (categoryId is null)
        {
            if (getDataFromLastCreation)
            {
                var category = context?.Get<ApiResultTest<CategoryResponseTest>>(ScenarioDataKey.CategoryCreate);
                if (category is null)
                    throw new Exception($"Not exist any category created");

                categoryId = category.Result.CategoryId;
            }
        }

        if (categoryId is null)
            throw new Exception($"Not choose any category id");

        await context.ApiDeleteRequestAsync<bool>(
            url: $"/ecommerce/api/v1/catalog/categories/{categoryId}",
            storeKey: ScenarioDataKey.CategoryDelete,
            scenarioName: GetType().Name);
    }
}

internal static class CategoryDeleteScenarioTestExtensions
{
    public static ScenarioRunner CategoryDelete(this ScenarioRunner runner, int categoryId)
    {
        return runner.AddScenario(new CategoryDeleteScenarioTest(categoryId));
    }

    public static ScenarioRunner CategoryDeleteLastCreation(this ScenarioRunner runner)
    {
        return runner.AddScenario(new CategoryDeleteScenarioTest(getDataFromLastCreation: true));
    }
}
