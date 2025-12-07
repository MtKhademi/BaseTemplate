namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

internal class UserDeleteScenarioTest : IRestApiScenarioTest
{
    public string? UserId { get; private set; }

    public string ApiEndpoint => $"/iam/api/v1/users";

    public UserDeleteScenarioTest(string? userId = null)
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

        var response = await context.Client.DeleteAsync($"{ApiEndpoint}/{UserId}");
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "User delete response");
        context.SetLastResponse(response);
    }
}

internal static class UserDeleteScenarioTestExtensions
{
    public static ScenarioRunner UserDelete(this ScenarioRunner runner, string? userId = null)
        => runner.AddScenario(new UserDeleteScenarioTest(userId));
}