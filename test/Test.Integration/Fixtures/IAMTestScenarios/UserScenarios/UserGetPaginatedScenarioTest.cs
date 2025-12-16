using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.UserScenarios;

internal class UserGetPaginatedScenarioTest : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {

        var apiResult = await context.ApiGetRequestAsync<PaginatedListTest<ApplicationUserResponseTest>>(
            url: $"/iam/api/v1/users",
            storeKey: ScenarioDataKey.UserGetPaginated,
            scenarioName: GetType().Name);
    }
}

internal static class UserGetPaginatedScenarioTestExtensions
{
    public static ScenarioRunner UserGetPaginated(this ScenarioRunner runner)
        => runner.AddScenario(new UserGetPaginatedScenarioTest());
}