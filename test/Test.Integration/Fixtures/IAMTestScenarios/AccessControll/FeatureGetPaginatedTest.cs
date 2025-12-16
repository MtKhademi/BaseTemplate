namespace Test.Integration.Fixtures.IAMTestScenarios.AccessControll;

internal class FeatureGetPaginatedScenrioTest(FeatureGetPaginatedRequestTest? request = default!) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiGetRequestAsync<PaginatedListTest<FeatureResponseTest>>(
            url: $"/iam/api/v1/access-controll/Features?{request?.ToQueryString()}",
            storeKey: ScenarioDataKey.AccessControll_FeatureGetPaginatedResponse,
            scenarioName: GetType().Name);
    }
}


internal static class FeatureGetPaginatedUserScenrioTestExtensions
{
    public static ScenarioRunner FeatureGetPaginated(this ScenarioRunner runner,
        FeatureGetPaginatedRequestTest? request = default!)
        => runner.AddScenario(new FeatureGetPaginatedScenrioTest(request));
}