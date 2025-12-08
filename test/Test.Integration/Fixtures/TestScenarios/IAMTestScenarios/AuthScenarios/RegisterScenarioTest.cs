namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.AuthScenarios;

internal class RegisterScenarioTest(RegistrationRequestTest request) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiPostRequestAsync<RegistrationRequestTest, ApplicationUserResponseTest>(
            url: "/iam/api/v1/auth/register",
            storeKey: ScenarioDataKey.Register,
            request: request,
            scenarioName: GetType().Name);

        context.AddToList(ScenarioDataKey.Users, apiResult);

    }
}

internal static class RegisterScenarioTestExtensions
{

    public static ScenarioRunner RegisterDefault(this ScenarioRunner runner,
        string userName = "testuser",
        string password = "P@ssw0rd",
        string confirmPassword = "P@ssw0rd")
        => Register(runner, userName, password, confirmPassword);


    public static ScenarioRunner Register(this ScenarioRunner runner,
        string? userName = default!,
        string? password = default!,
        string? confirmPassword = default!)
        => Register(runner, new RegistrationRequestTest(
            UserName: userName,
            Password: password,
            ConfirmPassword: confirmPassword));

    public static ScenarioRunner Register(this ScenarioRunner runner, RegistrationRequestTest request)
        => runner.AddScenario(new RegisterScenarioTest(request));


}