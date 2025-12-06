namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "Signout[REST]")]
public partial class SignOutRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public SignOutRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_singout()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper);
        runner.AddScenario(
            new LoginAdminScenarioTest(),
            new SignOutScenrioTest()
        );


        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<bool>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().BeTrue();
    }


    [Fact]
    public async Task Should_not_be_able_access_after_signout()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper);
        runner.AddScenario(
            new LoginAdminScenarioTest(),
            new SignOutScenrioTest(),
            new UserGetByIdScenarioTest(userId: "123")
        );


        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.UnAuthorization);
        apiResult.ErrorKey.Should().Be("JwtRESTUnauthorizedException");
    }
}
