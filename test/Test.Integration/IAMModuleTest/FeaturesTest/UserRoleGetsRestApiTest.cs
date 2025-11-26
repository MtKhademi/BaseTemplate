using Test.Integration.Fixtures.IAMModuleFixtures;

namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "user-get-roles[REST]")]
public partial class UserRoleGetsRestApiTest : BaseTest
{
    private readonly string _api = $"/api/IAM/v1/user/[user-name]/roles";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserRoleGetsRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        //_client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_get_roles_for_admin()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();

        //-ACT
        var response = await _client.GetAsync(_api.Replace("[user-name]", "admin"));
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<IEnumerable<UserRoleResponseTest>>>();
        apiResult.Should().NotBeNull();
        var userRoles = apiResult!.Result;
        userRoles.Should().NotBeNull();
        userRoles.Should().HaveCount(2);
        foreach (var userRole in userRoles)
        {
            userRole.UserName.Should().Be("admin"); 
        }

        userRoles.Should().ContainSingle(ur => ur.RoleName == "Basic"); // check exist roles basic
        userRoles.Should().ContainSingle(ur => ur.RoleName == "Admin"); // check exist roles admin

    }

    [Fact]
    public async Task Should_be_able_get_roles_for_basicUser()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();

        //-ACT
        var response = await _client.GetAsync(_api.Replace("[user-name]", "admin"));
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<IEnumerable<UserRoleResponseTest>>>();
        apiResult.Should().NotBeNull();
        var userRoles = apiResult!.Result;
        userRoles.Should().NotBeNull();
        userRoles.Should().HaveCount(2);
        foreach (var userRole in userRoles)
        {
            userRole.UserName.Should().Be("admin");
        }

        userRoles.Should().ContainSingle(ur => ur.RoleName == "Basic"); // check exist roles basic
        userRoles.Should().ContainSingle(ur => ur.RoleName == "Admin"); // check exist roles admin

    }
}
