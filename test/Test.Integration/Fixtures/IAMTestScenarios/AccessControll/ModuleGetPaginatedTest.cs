using Test.Infrastructure.Requestes;
using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.AccessControll;

internal class ModuleGetPaginatedScenrioTest(ModuleGetPaginatedRequestTest? request = default!) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiGetRequestAsync<PaginatedListTest<ModuleResponseTest>>(
            url: $"/iam/api/v1/access-controll/Modules?{request?.ToQueryString()}",
            storeKey: ScenarioDataKey.AccessControll_ModuleGetPaginatedResponse,
            scenarioName: GetType().Name);
    }
}


internal static class ModuleGetPaginatedUserScenrioTestExtensions
{
    public static ScenarioRunner ModuleGetPaginated(this ScenarioRunner runner,
        ModuleGetPaginatedRequestTest? request = default!)
        => runner.AddScenario(new ModuleGetPaginatedScenrioTest(request));
}