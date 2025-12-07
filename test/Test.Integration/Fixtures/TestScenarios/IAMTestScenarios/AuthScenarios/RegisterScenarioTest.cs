namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.AuthScenarios;

internal class RegisterScenarioTest : IRestApiScenarioTest
{
    public string ApiEndpoint => $"/iam/api/v1/auth/register";
    private readonly RegistrationRequestTest _request;

    public RegisterScenarioTest(RegistrationRequestTest request)
    {
        _request = request;
    }


    public async Task ExecuteAsync(ScenarioContext context)
    {
        var response = await context.Client.PostAsync(ApiEndpoint, _request.ToContentHttp());
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Register");

        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var responseData = await response.Content.ReadFromJsonAsync<ApiResultTest<ApplicationUserResponseTest>>();
        context.Set(ScenarioDataKey.Register, responseData);
        context.AddToList(ScenarioDataKey.CurrentUser, responseData!.Result!);
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