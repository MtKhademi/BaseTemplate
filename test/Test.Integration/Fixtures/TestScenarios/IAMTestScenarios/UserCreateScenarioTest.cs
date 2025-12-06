namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios;

internal class UserCreateScenarioTest : IRestApiScenarioTest
{
    public string ApiEndpoint => $"/iam/api/v1/users";
    private readonly UserCreateRequestTest _request;
    public UserCreateScenarioTest(
        string userName,
        string password = "P@ssw0rd")
        : this(new UserCreateRequestTest(
            UserName: userName,
            Password: password,
            ConfirmPassword: password
        ))
    {
    }
    public UserCreateScenarioTest(UserCreateRequestTest request)
    {
        _request = request;
    }


    public async Task ExecuteAsync(ScenarioContext context)
    {
        var response = await context.Client.PostAsync(ApiEndpoint, _request.ToContentHttp());
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "User Creation");

        context.SetLastResponse(response);

        await context.SetApiResultAsync(ScenarioDataKey.UserCreate, response);
    }
}

internal static class UserCreateScenarioTestExtensions
{
    public static ScenarioRunner AddUserCreateScenario(this ScenarioRunner runner,
        string userName,
        string password = "P@ssw0rd")
    {
        return runner.AddScenario(new UserCreateScenarioTest(userName, password));
    }
}