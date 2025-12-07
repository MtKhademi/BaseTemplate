namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

internal class UserChangeStateActiveScenarioTest : IRestApiScenarioTest
{
    public string? UserId { get; private set; }

    public string ApiEndpoint => $"/iam/api/v1/users/[userId]/change-state";

    /// <summary>
    /// if not provided, will get from created user in context
    /// </summary>
    /// <param name="userId"></param>
    public UserChangeStateActiveScenarioTest(string? userId = null)
    {
        UserId = userId;
    }
    public async Task ExecuteAsync(ScenarioContext context)
    {

        if (string.IsNullOrWhiteSpace(UserId))
        {
            var createdUser = context.Get<ApiResultTest<ApplicationUserResponseTest>>(ScenarioDataKey.UserCreate);
            if (createdUser == null || createdUser.Result is null || string.IsNullOrWhiteSpace(createdUser.Result.UserId))
            {
                context.TestOutputHelper.WriteLine(createdUser.ToJson());
                throw new InvalidOperationException("No UserId provided and no created user found in context.");
            }
            UserId = createdUser.Result.UserId;
        }

        var response = await context.Client.PatchAsync(ApiEndpoint.Replace("[userId]", UserId), null);
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Get User By Id");
        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var responseContent = await response.Content.ReadFromJsonAsync<ApiResultTest<ApplicationUserResponseTest>>();
        context.Set(ScenarioDataKey.UserGetByIdResponse, responseContent);
    }


}

internal static class UserChangeStateActiveScenarioTestExtensions
{
    public static ScenarioRunner UserChangeStateActive(this ScenarioRunner runner,
        string? userId = null)
    {
        return runner.AddScenario(new UserChangeStateActiveScenarioTest(userId));
    }
}