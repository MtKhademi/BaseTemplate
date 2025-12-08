namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

internal class UserGetByIdScenarioTest(string? userId = null) : IScenarioStep
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


        var apiResult = await context.ApiGetRequestAsync<ApplicationUserResponseTest>(
            url: $"/iam/api/v1/users/{userId}",
            storeKey: ScenarioDataKey.UserGetByIdResponse,
            scenarioName: GetType().Name);

        context.AddToList(ScenarioDataKey.Users, apiResult);

    }
}

internal static class UserGetByIdScenarioTestExtensions
{
    public static ScenarioRunner UserGetById(this ScenarioRunner runner,
        string? userId = null)
    {
        return runner.AddScenario(new UserGetByIdScenarioTest(userId));
    }
}