namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

internal class UserDeleteScenarioTest(string? userId = null) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {

        if (string.IsNullOrWhiteSpace(userId))
        {
            var createdUser = context.Get<ApiResultTest<ApplicationUserResponseTest>>(ScenarioDataKey.UserCreate);
            if (createdUser == null || createdUser.Result is null || string.IsNullOrWhiteSpace(createdUser.Result.UserId))
            {
                context.TestOutputHelper.WriteLine(createdUser.ToJson());
                throw new InvalidOperationException("No UserId provided and no created user found in context.");
            }
            userId = createdUser.Result.UserId;
        }

      
        var apiResult = await context.ApiDeleteRequestAsync<bool>(
            url: $"/iam/api/v1/users/{userId}",
            storeKey: ScenarioDataKey.UserDeleteResponse,
            scenarioName: GetType().Name);
    }
}

internal static class UserDeleteScenarioTestExtensions
{
    public static ScenarioRunner UserDelete(this ScenarioRunner runner, string? userId = null)
        => runner.AddScenario(new UserDeleteScenarioTest(userId));
}