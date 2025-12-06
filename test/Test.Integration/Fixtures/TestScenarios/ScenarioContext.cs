using FluentAssertions;

namespace Test.Integration.Fixtures.TestScenarios;

internal class ScenarioContext
{
    private readonly Dictionary<ScenarioDataKey, string> _data = new();

    public HttpClient Client { get; init; }
    public ITestOutputHelper TestOutputHelper { get; init; }
    public HttpResponseMessage? LastResponse { get; private set; }
    public ScenarioContext(HttpClient client, ITestOutputHelper testOutputHelper)
    {
        Client = client;
        TestOutputHelper = testOutputHelper;
    }


    public T? Get<T>(ScenarioDataKey key)
    {
        return _data.TryGetValue(key, out var value) ? value.ToModel<T>() : default!;
    }

    public void Set<T>(ScenarioDataKey key, T value)
    {
        _data[key] = value!.ToJson();
    }

    public async Task SetApiResultAsync(ScenarioDataKey key, HttpResponseMessage response)
    {
        _data[key] = await response.Content.ReadAsStringAsync();
    }

    public void SetLastResponse(HttpResponseMessage response)
    {
        LastResponse = response;
    }

}

public enum ScenarioDataKey
{
    None,
    CurrentUser,
    UserCreate,
    UserGetByIdResponse,
}
