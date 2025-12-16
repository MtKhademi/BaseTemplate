using Test.Infrastructure.Requestes;
using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.AccessControll;

internal class PermissionGetPaginatedScenrioTest(PermissionGetPaginatedRequestTest? request = default!) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiGetRequestAsync<PaginatedListTest<PermissionResponseTest>>(
            url: $"/iam/api/v1/access-controll/permissions?{request?.ToQueryString()}",
            storeKey: ScenarioDataKey.AccessControll_PermissionGetPaginatedResponse,
            scenarioName: GetType().Name);
    }
}


internal static class PermissionGetPaginatedUserScenrioTestExtensions
{
    public static ScenarioRunner PermissionGetPaginated(this ScenarioRunner runner,
        PermissionGetPaginatedRequestTest? request = default!)
        => runner.AddScenario(new PermissionGetPaginatedScenrioTest(request));
}