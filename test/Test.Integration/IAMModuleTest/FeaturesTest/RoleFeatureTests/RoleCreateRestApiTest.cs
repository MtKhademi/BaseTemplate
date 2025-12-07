namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "role-create[REST]")]
public partial class RoleCreateRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public RoleCreateRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
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
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleCreate(roleName, roleDescription);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
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
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleCreate(roleName, roleDescription);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<RoleResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.RoleName.Should().Be(roleName);
        apiResult.Result.RoleDescription.Should().Be(roleDescription);
        apiResult.Result.RoleId.Should().NotBeEmpty();
    }


    [Fact]
    public async Task Should_not_be_able_create_a_role_when_already_exist_roleName()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleCreate("roleName1", "desc1")
            .RoleCreate("roleName1", "desc");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
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
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RoleCreate("roleName", "desc");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }


    [Fact]
    public async Task Should_not_be_able_create_a_new_role_when_you_dont_have_access()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefault()
            .LoginDefaultUser()
            .RoleCreate("roleName", "desc");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }
}
