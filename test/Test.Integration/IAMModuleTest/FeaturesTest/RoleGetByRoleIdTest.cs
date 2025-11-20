using Test.Integration.Fixtures.IAMModuleFixtures;

namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "role-get-byId[REST]")]
public partial class RoleGetByRoleId : BaseTest
{
    private readonly string _api = $"/api/iam/v1/role";
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
        await _client.IAMLoginAdmin();

        //-ACT
        var response = await _client.GetAsync($"{_api}/role_Id");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("RoleWithRoleIdNotFoundException");
    }


    [Fact]
    public async Task Should_be_able_get_role_if_current_user_is_admin()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        var role = await _client.IAMCreateRole(roleName: $"Role-test", roleDescription: $"Description for Role-test");

        //-ACT
        var response = await _client.GetAsync($"{_api}/{role.Id}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<RoleResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.Id.Should().Be(role.Id);
        apiResult.Result.Name.Should().Be(role.Name);
        apiResult.Result.Description.Should().Be(role.Description);
    }


    [Fact]
    public async Task Should_not_be_able_get_role_if_dont_login_user()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
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
        await _client.IAMRegisterUserAndLoginUser();

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("JwtRESTForbiddenException");
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.Forbidden);
    }
}
