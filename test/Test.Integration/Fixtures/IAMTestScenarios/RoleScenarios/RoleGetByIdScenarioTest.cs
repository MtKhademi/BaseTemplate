using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.RoleScenarios;

internal class RoleGetByIdScenarioTest(string? roleId = null) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (string.IsNullOrWhiteSpace(roleId))
        {
            var roleCreated = context.Get<ApiResultTest<RoleResponseTest>>(ScenarioDataKey.RoleCreate);
            if (roleCreated == null || roleCreated.Result is null || string.IsNullOrWhiteSpace(roleCreated.Result.RoleId))
            {
                context.TestOutputHelper.WriteLine(roleCreated.ToJson());
                throw new InvalidOperationException("No RoleId provided and no created Role found in context.");
            }
            roleId = roleCreated.Result.RoleId;
        }

        var apiResult = await context.ApiGetRequestAsync<RoleResponseTest>(
            url: $"/iam/api/v1/roles/{roleId}",
            storeKey: ScenarioDataKey.RoleGetByIdResponse,
            scenarioName: GetType().Name);
    }
}

internal static class RoleGetByIdScenarioTestExtensions
{
    public static ScenarioRunner RoleGetById(this ScenarioRunner runner, string? RoleId = null)
        => runner.AddScenario(new RoleGetByIdScenarioTest(RoleId));
}