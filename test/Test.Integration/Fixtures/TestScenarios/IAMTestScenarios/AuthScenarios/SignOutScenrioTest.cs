namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.AuthScenarios;

internal class SignOutScenrioTest : IScenarioStep
{
    public SignOutScenrioTest()
    {
    }
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiPostRequestAsync<bool>(
            url: "/iam/api/v1/auth/signout",
            ScenarioDataKey.SignOut,
            scenarioName: GetType().Name);
    }
}


internal static class SignOutScenrioTestExtensions
{
    public static ScenarioRunner SignOut(this ScenarioRunner runner)
    {
        return runner.AddScenario(new SignOutScenrioTest());
    }
}