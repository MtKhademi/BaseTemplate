namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.AuthScenarios;

internal class SignOutScenrioTest : IRestApiScenarioTest
{
    public string ApiEndpoint => $"/iam/api/v1/auth/signout";

    public SignOutScenrioTest()
    {
    }
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var response = await context.Client.PostAsync(ApiEndpoint, null);

        await response.WriteOnConsoleAsync(context.TestOutputHelper, "signout user");

        context.SetLastResponse(response);
    }
}


internal static class SignOutScenrioTestExtensions
{
    public static ScenarioRunner SignOut(this ScenarioRunner runner)
    {
        return runner.AddScenario(new SignOutScenrioTest());
    }
}