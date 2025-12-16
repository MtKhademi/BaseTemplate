using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.UserScenarios;

internal class UserRoleGetsScenarioTest : IScenarioStep
{
    public string? UserId { get; private set; }

    public string ApiEndpoint => $"/iam/api/v1/users/[userId]/roles";

    public UserRoleGetsScenarioTest(string? userId = null)
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

        var response = await context.Client.GetAsync(ApiEndpoint.Replace("[userId]", UserId));
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "User roles get");
        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
            return;

        var apiResult = await response.Content.ReadFromJsonAsync<ApiResultTest<UserRoleResponseTest>>();
        context.Set(ScenarioDataKey.UserRoleGetsResponse, apiResult);
    }
}

internal static class UserRoleGetsScenarioTestExtensions
{
    /// <summary>
    /// if not set user id will be taken from created user in context
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static ScenarioRunner UserRoleGets(this ScenarioRunner runner,
        string? userId = null)
    {
        return runner.AddScenario(new UserRoleGetsScenarioTest(userId));
    }
}