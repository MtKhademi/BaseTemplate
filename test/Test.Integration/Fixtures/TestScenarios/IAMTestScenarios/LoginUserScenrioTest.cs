namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios;

internal class LoginUserScenrioTest : IRestApiScenarioTest
{
    public string UserName { get; init; }
    public string Password { get; init; }

    public string ApiEndpoint => $"/iam/api/v1/auth/login";

    public LoginUserScenrioTest(
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

        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Login user");

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
