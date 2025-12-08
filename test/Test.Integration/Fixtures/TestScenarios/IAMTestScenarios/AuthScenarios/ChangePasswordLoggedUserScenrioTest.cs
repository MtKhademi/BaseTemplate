namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.AuthScenarios;

internal class ChangePasswordLoggedUserScenrioTest : IRestApiScenarioTest
{
    private ChangePasswordRequestTest _request;

    public string ApiEndpoint => $"/iam/api/v1/auth/change-password";

    public ChangePasswordLoggedUserScenrioTest(ChangePasswordRequestTest request)
    {
        _request = request;
    }
    public async Task ExecuteAsync(ScenarioContext context)
    {

        var response = await context.Client.PatchAsync(ApiEndpoint, _request.ToContentHttp());

        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Change Password Logged User");

        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }


        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<bool>>();
        context.Set(ScenarioDataKey.ChangePasswordLoggedUserResponse, apiResult);
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