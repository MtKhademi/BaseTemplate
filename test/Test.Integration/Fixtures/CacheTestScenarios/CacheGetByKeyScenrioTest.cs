namespace Test.Integration.Fixtures.CacheTestScenarios;

internal class CacheGetByKeyScenrioTest(string key) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiGetRequestAsync<string>(
            url: $"/cache/api/v1/{key}",
            storeKey: ScenarioDataKey.CacheGetByKeyResponse,
            scenarioName: GetType().Name);
    }
}


internal static class CacheGetByKeyUserScenrioTestExtensions
{
    public static ScenarioRunner CacheGetByKey(this ScenarioRunner runner,string key)
        => runner.AddScenario(new CacheGetByKeyScenrioTest(key));
}