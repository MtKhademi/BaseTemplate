using Test.Integration.Fixtures;
using Test.Integration.Fixtures.IAMTestScenarios.AccessControll;
using Test.Integration.Fixtures.IAMTestScenarios.UserScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.AccessControllFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "access-controll-module-pagiated[REST]")]
public partial class ModuleGetPaginatedRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public ModuleGetPaginatedRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_Modules()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .ModuleGetPaginated();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = context.Get<ApiResultTest<PaginatedListTest<ModuleResponseTest>>>
            (ScenarioDataKey.AccessControll_ModuleGetPaginatedResponse);
        apiResult.Should().NotBeNull();
    }



    [Fact]
    public async Task Should_not_be_able_get_Modules_if_dont_have_access()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefaultUser()
            .LoginDefaultUser()
            .ModuleGetPaginated();

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
