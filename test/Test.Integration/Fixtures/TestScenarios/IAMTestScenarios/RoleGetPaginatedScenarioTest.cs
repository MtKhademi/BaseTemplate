namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios;

internal class RoleGetPaginatedScenarioTest : IRestApiScenarioTest
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

        var responseContent = await response.Content.ReadFromJsonAsync<PaginatedListTest<RoleResponseTest>>();
        context.Set(ScenarioDataKey.RoleGetPaginated, responseContent);
    }
}

internal static class RoleGetPaginatedScenarioTestExtensions
{
    public static ScenarioRunner RoleGetPaginated(this ScenarioRunner runner)
        => runner.AddScenario(new RoleGetPaginatedScenarioTest());
}