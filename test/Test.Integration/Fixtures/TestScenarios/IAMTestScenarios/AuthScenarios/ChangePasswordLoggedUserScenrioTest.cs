namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.AuthScenarios;

internal class ChangePasswordLoggedUserScenrioTest(ChangePasswordRequestTest request) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        await context.ApiPatchRequestAsync<ChangePasswordRequestTest, bool>(
            url: "/iam/api/v1/auth/change-password",
            storeKey: ScenarioDataKey.ChangePasswordLoggedUserResponse,
            request: request,
            scenarioName: GetType().Name);
    }
}


internal static class ChangePasswordLoggedUserUserScenrioTestExtensions
{
    public static ScenarioRunner ChangePasswordLoggedUser(this ScenarioRunner runner)
        => runner.AddScenario(new ChangePasswordLoggedUserScenrioTest(
            new ChangePasswordRequestTest(
                CurrentPassword: "P@ssw0rd",
                NewPassword: "N3wP@ssw0rd123",
                ConfirmNewPassword: "N3wP@ssw0rd123"
                )));
    public static ScenarioRunner ChangePasswordLoggedUser(this ScenarioRunner runner, ChangePasswordRequestTest request)
        => runner.AddScenario(new ChangePasswordLoggedUserScenrioTest(request));
}