namespace Test.Integration.Fixtures.CacheTestScenarios;

internal class CacheClearAllScenrioTest() : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiDeleteRequestAsync<bool>(
            url: $"/cache/api/v1",
            storeKey: ScenarioDataKey.CacheClearAllResponse,
            scenarioName: GetType().Name);
    }
}


internal static class CacheClearAllUserScenrioTestExtensions
{
    public static ScenarioRunner CacheClearAll(this ScenarioRunner runner)
        => runner.AddScenario(new CacheClearAllScenrioTest());
}