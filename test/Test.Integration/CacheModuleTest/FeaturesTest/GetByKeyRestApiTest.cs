using Test.Integration.CacheModuleTest.Requests;
using Test.Integration.Fixtures;
using Test.Integration.Fixtures.CacheTestScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.AccessControllFeatureTests;

[Collection("Collection tests v1")]
[Trait("CACHE", "get-by-key[REST]")]
public partial class GetByKeyRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public GetByKeyRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_bey_key()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .CacheSet(key: "key1", new CacheSetRequestTest(
                key: "key1",
                value: "value1", absoluteExpirationRelativeToNowBaseMinute: 10))
            .CacheGetByKey("key1");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = context.Get<ApiResultTest<string>>(ScenarioDataKey.CacheGetByKeyResponse);
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().Be("value1");
    }

    [Fact]
    public async Task Should_not_be_able_clear_all_cache_if_you_dont_have_access()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefaultUser()
            .LoginDefaultUser()
            .CacheClearAll();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.AssertionJwtForbidden();
    }
}
