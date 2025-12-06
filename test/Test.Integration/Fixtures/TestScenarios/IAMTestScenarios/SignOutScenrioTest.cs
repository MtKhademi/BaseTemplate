namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios;

internal class SignOutScenrioTest : IRestApiScenarioTest
{
    public string ApiEndpoint => $"/iam/api/v1/auth/signout";

    public SignOutScenrioTest()
    {
    }
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var response = await context.Client.PostAsync(ApiEndpoint, null);

        await response.WriteOnConsoleAsync(context.TestOutputHelper, "signout user");

        context.SetLastResponse(response);

        await context.SetApiResultAsync(ScenarioDataKey.CurrentUser, response);

        if (response.IsSuccessStatusCode)
        {
            context.Client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
