namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.RoleScenarios;

internal class RoleDeleteScenarioTest : IRestApiScenarioTest
{
    public string ApiEndpoint => $"/iam/api/v1/roles";
    public string? RoleId { get; private set; }
    public RoleDeleteScenarioTest(string? roleId = null)
    {
        RoleId = roleId;
    }

    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (string.IsNullOrWhiteSpace(RoleId))
        {
            var createdRoleResponse = context.Get<ApiResultTest<RoleResponseTest>>(ScenarioDataKey.RoleCreate);
            RoleId = createdRoleResponse.Result.RoleId;
        }

        var response = await context.Client.DeleteAsync($"{ApiEndpoint}/{RoleId}");
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Role Delete");

        context.SetLastResponse(response);

        if(!response.IsSuccessStatusCode)
            return;
    }
}

internal static class RoleDeleteScenarioTestExtensions
{
    public static ScenarioRunner RoleDelete(this ScenarioRunner runner, string? roleId = null)
    {
        return runner.AddScenario(new RoleDeleteScenarioTest(roleId));
    }
}