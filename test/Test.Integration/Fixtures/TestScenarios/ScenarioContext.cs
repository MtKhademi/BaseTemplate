namespace Test.Integration.Fixtures.TestScenarios;

internal class ScenarioContext
{
    private readonly Dictionary<string, string> _data = new();

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
        return _data.TryGetValue(key.ToString(), out var value) ? value.ToModel<T>() : default!;
    }
    public void Set<T>(ScenarioDataKey key, T value)
    {
        _data[key.ToString()] = value!.ToJson();
    }

    internal void RecreateList<T>(ScenarioDataKey key,List<T> values)
    {
        _data[key.ToString()] = values.ToJson();
    }
    public void AddToList<T>(ScenarioDataKey key, T value)
    {
        var list = new List<T>();
        if (_data.TryGetValue(key.ToString(), out var listObj))
        {
            list = listObj.ToModel<List<T>>() ?? new List<T>();
        }
        list.Add(value);
        _data[key.ToString()] = list.ToJson();
    }
    public List<T> GetList<T>(ScenarioDataKey key) =>
        _data.TryGetValue(key.ToString(), out var obj) ? obj.ToModel<List<T>>() : new List<T>();

    public void SetLastResponse(HttpResponseMessage response)
    {
        LastResponse = response;
    }

}

public enum ScenarioDataKey
{
    None,
    CurrentUser,
    Register,

    Users,
    UserCreate,
    UserGetByIdResponse,
    UserDeleteResponse,
    UserRoleGetsResponse,

    Roles,
    RoleCreate,
    RoleDelete,
    RoleGetPaginated,
    RoleGetByIdResponse,
    UserRoleChangeResponse,
    RoleUpdate,
    UserUpdate,
}
