using Test.Integration.CacheModuleTest.Requests;

namespace Test.Integration.Fixtures.CacheTestScenarios;

internal class CacheSetScenrioTest(string key, CacheSetRequestTest request) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiPostRequestAsync<CacheSetRequestTest, bool>(
            url: $"/cache/api/v1/{key}",
            request: request,
            storeKey: ScenarioDataKey.CacheSetResponse,
            scenarioName: GetType().Name);
    }
}


internal static class CacheSetUserScenrioTestExtensions
{
    public static ScenarioRunner CacheSet(this ScenarioRunner runner, string key, CacheSetRequestTest request)
        => runner.AddScenario(new CacheSetScenrioTest(key, request));
}