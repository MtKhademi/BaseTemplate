using Test.Integration.Fixtures.IAMModuleFixtures;
using Test.Integration.Fixtures.TestScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "Login[REST]")]
public partial class LoginRestApiTest : BaseTest
{
    private readonly string _api = $"/api/iam/v1/login";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public LoginRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        //_client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
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
        var testScenarioRunner = new ScenarioRunner(_client, _outPutHelper);
        var dto = new LoginRequestTest(
            UserName: userName,
            Password: password
        );

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("LoginRequestException");
    }

    [Fact]
    public async Task Should_not_be_able_login_when_user_does_not_exist()
    {
        //-ARRANGE
        var dto = new LoginRequestTest(
            UserName: "testuser",
            Password: "P@ssw0rd");

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
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

        var dto = new LoginRequestTest(UserName: "testuser", Password: "P@ssasdssssd");

        await _client.IAMRegister(
            userName: "testuser",
            password: "testuser",
            confirmPassword: "testuser");

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("UserNotCorrectPasswordException");
    }

    [Fact]
    public async Task Should_be_able_login()
    {
        //-ARRANGE
        var dto = new LoginRequestTest(
            UserName: "testuser",
            Password: "P@ssasdssssd");

        await _client.IAMRegister(
            userName: "testuser",
            password: "P@ssasdssssd",
            confirmPassword: "P@ssasdssssd");

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
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
        //-ARRANGE
        var dto = new LoginRequestTest(
            UserName: "admin",
            Password: "8585@8585");

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<TokenResponseTest>>();
        apiResult.Should().NotBeNull();
        var userToken = apiResult.Result;
        userToken.Should().NotBeNull();
        userToken.Token.Should().NotBeNullOrEmpty();
        userToken.RefreshToken.Should().NotBeNullOrEmpty();
    }
}
