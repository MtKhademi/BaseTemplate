using Test.Integration.TestScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;
using Test.Integration.Fixtures;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;

internal class CategoryUpdateScenarioTest(
    int categoryId = 0,
    UpdateCategoryRequestTest? request = null,
    bool isUpdateLastCreation = false,
    int? numberChooseForUpdate = null,
    int? numberChooseForParent = null
    ) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {

        if (isUpdateLastCreation)
        {
            var category = context.Get<ApiResultTest<CategoryResponseTest>>(ScenarioDataKey.CategoryCreate);
            if (category != null && category.Result != null)
            {
                categoryId = category.Result.CategoryId;
                request = request with { CategoryId = categoryId };
            }
        }

        if (numberChooseForUpdate.HasValue)
        {
            var categories = context.GetList<CategoryResponseTest>(ScenarioDataKey.Categories);
            categoryId = categories[numberChooseForUpdate.Value].CategoryId;
            request = request with { CategoryId = categoryId };
        }

        if (numberChooseForParent.HasValue)
        {
            var categories = context.GetList<CategoryResponseTest>(ScenarioDataKey.Categories);
            categoryId = categories[numberChooseForParent.Value].CategoryId;
            request = request with { ParentId = categoryId };
        }

        await context.ApiPutRequestAsync<UpdateCategoryRequestTest, CategoryResponseTest>(
            url: $"/ecommerce/api/v1/catalog/categories/{categoryId}",
            request: request,
            storeKey: ScenarioDataKey.CategoryUpdate,
            scenarioName: GetType().Name);
    }
}

internal static class CategoryUpdateScenarioTestExtensions
{

    public static ScenarioRunner CategoryUpdate(
        this ScenarioRunner runner,
        int categoryId = 0,
        UpdateCategoryRequestTest? request = null,
        bool isUpdateLastCreation = false,
        int? numberChooseForUpdate = null,
        int? numberChooseForParent = null)
        => runner.AddScenario(new CategoryUpdateScenarioTest(
            request: request,
            categoryId: categoryId,
            isUpdateLastCreation: isUpdateLastCreation,
            numberChooseForParent: numberChooseForParent,
            numberChooseForUpdate: numberChooseForUpdate));


    public static ScenarioRunner CategoryUpdateLastCreation(
        this ScenarioRunner runner,
        UpdateCategoryRequestTest request)
    {
        return runner.AddScenario(
            new CategoryUpdateScenarioTest(
                request: request,
                categoryId: 0,
                isUpdateLastCreation: true));
    }
}
