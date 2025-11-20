using Test.Integration.Fixtures.IAMModuleFixtures;

namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "role-get-paginated[REST]")]
public partial class RoleGetPaginatedTest : BaseTest
{
    private readonly string _api = $"/api/iam/v1/role";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public RoleGetPaginatedTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_roles_if_current_user_is_admin()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        for (int i = 1; i <= 5; i++)
        {
            await _client.IAMCreateRole(roleName: $"Role{i}", roleDescription: $"Description for Role{i}");
        }

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<PaginatedListTest<RoleResponseTest>>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.TotalItems.Should().BeGreaterThanOrEqualTo(6);

        var expectRole = apiResult.Result.Data.FirstOrDefault(u => u.Name == "Role5");
        expectRole.Should().NotBeNull();
        expectRole.Name.Should().Be("Role5");
        expectRole.Description.Should().Be("Description for Role5");
    }


    [Fact]
    public async Task Should_not_be_able_get_roles_if_dont_login_user()
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
    public async Task Should_not_be_able_get_roles_if_current_user_dont_have_access()
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
