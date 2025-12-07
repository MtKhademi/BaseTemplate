namespace Test.Integration.IAMModuleTest.FeaturesTest.UserFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "user-change-state-active[REST]")]
public partial class UserChangeStateActiveRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserChangeStateActiveRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_change_state_user()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserCreateDefault()
            .UserChangeStateActive();

        //-ACT
        var context = await runner.RunAsync();


        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userCreated = context.Get<ApiResultTest<ApplicationUserResponseTest>>(ScenarioDataKey.UserCreate);
        var userChangeState = await response.Content.ReadModelFromJsonAsync<ApiResultTest<ApplicationUserResponseTest>>();

        userChangeState.Should().NotBeNull();
        userCreated.Should().NotBeNull();
        userChangeState.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.Success);
        userChangeState.Result.Should().NotBeNull();
        userChangeState.Result!.UserId.Should().Be(userCreated.Result!.UserId);
        userChangeState.Result!.IsActive.Should().Be(!userCreated.Result.IsActive);
    }

    [Fact]
    public async Task Should_not_be_able_update_when_not_exist_user()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserChangeStateActive(userId: "123");

        //-ACT
        var context = await runner.RunAsync();


        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
        apiResult.ErrorKey.Should().Be("UserNotFoundWithUserIdException");

    }

    [Fact]
    public async Task Should_not_be_able_update_when_you_dont_have_access()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefault()
            .LoginDefaultUser()
            .UserChangeStateActive(userId: "user-id");

        //-ACT
        var context = await runner.RunAsync();


        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.Forbidden);
        apiResult.ErrorKey.Should().Be("JwtRESTForbiddenException");

    }


}
