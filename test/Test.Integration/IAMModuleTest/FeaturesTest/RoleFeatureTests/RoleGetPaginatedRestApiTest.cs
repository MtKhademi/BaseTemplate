namespace Test.Integration.IAMModuleTest.FeaturesTest.RoleFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "role-get-paginated[REST]")]
public partial class RoleGetPaginatedRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public RoleGetPaginatedRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_roles_if_current_user_is_admin()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleCreate(roleName: "Role1", roleDescription: "Description for Role1")
            .RoleCreate(roleName: "Role2", roleDescription: "Description for Role2")
            .RoleCreate(roleName: "Role3", roleDescription: "Description for Role3")
            .RoleCreate(roleName: "Role4", roleDescription: "Description for Role4")
            .RoleCreate(roleName: "Role5", roleDescription: "Description for Role5")
            .RoleGetPaginated();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<PaginatedListTest<RoleResponseTest>>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.TotalItems.Should().BeGreaterThanOrEqualTo(6);

        var expectRole = apiResult.Result.Data.FirstOrDefault(u => u.RoleName == "Role5");
        expectRole.Should().NotBeNull();
        expectRole.RoleName.Should().Be("Role5");
        expectRole.RoleDescription.Should().Be("Description for Role5");
    }


    [Fact]
    public async Task Should_not_be_able_get_roles_if_dont_login_user()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RoleGetPaginated();

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
    public async Task Should_not_be_able_get_roles_if_current_user_dont_have_access()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleCreate(roleName: "Role1", roleDescription: "Description for Role1")
            .SignOut()
            .RegisterDefault()
            .LoginDefaultUser()
            .RoleGetPaginated();

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
