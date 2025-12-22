using Test.Integration.TestScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;
using Test.Integration.Fixtures;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;

internal class CategoryCreateScenarioTest(
    CreateCategoryRequestTest request,
    bool isParentLastCreation = false) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (isParentLastCreation)
        {
            var category = context.Get<ApiResultTest<CategoryResponseTest>>(ScenarioDataKey.CategoryCreate);
            if (category != null && category.Result != null)
                request = request with { ParentId = category.Result.CategoryId };
        }

        await context.ApiPostRequestAsync<CreateCategoryRequestTest, CategoryResponseTest>(
            url: "/ecommerce/api/v1/catalog/categories",
            storeKey: ScenarioDataKey.CategoryCreate,
            request: request,
            scenarioName: GetType().Name);
    }
}

internal static class CategoryCreateScenarioTestExtensions
{
    public static ScenarioRunner CategoryCreateDefault(this ScenarioRunner runner,
        string name = "Test Category")
        => CategoryCreate(runner,
            request: new CreateCategoryRequestTest(Name: name),
            isParentLastCreation: false);

    public static ScenarioRunner CategoryCreateWithParentLastCreation(
        this ScenarioRunner runner,
        string name)
        => CategoryCreate(runner,
            request: new CreateCategoryRequestTest(Name: name),
            isParentLastCreation: true);

    public static ScenarioRunner CategoryCreate(
        this ScenarioRunner runner,
        CreateCategoryRequestTest request,
        bool isParentLastCreation = false)
    {
        return runner.AddScenario(
            new CategoryCreateScenarioTest(
                request: request,
                isParentLastCreation: isParentLastCreation));
    }
}
