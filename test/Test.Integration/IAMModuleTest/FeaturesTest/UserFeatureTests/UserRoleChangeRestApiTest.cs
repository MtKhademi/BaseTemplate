using Test.Integration.Fixtures;
using Test.Integration.Fixtures.IAMTestScenarios.RoleScenarios;
using Test.Integration.Fixtures.IAMTestScenarios.UserScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.UserFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "user-role-change[REST]")]
public partial class UserRoleChangeRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserRoleChangeRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_update_a_role_user()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleCreate(roleName: "ROLE-1", roleDescription: "Role 1 description")
            .RoleCreate(roleName: "ROLE-2", roleDescription: "Role 2 description")
            .RoleCreate(roleName: "NEW-ROLE", roleDescription: "New Role description")
            .UserCreate(new UserCreateRequestTest
            {
                Email = "test2@example.com",
                UserName = "testuser2",
                Password = "P@ssw0rd",
                ConfirmPassword = "P@ssw0rd",
                PhoneNumber = "12345678912",
                FirstName = "Test",
                LastName = "User"
            })
            .UserRoleChange(new UserRolesChangeRequestTest(),
                userSelecte: user => user.UserName == "testuser2",
                roleSelecte: role => role.RoleName == "ROLE-2")
            .UserRoleGets();
        //-ACT
        var context = await runner.RunAsync();


        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResultUserRoles = context.Get<ApiResultTest<UserRoleResponseTest>>(ScenarioDataKey.UserRoleGetsResponse);
        
        apiResultUserRoles.Should().NotBeNull();
        var userRoles = apiResultUserRoles.Result;  
        userRoles.Should().NotBeNull();
        userRoles.UserId.Should().NotBeNull();
        userRoles.UserName.Should().Be("testuser2");

        userRoles.Roles.Should().NotBeNull();
        userRoles.Roles.Should().HaveCount(2);
        userRoles.Roles!.Should().Contain(role => role.RoleName == "ROLE-2");
        userRoles.Roles!.Should().Contain(role => role.RoleName == "Basic");
    }
}
