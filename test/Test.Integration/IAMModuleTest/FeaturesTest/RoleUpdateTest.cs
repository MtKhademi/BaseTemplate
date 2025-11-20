using Test.Integration.Fixtures.IAMModuleFixtures;

namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "role-update[REST]")]
public partial class RoleUpdateTest : BaseTest
{
    private readonly string _api = $"/api/iam/v1/role";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public RoleUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData("", "", "")]
    [InlineData(null, null, "desc")]
    [InlineData(null, "", "desc")]
    public async Task Should_not_be_able_update_a_role_when_not_send_correct_data(
        string roleId, string roleName, string roleDescription)
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        var request = new RoleUpdateRequestTest
        {
            RoleId = roleId,
            Name = roleName,
            Description = roleDescription
        };

        //-ACT
        var response = await _client.PutAsync(_api, request.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("RoleUpdateCommandException");
    }

    [Fact]
    public async Task Should_be_able_update_a_role()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        var role = await _client.IAMCreateRole("roleName1", "desc");
        var request = new RoleUpdateRequestTest
        {
            RoleId = role.Id,
            Name = "role-new-name",
            Description = "role-new-desc"
        };

        //-ACT
        var response = await _client.PutAsync(_api, request.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<RoleResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.Name.Should().Be(request.Name);
        apiResult.Result.Description.Should().Be(request.Description);
        apiResult.Result.Id.Should().NotBeEmpty();
        apiResult.Result.Id.Should().Be(role.Id);
    }

    [Fact]
    public async Task Should_not_be_able_update_a_role_when_exist_this_roleName_in_another_role()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        var role = await _client.IAMCreateRole("roleName1", "desc");
        var role2 = await _client.IAMCreateRole("roleName2", "desc");
        var request = new RoleUpdateRequestTest
        {
            RoleId = role.Id,
            Name = "roleName2",
            Description = "role-new-desc"
        };

        //-ACT
        var response = await _client.PutAsync(_api, request.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_not_be_able_update_a_role_when_not_exist_role()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        var request = new RoleUpdateRequestTest
        {
            RoleId = "role.Id",
            Name = "roleName1",
            Description = "desc"
        };

        //-ACT
        var response = await _client.PutAsync(_api, request.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
        apiResult.ErrorKey.Should().Be("RoleWithRoleIdNotFoundException");
    }


    [Fact]
    public async Task Should_not_be_able_update_a_new_role_when_you_dont_login()
    {
        //-ARRANGE
        var request = new RoleUpdateRequestTest
        {
            RoleId = "roleId1",
            Name = "roleName",
            Description = "desc"
        };

        //-ACT
        var response = await _client.PostAsync(_api, request.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }


    [Fact]
    public async Task Should_not_be_able_update_a_new_role_when_you_dont_have_access()
    {
        //-ARRANGE
        await _client.IAMRegisterUserAndLoginUser();
        var request = new RoleUpdateRequestTest
        {
            RoleId = "roleId1",
            Name = "roleName",
            Description = "desc"
        };

        //-ACT
        var response = await _client.PutAsync(_api, request.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }
}
