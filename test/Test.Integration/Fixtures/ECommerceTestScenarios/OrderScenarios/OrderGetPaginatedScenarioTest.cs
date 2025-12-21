using Test.Integration.TestScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.OrderScenarios;

internal class OrderGetPaginatedScenarioTest(int pageNumber = 1, int pageSize = 10, Guid? userId = null) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}";
        if (userId.HasValue)
        {
            queryString += $"&userId={userId.Value}";
        }

        var apiResult = await context.ApiGetRequestAsync<PaginatedListTest<OrderResponseTest>>(
            url: $"/ecommerce/api/v1/orders{queryString}",
            storeKey: ScenarioDataKey.OrderGetPaginated,
            scenarioName: GetType().Name);
    }
}

internal static class OrderGetPaginatedScenarioTestExtensions
{
    public static ScenarioRunner OrderGetPaginated(this ScenarioRunner runner,
        int pageNumber = 1,
        int pageSize = 10,
        Guid? userId = null)
        => runner.AddScenario(new OrderGetPaginatedScenarioTest(pageNumber, pageSize, userId));
}
