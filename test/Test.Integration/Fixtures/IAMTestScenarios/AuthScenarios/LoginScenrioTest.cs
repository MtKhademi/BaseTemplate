using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.AuthScenarios;

internal class LoginScenrioTest(LoginRequestTest request) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiPostRequestAsync<LoginRequestTest, TokenResponseTest>(
            url: "/iam/api/v1/auth/login",
            storeKey: ScenarioDataKey.CurrentUser,
            request: request,
            scenarioName: GetType().Name);

        if (apiResult?.Result?.Token != null)
        {
            context.Client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiResult.Result.Token);
        }
    }
}


internal static class LoginUserScenrioTestExtensions
{
    public static ScenarioRunner LoginAdmin(this ScenarioRunner runner) => LoginUser(runner, "admin", "8585@8585");
    public static ScenarioRunner LoginDefaultUser(this ScenarioRunner runner) => LoginUser(runner, new LoginRequestTest("testuser", "P@ssw0rd"));
    public static ScenarioRunner LoginUser(this ScenarioRunner runner,
        string? userName = default!,
        string? password = default!)
        => LoginUser(runner, new LoginRequestTest(userName, password));
    public static ScenarioRunner LoginUser(this ScenarioRunner runner, LoginRequestTest request)
        => runner.AddScenario(new LoginScenrioTest(request));
}