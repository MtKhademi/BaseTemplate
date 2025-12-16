using Test.Integration.Fixtures.IAMTestScenarios.AuthScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.AuthFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "Login[REST]")]
public partial class LoginRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public LoginRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData(null, null)]
    [InlineData("test@example.com", null)]
    [InlineData("test@example.com", "pass")]
    public async Task Should_not_be_able_login_when_not_send_correct_data(
        string? userName = default!,
        string? password = default!)
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginUser(userName, password);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("LoginRequestException");
    }

    [Fact]
    public async Task Should_not_be_able_login_when_user_does_not_exist()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginUser("testuser", "P@ssw0rd");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
        apiResult.ErrorKey.Should().Be("UserNotFoundWithUserNameException");
    }


    [Fact]
    public async Task Should_not_be_able_login_when_user_not_dont_correct_password()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefaultUser()
            .LoginUser("testuser", "P@ssw0rd123");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("UserNotCorrectPasswordException");
    }

    [Fact]
    public async Task Should_be_able_login()
    {
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .RegisterDefaultUser()
             .LoginUser("testuser", "P@ssw0rd");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<TokenResponseTest>>();
        apiResult.Should().NotBeNull();
        var userToken = apiResult.Result;
        userToken.Should().NotBeNull();
        userToken.Token.Should().NotBeNullOrEmpty();
        userToken.RefreshToken.Should().NotBeNullOrEmpty();
    }


    [Fact]
    public async Task Should_be_able_adminLogin()
    {
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<TokenResponseTest>>();
        apiResult.Should().NotBeNull();
        var userToken = apiResult.Result;
        userToken.Should().NotBeNull();
        userToken.Token.Should().NotBeNullOrEmpty();
        userToken.RefreshToken.Should().NotBeNullOrEmpty();
    }
}
