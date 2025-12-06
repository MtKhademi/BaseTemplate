using Castle.Components.DictionaryAdapter.Xml;
using IAMModule.Contract.Responses;
using Test.Integration.Fixtures.IAMModuleFixtures;

namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "user-role-change[REST]")]
public partial class UserRoleChangeRestApiTest : BaseTest
{
    private readonly string _api = $"/api/iam/v1/user/[user-id]/role-change/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserRoleChangeRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        //_client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_update_a_role_user()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        var newRole = await _client.IAMCreateRole(roleName: "NEW-ROLE", roleDescription: "New Role description");
        var user = await _client.IAMCreateAUser(new UserCreateRequestTest
        {
            Email = "test@example.com",
            UserName = "testuser",
            Password = "P@ssw0rd",
            ConfirmPassword = "P@ssw0rd",
            PhoneNumber = "1234567890",
            FirstName = "Test",
            LastName = "User"
        });
        var apiGetRoleAddress = "/api/IAM/v1/user/[user-name]/roles";
        var response = await _client.GetAsync(apiGetRoleAddress.Replace("[user-name]", user.UserName));
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadModelFromJsonAsync<ApiResultTest<List<UserRoleResponse>>>();
        result.Should().NotBeNull();
        result.Result.Should().HaveCount(1);

        //-ACT
        response = await _client.PutAsync(_api.Replace("[user-id]", user.UserId),
            new UserRolesChangeRequestTest(UserId: user.UserId, RoleIds: [newRole.Id]).ToContentHttp());

        //-ASSERT
        response = await _client.GetAsync(apiGetRoleAddress.Replace("[user-name]", user.UserName));
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result = await response.Content.ReadModelFromJsonAsync<ApiResultTest<List<UserRoleResponse>>>();
        result.Should().NotBeNull();
        result.Result.Should().HaveCount(2);
        result.Result.Where(x => x.RoleName == "NEW-ROLE").Should().HaveCount(1);
    }
}
