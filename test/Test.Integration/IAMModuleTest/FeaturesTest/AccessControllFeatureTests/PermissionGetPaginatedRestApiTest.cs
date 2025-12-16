using Test.Integration.Fixtures;
using Test.Integration.Fixtures.IAMTestScenarios.AccessControll;
using Test.Integration.Fixtures.IAMTestScenarios.UserScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.AccessControllFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "access-controll-permissions-pagiated[REST]")]
public partial class PermissionGetPaginatedRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public PermissionGetPaginatedRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_permissions()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .PermissionGetPaginated();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = context.Get<ApiResultTest<PaginatedListTest<PermissionResponseTest>>>
            (ScenarioDataKey.AccessControll_PermissionGetPaginatedResponse);
        apiResult.Should().NotBeNull();
    }



    [Fact]
    public async Task Should_not_be_able_get_permissions_if_dont_have_access()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefaultUser()
            .LoginDefaultUser()
            .PermissionGetPaginated();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var apiResult = await response.Content.ReadFromJsonAsync<ApiResultTest>();
        apiResult.AssertionJwtForbidden();

    }
}
