namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.RoleScenarios;

internal class RoleGetByIdScenarioTest : IRestApiScenarioTest
{
    public string? RoleId { get; private set; }

    public string ApiEndpoint => $"/iam/api/v1/roles";

    public RoleGetByIdScenarioTest(string? roleId = null)
    {
        RoleId = roleId;
    }
    public async Task ExecuteAsync(ScenarioContext context)
    {

        if (string.IsNullOrWhiteSpace(RoleId))
        {
            var roleCreated = context.Get<ApiResultTest<RoleResponseTest>>(ScenarioDataKey.RoleCreate);
            if (roleCreated == null || roleCreated.Result is null || string.IsNullOrWhiteSpace(roleCreated.Result.RoleId))
            {
                context.TestOutputHelper.WriteLine(roleCreated.ToJson());
                throw new InvalidOperationException("No RoleId provided and no created Role found in context.");
            }
            RoleId = roleCreated.Result.RoleId;
        }

        var response = await context.Client.GetAsync($"{ApiEndpoint}/{RoleId}");
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Get Role By Id");
        context.SetLastResponse(response);


        if (!response.IsSuccessStatusCode)
            return;

        var roleResponse = await response.Content.ReadFromJsonAsync<ApiResultTest<RoleResponseTest>>();
        context.Set(ScenarioDataKey.RoleGetByIdResponse, roleResponse);
    }
}

internal static class RoleGetByIdScenarioTestExtensions
{
    public static ScenarioRunner RoleGetById(this ScenarioRunner runner, string? RoleId = null)
        => runner.AddScenario(new RoleGetByIdScenarioTest(RoleId));
}