using Test.Integration.Fixtures;

public class ScenarioContext
{
    private readonly Dictionary<string, object> _items = new();

    public HttpClient Client { get; }
    public ITestOutputHelper TestOutputHelper { get; }
    public HttpResponseMessage? LastResponse { get; private set; }

    public ScenarioContext(HttpClient client, ITestOutputHelper testOutputHelper)
    {
        Client = client;
        TestOutputHelper = testOutputHelper;
    }

    public T? Get<T>(ScenarioDataKey key) =>
        _items.TryGetValue(key.ToString(), out var val) ? (T?)val : default;

    public void Set<T>(ScenarioDataKey key, T value) =>
        _items[key.ToString()] = value!;

    public void SetLastResponse(HttpResponseMessage response) => LastResponse = response;

    public void AddToList<T>(ScenarioDataKey key, ApiResultTest<T>? apiResult)
    {
        if (apiResult is null || apiResult.Result is null)
            return;

        AddToList(key, apiResult!.Result!);
    }

    public void AddToList<T>(ScenarioDataKey key, T value)
    {
        if (value is null)
            return;

        var type = typeof(T);

        // 🔍 PaginatedListTest<X>
        if (type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(PaginatedListTest<>))
        {
            var itemType = type.GetGenericArguments()[0];
            var itemsProperty = type.GetProperty("Items");

            if (itemsProperty?.GetValue(value) is not System.Collections.IEnumerable items)
                return;

            var listType = typeof(List<>).MakeGenericType(itemType);

            if (!_items.TryGetValue(key.ToString(), out var existingList)
                || existingList.GetType() != listType)
            {
                existingList = Activator.CreateInstance(listType)!;
                _items[key.ToString()] = existingList;
            }

            var addMethod = listType.GetMethod("Add")!;

            foreach (var item in items)
                addMethod.Invoke(existingList, new[] { item });

            return;
        }

        // ✅ Non-paginated
        if (!_items.TryGetValue(key.ToString(), out var existingObj)
            || existingObj is not List<T> list)
        {
            list = new List<T>();
            _items[key.ToString()] = list;
        }

        list.Add(value);
    }


    public List<T> GetList<T>(ScenarioDataKey key) =>
        _items.TryGetValue(key.ToString(), out var obj) && obj is List<T> list ? list : new List<T>();
}