using Test.Integration.Fixtures.IAMTestScenarios.AuthScenarios;
using Test.Integration.Fixtures.IAMTestScenarios.RoleScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.RoleFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "role-delete[REST]")]
public partial class RoleDeleteTestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public RoleDeleteTestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_not_be_able_delete_a_role_if_not_exist()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleDelete("role-id");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("RoleWithRoleIdNotFoundException");
    }


    [Fact]
    public async Task Should_be_able_delete()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleCreate(roleName: "Role-to-delete", roleDescription: "Description for Role-to-delete")
            .RoleDelete();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeTrue();
    }


    [Fact]
    public async Task Should_not_be_able_delete_a_role_if_dont_login_user()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RoleDelete(roleId: "role-id");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("JwtRESTUnauthorizedException");
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.UnAuthorization);
    }


    [Fact]
    public async Task Should_not_be_able_delete_a_role_if_current_user_dont_have_access()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefaultUser()
            .LoginDefaultUser()
            .RoleDelete(roleId: "role-id");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("JwtRESTForbiddenException");
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.Forbidden);
    }
}
