namespace Test.Integration.Fixtures.TestScenarios;

internal interface IScenarioTest
{
    Task ExecuteAsync(ScenarioContext context);
}


internal interface IRestApiScenarioTest : IScenarioTest
{
    string ApiEndpoint { get; }
}