namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "user-get-by-id[REST]")]
public partial class UserGetByIdRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserGetByIdRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_user()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .AddLoginAdminScenario()
            .AddUserCreateScenario(userName: "testuser1")
            .AddUserGetByIdScenario();


        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var userCreateResult = context.Get<ApiResultTest<ApplicationUserResponseTest>>(ScenarioDataKey.UserCreate)?.Result ?? null;
        var userGetByIdResult = context.Get<ApiResultTest<ApplicationUserResponseTest>>(ScenarioDataKey.UserGetByIdResponse)?.Result ?? null;
        userCreateResult.Should().NotBeNull();
        userGetByIdResult.Should().NotBeNull();

        userGetByIdResult!.UserId.Should().Be(userCreateResult!.UserId);
        userGetByIdResult!.UserName.Should().Be(userCreateResult!.UserName);
        userGetByIdResult!.Email.Should().Be(userCreateResult!.Email);
        userGetByIdResult!.FirstName.Should().Be(userCreateResult!.FirstName);
        userGetByIdResult!.LastName.Should().Be(userCreateResult!.LastName);
        userGetByIdResult!.PhoneNumber.Should().Be(userCreateResult!.PhoneNumber);
        userGetByIdResult!.IsActive.Should().Be(userCreateResult!.IsActive);
        userGetByIdResult!.EmailConfirmed.Should().Be(userCreateResult!.EmailConfirmed);
    }

    [Fact]
    public async Task Should_not_be_able_get_user_when_not_exist()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .AddLoginAdminScenario()
            .AddUserGetByIdScenario(userId: "123");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
        apiResult.ErrorKey.Should().Be("IamUserNotFoundException");

    }


    [Fact]
    public async Task Should_not_be_able_get_user_if_dont_login_user()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .AddUserGetByIdScenario(userId: "123");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.UnAuthorization);
        apiResult.ErrorKey.Should().Be("JwtRESTUnauthorizedException");
    }
}
