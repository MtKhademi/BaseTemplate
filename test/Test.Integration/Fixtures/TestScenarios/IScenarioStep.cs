namespace Test.Integration.Fixtures.TestScenarios;

internal interface IScenarioStep
{
    Task ExecuteAsync(ScenarioContext context);
}
