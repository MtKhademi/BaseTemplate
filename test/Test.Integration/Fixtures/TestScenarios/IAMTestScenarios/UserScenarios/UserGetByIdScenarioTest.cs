namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

internal class UserGetByIdScenarioTest : IRestApiScenarioTest
{
    public string? UserId { get; private set; }

    public string ApiEndpoint => $"/iam/api/v1/users";

    public UserGetByIdScenarioTest(string? userId = null)
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

        var response = await context.Client.GetAsync($"{ApiEndpoint}/{UserId}");
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Get User By Id");
        context.SetLastResponse(response);

        if(!response.IsSuccessStatusCode)
        {
            return;
        }
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