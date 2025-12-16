using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.RoleScenarios;

internal class RoleCreateScenarioTest(RoleCreateRequestTest request) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {

        var apiResult = await context.ApiPostRequestAsync<RoleCreateRequestTest, RoleResponseTest>(
            url: "/iam/api/v1/roles",
            storeKey: ScenarioDataKey.RoleCreate,
            request: request,
            scenarioName: GetType().Name);

        context.AddToList(ScenarioDataKey.Roles, apiResult);

    }
}

internal static class RoleCreateScenarioTestExtensions
{

    public static ScenarioRunner RoleCreate(this ScenarioRunner runner,
        string? roleName = default!,
        string? roleDescription = default!)
        => RoleCreate(runner, new RoleCreateRequestTest(Name: roleName, Description: roleDescription));

    public static ScenarioRunner RoleCreate(this ScenarioRunner runner, RoleCreateRequestTest request)
        => runner.AddScenario(new RoleCreateScenarioTest(request));
}