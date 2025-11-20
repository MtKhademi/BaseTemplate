using Test.Integration.Fixtures.IAMModuleFixtures;

namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "role-create[REST]")]
public partial class RoleCreateTest : BaseTest
{
    private readonly string _api = $"/api/iam/v1/role";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public RoleCreateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData("", "")]
    [InlineData(null, "desc")]
    public async Task Should_not_be_able_create_new_role_when_not_send_correct_data(string roleName, string roleDescription)
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        var request = new RoleCreateRequestTest
        {
            Name = roleName,
            Description = roleDescription
        };

        //-ACT
        var response = await _client.PostAsync(_api, request.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("RoleCreateCommandException");
    }

    [Theory]
    [InlineData("roleName", "")]
    [InlineData("roleName1", "desc")]
    [InlineData("Admin-role", "desc")]
    public async Task Should_be_able_create_new_role(string roleName, string roleDescription)
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        var request = new RoleCreateRequestTest
        {
            Name = roleName,
            Description = roleDescription
        };

        //-ACT
        var response = await _client.PostAsync(_api, request.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<RoleResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.Name.Should().Be(roleName);
        apiResult.Result.Description.Should().Be(roleDescription);
        apiResult.Result.Id.Should().NotBeEmpty();
    }


    [Fact]
    public async Task Should_not_be_able_create_a_role_when_already_exist_roleName()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        await _client.IAMCreateRole("roleName1", "desc");
        var request = new RoleCreateRequestTest
        {
            Name = "roleName1",
            Description = "desc"
        };

        //-ACT
        var response = await _client.PostAsync(_api, request.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
        apiResult.ErrorKey.Should().Be("RoleWithNameAlreadyExistException");
    }


    [Fact]
    public async Task Should_not_be_able_create_a_new_role_when_you_dont_login()
    {
        //-ARRANGE
        var request = new RoleCreateRequestTest
        {
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
    public async Task Should_not_be_able_create_a_new_role_when_you_dont_have_access()
    {
        //-ARRANGE
        await _client.IAMRegisterUserAndLoginUser();
        var request = new RoleCreateRequestTest
        {
            Name = "roleName",
            Description = "desc"
        };

        //-ACT
        var response = await _client.PostAsync(_api, request.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }
}
