using Test.Integration.CacheModuleTest.Requests;
using Test.Integration.Fixtures;
using Test.Integration.Fixtures.CacheTestScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.AccessControllFeatureTests;

[Collection("Collection tests v1")]
[Trait("CACHE", "set[REST]")]
public partial class SetRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public SetRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Shoud_be_able_set_a_new_cache()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .CacheSet("key-test", new CacheSetRequestTest(key: "key-test", value: "value-test"));

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = context.Get<ApiResultTest<bool>>(ScenarioDataKey.CacheSetResponse);
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().BeTrue();
    }

    [Fact]
    public async Task Should_not_be_able_set_when_not_access()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefaultUser()
            .LoginDefaultUser()
            .CacheSet("key-test", new CacheSetRequestTest(key: "key-test", value: "value-test"));

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
