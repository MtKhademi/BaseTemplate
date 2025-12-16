using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.UserScenarios;

internal class UserChangeStateActiveScenarioTest(string? userId = null) : IScenarioStep
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


        var apiResult = await context.ApiPatchRequestAsync<ApplicationUserResponseTest>(
            url: "/iam/api/v1/users/[userId]/change-state".Replace("[userId]", userId),
            storeKey: ScenarioDataKey.UserGetByIdResponse,
            scenarioName: GetType().Name);
       
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