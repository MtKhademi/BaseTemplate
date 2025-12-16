using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.UserScenarios;

internal class UserCreateScenarioTest(UserCreateRequestTest request) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiPostRequestAsync<UserCreateRequestTest, ApplicationUserResponseTest>(
            url: "/iam/api/v1/users",
            storeKey: ScenarioDataKey.UserCreate,
            request: request,
            scenarioName: GetType().Name);

        context.AddToList(ScenarioDataKey.Users, apiResult);
    }
}

internal static class UserCreateScenarioTestExtensions
{
    public static ScenarioRunner UserCreateDefault(this ScenarioRunner runner,
        string userName = "testuser",
        string password = "P@ssw0rd",
        string confirmPassword = "P@ssw0rd")
        => UserCreate(runner, userName, password, confirmPassword);

    public static ScenarioRunner UserCreate(this ScenarioRunner runner,
        string? userName,
        string? password,
        string? confirmPassword)
        => UserCreate(runner, new UserCreateRequestTest(
            UserName: userName,
            Password: password,
            ConfirmPassword: confirmPassword
        ));

    public static ScenarioRunner UserCreate(this ScenarioRunner runner,
        UserCreateRequestTest request)
    {
        return runner.AddScenario(new UserCreateScenarioTest(request));
    }
}