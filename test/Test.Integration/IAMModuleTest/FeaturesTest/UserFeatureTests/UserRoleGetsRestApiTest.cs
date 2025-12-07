namespace Test.Integration.IAMModuleTest.FeaturesTest.UserFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "user-get-roles[REST]")]
public partial class UserRoleGetsRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserRoleGetsRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_get_roles_for_admin()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserCreateDefault()
            .UserRoleGets();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<UserRoleResponseTest>>();
        apiResult.Should().NotBeNull();
        var userRoles = apiResult!.Result;
        userRoles.Should().NotBeNull();
        userRoles.Roles.Should().NotBeNullOrEmpty();
        foreach (var userRole in userRoles.Roles)
        {
            userRole.RoleId.Should().NotBeNullOrEmpty();
            userRole.RoleName.Should().Be("Basic");
        }
    }
}
