namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.RoleScenarios;

internal class RoleGetPaginatedScenarioTest : IScenarioStep
{
    public string ApiEndpoint => $"/iam/api/v1/roles";
    public RoleGetPaginatedScenarioTest()
    {
    }

    public async Task ExecuteAsync(ScenarioContext context)
    {

        var response = await context.Client.GetAsync(ApiEndpoint);
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Role get paginated");

        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var apiResult = await context.ApiGetRequestAsync<PaginatedListTest<RoleResponseTest>>(
            url: $"/iam/api/v1/roles",
            storeKey: ScenarioDataKey.RoleGetPaginated,
            scenarioName: GetType().Name);
    }
}

internal static class RoleGetPaginatedScenarioTestExtensions
{
    public static ScenarioRunner RoleGetPaginated(this ScenarioRunner runner)
        => runner.AddScenario(new RoleGetPaginatedScenarioTest());
}