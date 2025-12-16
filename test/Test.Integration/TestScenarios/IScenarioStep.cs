namespace Test.Integration.TestScenarios;

internal interface IScenarioStep
{
    Task ExecuteAsync(ScenarioContext context);
}
