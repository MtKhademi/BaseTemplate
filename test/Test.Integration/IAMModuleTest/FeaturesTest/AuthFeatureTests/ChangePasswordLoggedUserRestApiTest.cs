using Test.Integration.Fixtures;
using Test.Integration.Fixtures.IAMTestScenarios.AccessControll;
using Test.Integration.Fixtures.IAMTestScenarios.UserScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.AuthFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "change-password-logged-user[REST]")]
public partial class ChangePasswordLoggedUserRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public ChangePasswordLoggedUserRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData(null, null, null)]
    public async Task Should_not_be_able_change_password_when_not_send_correct_data(
        string? userName = default!,
        string? password = default!,
        string? confirmPassword = default!)
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefaultUser()
            .LoginDefaultUser()
            .ChangePasswordLoggedUser(new ChangePasswordRequestTest(
                CurrentPassword: password,
                NewPassword: password,
                ConfirmNewPassword: confirmPassword));

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("ChangePasswordCommandException");
    }

    [Fact]
    public async Task Should_be_able_change_password()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .ChangePasswordLoggedUser(new ChangePasswordRequestTest(
                CurrentPassword: "8585@8585",
                NewPassword: "newpasswo@@rd",
                ConfirmNewPassword: "newpasswo@@rd"
            ));

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = context.Get<ApiResultTest<bool>>(ScenarioDataKey.ChangePasswordLoggedUserResponse);
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().BeTrue();
    }


    [Fact]
    public async Task Should_be_able_change_password_current_user()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .RegisterDefaultUser()
            .LoginDefaultUser()
            .ChangePasswordLoggedUser();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = context.Get<ApiResultTest<bool>>(ScenarioDataKey.ChangePasswordLoggedUserResponse);
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().BeTrue();

    }

    [Fact]
    public async Task Should_not_be_able_get_data_after_change_passwrd_must_expier_token()
    {

        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .ChangePasswordLoggedUser(new ChangePasswordRequestTest(
                CurrentPassword: "8585@8585",
                NewPassword: "newpasswo@@rd",
                ConfirmNewPassword: "newpasswo@@rd"
            ))
            .UserCreateDefault();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var apiResult = await response.Content.ReadFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }
}
