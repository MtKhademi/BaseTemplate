namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.AuthScenarios;

internal class LoginScenrioTest : IRestApiScenarioTest
{
    private LoginRequestTest _request;

    public string ApiEndpoint => $"/iam/api/v1/auth/login";

    public LoginScenrioTest(LoginRequestTest request)
    {
        _request = request;
    }
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var response = await context.Client.PostAsync(ApiEndpoint, _request.ToContentHttp());

        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Login user");

        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }


        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<TokenResponseTest>>();
        apiResult.Should().NotBeNull();
        var tokenResponse = apiResult!.Result;
        tokenResponse.Should().NotBeNull();

        context.Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiResult!.Result!.Token);
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