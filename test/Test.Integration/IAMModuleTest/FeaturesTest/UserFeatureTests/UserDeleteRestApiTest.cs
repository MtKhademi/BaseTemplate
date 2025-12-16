using Test.Integration.Fixtures;
using Test.Integration.Fixtures.IAMTestScenarios.AuthScenarios;
using Test.Integration.Fixtures.IAMTestScenarios.UserScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.UserFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "user-delete[REST]")]
public partial class UserDeleteRestApiTest : BaseTest
{
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
        var createRequest = new UserCreateRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserCreate(createRequest)
            .UserDelete();


        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = context.Get<ApiResultTest>(ScenarioDataKey.UserDeleteResponse);
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Should_not_be_able_delete_a_user_when_not_exist()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserDelete("user-id-not-exist-12345");


        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("UserNotFoundWithUserIdException");
    }

    [Fact]
    public async Task Should_not_be_able_delete_a_user_when_dont_login()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .UserDelete("USER-ID");


        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("JwtRESTUnauthorizedException");
    }

    [Fact]
    public async Task Should_not_be_able_delete_a_user_when_dont_have_access()
    {
        //-ARRANGE
        var createRequest = new UserCreateRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserCreate(createRequest)
            .SignOut()
            .RegisterDefaultUser()
            .LoginDefaultUser()
            .UserDelete("user-id");


        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("JwtRESTForbiddenException");
    }

}
