using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.RoleScenarios;

internal class RoleDeleteScenarioTest(string? roleId = null) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (string.IsNullOrWhiteSpace(roleId))
        {
            var createdRoleResponse = context.Get<ApiResultTest<RoleResponseTest>>(ScenarioDataKey.RoleCreate);
            roleId = createdRoleResponse.Result.RoleId;
        }

        var apiResult = await context.ApiDeleteRequestAsync<bool>(
            url: $"/iam/api/v1/roles/{roleId}",
            storeKey: ScenarioDataKey.RoleDelete,
            scenarioName: GetType().Name);
       
    }
}

internal static class RoleDeleteScenarioTestExtensions
{
    public static ScenarioRunner RoleDelete(this ScenarioRunner runner, string? roleId = null)
    {
        return runner.AddScenario(new RoleDeleteScenarioTest(roleId));
    }
}