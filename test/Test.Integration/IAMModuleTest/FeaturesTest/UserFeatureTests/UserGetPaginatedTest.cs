using Test.Integration.Fixtures.IAMTestScenarios.UserScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.UserFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "user-get-paginated[REST]")]
public partial class UserGetPaginatedTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserGetPaginatedTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_users_if_current_user_is_admin()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserCreate(userName: "user1", password: "8585@8585", confirmPassword: "8585@8585")
            .UserCreate(userName: "user2", password: "8585@8585", confirmPassword: "8585@8585")
            .UserCreate(userName: "user3", password: "8585@8585", confirmPassword: "8585@8585")
            .UserCreate(new UserCreateRequestTest(
                Email: "user5@example.com",
                FirstName: "User5",
                LastName: "User5",
                UserName: "user5",
                PhoneNumber: "1234567895",
                Password: "PAS123@",
                ConfirmPassword: "PAS123@"))
            .UserGetPaginated();


        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<PaginatedListTest<ApplicationUserResponseTest>>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.TotalItems.Should().BeGreaterThanOrEqualTo(4);

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
}
