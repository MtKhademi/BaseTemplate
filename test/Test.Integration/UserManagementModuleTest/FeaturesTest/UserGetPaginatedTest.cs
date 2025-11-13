using Test.Integration.Fixtures.UserManagementModuleFixtures;

namespace Test.Integration.UserManagementModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("UserManagement", "user[REST]")]
public partial class UserGetPaginatedTest : BaseTest
{
    private readonly string _api = $"/api/user-management/v1/user";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserGetPaginatedTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        //_client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_users_if_current_user_is_admin()
    {
        //-ARRANGE
        for (int i = 1; i <= 5; i++)
        {
            await _client.IAMRegister(
                email: $"user{i}@example.com",
                userName: $"user{i}",
                phoneNumber: $"123456789{i}",
                firstName: $"User{i}",
                lastName: $"User{i}");
        }

        await _client.IAMLoginAdmin();

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<PaginatedListTest<ApplicationUserResponseTest>>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.TotalItems.Should().BeGreaterThanOrEqualTo(6);

        var expectUser = apiResult.Result.Data.FirstOrDefault(u => u.Email == "user5@example.com");
        expectUser.Should().NotBeNull();
        expectUser.Email.Should().Be("user5@example.com");
        expectUser.FirstName.Should().Be("User5");
        expectUser.LastName.Should().Be("User5");
        expectUser.UserName.Should().Be("user5");
        expectUser.IsActive.Should().BeTrue();
        expectUser.EmailConfirmed.Should().BeFalse();
        expectUser.PhoneNumber.Should().Be("1234567895");
    }


    [Fact]
    public async Task Should_not_be_able_get_users_if_dont_login_user()
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
    public async Task Should_not_be_able_get_users_if_current_user_dont_have_access()
    {
        //-ARRANGE
        await _client.IAMRegister(); // create default user
        await _client.IAMLoginUser(); // login default user

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
