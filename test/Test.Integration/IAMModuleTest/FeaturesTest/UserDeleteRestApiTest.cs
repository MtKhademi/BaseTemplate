using Test.Integration.Fixtures.IAMModuleFixtures;

namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "user-delete[REST]")]
public partial class UserDeleteRestApiTest : BaseTest
{
    private readonly string _api = $"/api/iam/v1/user";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserDeleteRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_delete_a_user()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        var createRequest = new UserCreateRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        var user = await _client.IAMCreateAUser(createRequest);

        //-ACT
        var response = await _client.DeleteAsync($"{_api}/{user.UserId}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_not_be_able_delete_a_user_when_not_exist()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();

        //-ACT
        var response = await _client.DeleteAsync($"{_api}/USER-ID");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("UserNotFoundWithUserIdException");
    }

    [Fact]
    public async Task Should_not_be_able_delete_a_user_when_dont_login()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.DeleteAsync($"{_api}/USER-ID");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("JwtRESTUnauthorizedException");
    }

    [Fact]
    public async Task Should_not_be_able_delete_a_user_when_dont_have_access()
    {
        //-ARRANGE
        await _client.IAMRegisterUserAndLoginUser();

        //-ACT
        var response = await _client.DeleteAsync($"{_api}/USER-ID");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("JwtRESTForbiddenException");
    }

}
