namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

internal class UserGetPaginatedScenarioTest : IRestApiScenarioTest
{
    public string ApiEndpoint => $"/iam/api/v1/users";

    public UserGetPaginatedScenarioTest()
    {
    }
    public async Task ExecuteAsync(ScenarioContext context)
    {

        var response = await context.Client.GetAsync(ApiEndpoint);
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Get users paginated");
        context.SetLastResponse(response);
    }
}

internal static class UserGetPaginatedScenarioTestExtensions
{
    public static ScenarioRunner UserGetPaginated(this ScenarioRunner runner)
        =>runner.AddScenario(new UserGetPaginatedScenarioTest());
}