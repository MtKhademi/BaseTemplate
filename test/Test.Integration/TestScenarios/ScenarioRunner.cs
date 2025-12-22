namespace Test.Integration.TestScenarios;

internal class ScenarioRunner
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly List<IScenarioStep> _scenarios = [];
    public ScenarioContext? Context { get; private set; }

    public ScenarioRunner(HttpClient client, ITestOutputHelper testOutputHelper) =>
        (_client, _testOutputHelper) = (client, testOutputHelper);

    public ScenarioRunner AddScenario(params IScenarioStep[] scenarios)
    {
        _scenarios.AddRange(scenarios);
        return this;
    }
    public async Task<ScenarioContext> RunAsync()
    {
        Context = new ScenarioContext(_client, _testOutputHelper);
        foreach (var scenario in _scenarios)
            await scenario.ExecuteAsync(Context);

        return Context;
    }
}
