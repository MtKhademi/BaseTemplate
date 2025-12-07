using Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.AuthScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.RoleFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "role-get-byId[REST]")]
public partial class RoleGetByRoleId : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public RoleGetByRoleId(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_not_be_able_get_role_if_not_exist()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleGetById(RoleId: "role-id");

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
    public async Task Should_be_able_get_role_if_current_user_is_admin()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleCreate(roleName: "role-name", roleDescription: "role-description")
            .RoleGetById();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roleCreatedResult = context.Get<ApiResultTest<RoleResponseTest>>(ScenarioDataKey.RoleCreate);
        var roleGetByIdResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<RoleResponseTest>>();
        roleGetByIdResult.Should().NotBeNull();
        roleCreatedResult.Should().NotBeNull();

        roleCreatedResult.Result.Should().NotBeNull();
        roleGetByIdResult.Result.Should().NotBeNull();

        roleCreatedResult.Result!.RoleId.Should().Be(roleGetByIdResult.Result!.RoleId);
        roleCreatedResult.Result!.RoleName.Should().Be(roleGetByIdResult.Result!.RoleName);
        roleCreatedResult.Result!.RoleDescription.Should().Be(roleGetByIdResult.Result!.RoleDescription);

    }


    [Fact]
    public async Task Should_not_be_able_get_role_if_dont_login_user()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RoleGetById(RoleId: "123");

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
    public async Task Should_not_be_able_get_role_if_current_user_dont_have_access()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefault()
            .LoginDefaultUser()
            .RoleGetById(RoleId: "123");

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
