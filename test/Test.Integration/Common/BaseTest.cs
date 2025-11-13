namespace Test.Integration.Common;

public class BaseTest : IAsyncLifetime
{
    private readonly WebAppFactory _factory;

    public BaseTest(WebAppFactory factory)
    {
        _factory = factory;
    }
    
    public async Task DisposeAsync()
    {
        await _factory.ClearDbAsync();
    }

    public async Task InitializeAsync()
    {
        await _factory.InitializeAsync();
    }
}
