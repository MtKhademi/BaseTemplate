using Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.AuthScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.RoleFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "role-update[REST]")]
public partial class RoleUpdateRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public RoleUpdateRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData("", "", "")]
    [InlineData(null, null, "desc")]
    [InlineData(null, "", "desc")]
    public async Task Should_not_be_able_update_a_role_when_not_send_correct_data(
        string roleId, string roleName, string roleDescription)
    {

        //-ARRANGE
        var request = new RoleUpdateRequestTest
        {
            RoleId = roleId,
            Name = roleName,
            Description = roleDescription
        };
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleUpdate(request);

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
        apiResult.ErrorKey.Should().Be("RoleUpdateCommandException");
    }

    [Fact]
    public async Task Should_be_able_update_a_role()
    {
        //-ARRANGE
        var request = new RoleUpdateRequestTest
        {
            RoleId = null,
            Name = "role-new-name",
            Description = "role-new-desc"
        };
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleCreate(roleName: "roleName1", roleDescription: "desc")
            .RoleUpdate(request);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<RoleResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.RoleName.Should().Be(request.Name);
        apiResult.Result.RoleDescription.Should().Be(request.Description);
        apiResult.Result.RoleId.Should().NotBeEmpty();
        apiResult.Result.RoleId.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_not_be_able_update_a_role_when_exist_this_roleName_in_another_role()
    {
        //-ARRANGE
        var request = new RoleUpdateRequestTest
        {
            RoleId = null,
            Name = "roleName1",
            Description = "role-new-desc"
        };
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .RoleCreate(roleName: "roleName1", roleDescription: "desc")
            .RoleCreate(roleName: "roleName2", roleDescription: "desc")
            .RoleUpdate(request, conditionChoose: role => role.RoleName == "roleName2");

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }
}
