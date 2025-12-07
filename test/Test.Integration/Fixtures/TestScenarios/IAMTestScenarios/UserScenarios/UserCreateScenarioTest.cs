namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

internal class UserCreateScenarioTest : IRestApiScenarioTest
{
    public string ApiEndpoint => $"/iam/api/v1/users";
    private readonly UserCreateRequestTest _request;
    public UserCreateScenarioTest(UserCreateRequestTest request)
    {
        _request = request;
    }


    public async Task ExecuteAsync(ScenarioContext context)
    {
        var response = await context.Client.PostAsync(ApiEndpoint, _request.ToContentHttp());
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "User Creation");

        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
            return;

        var createdUser = await response.Content.ReadFromJsonAsync<ApiResultTest<ApplicationUserResponseTest>>();
        context.Set(ScenarioDataKey.UserCreate, createdUser);
        context.AddToList(ScenarioDataKey.Users, createdUser!.Result);
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