namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios;

internal class LoginAdminScenarioTest : IRestApiScenarioTest
{
    public string UserName { get; init; }
    public string Password { get; init; }

    public string ApiEndpoint => $"/iam/api/v1/auth/login";

    public LoginAdminScenarioTest(
        string userName = "admin",
        string password = "8585@8585")
    {
        UserName = userName;
        Password = password;
    }
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var response = await context.Client.PostAsync(ApiEndpoint, new LoginRequestTest
        (
            UserName: UserName,
            Password: Password
        ).ToContentHttp());

        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Login admin");

        context.SetLastResponse(response);

        await context.SetApiResultAsync(ScenarioDataKey.CurrentUser, response);

        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<TokenResponseTest>>();
        apiResult.Should().NotBeNull();
        var tokenResponse = apiResult!.Result;
        tokenResponse.Should().NotBeNull();

        context.Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiResult!.Result!.Token);
    }
}


internal static class LoginAdminScenarioTestExtensions
{
    public static ScenarioRunner AddLoginAdminScenario(this ScenarioRunner runner,
        string userName = "admin",
        string password = "8585@8585")
    {
        return runner.AddScenario(new LoginAdminScenarioTest(userName, password));
    }
}