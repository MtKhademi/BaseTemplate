using Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

namespace Test.Integration.IAMModuleTest.FeaturesTest.UserFeatureTests;

[Collection("Collection tests v1")]
[Trait("IAM", "user-update[REST]")]
public partial class UserUpdateRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserUpdateRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_update_a_user()
    {
        //-ARRANGE
        var createRequest = new UserCreateRequestTest(
          Email: "test@example.com",
          UserName: "testuser",
          Password: "P@ssw0rd",
          ConfirmPassword: "P@ssw0rd",
          PhoneNumber: "1234567890",
          FirstName: "Test",
          LastName: "User");

        var updateRequest = new UserUpdateRequestTest(
         Email: "test-updated@example.com",
         UserName: "testuser-updated",
         PhoneNumber: "123U567890",
         FirstName: "Test-updated",
         LastName: "User - updated");
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserCreate(createRequest)
            .UserUpdate(updateRequest, userIndex: Numericals.UseFirst);


        //-ACT
        var context = await runner.RunAsync();


        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = context.Get<ApiResultTest<ApplicationUserResponseTest>>(ScenarioDataKey.UserUpdate);
        apiResult.Should().NotBeNull();
        var userResult = apiResult!.Result;
        userResult.Should().NotBeNull();
        userResult.Email.Should().Be(updateRequest.Email);
        userResult.FirstName.Should().Be(updateRequest.FirstName);
        userResult.LastName.Should().Be(updateRequest.LastName);
        userResult.PhoneNumber.Should().Be(updateRequest.PhoneNumber);
        userResult.UserId.Should().NotBeEmpty();
        userResult.IsActive.Should().BeTrue();
        userResult.EmailConfirmed.Should().BeFalse();
    }

    [Fact]
    public async Task Should_not_be_able_update_a_user_when_exist_already_userEmail()
    {
        //-ARRANGE
        var createRequest = new UserCreateRequestTest(
          Email: "test@example.com",
          UserName: "testuser",
          Password: "P@ssw0rd",
          ConfirmPassword: "P@ssw0rd",
          PhoneNumber: "1234567890",
          FirstName: "Test",
          LastName: "User");

        var updateRequest = new UserUpdateRequestTest(
         Email: "test2@example.com",
         UserName: "testuser-updated");
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserCreate(createRequest)
            .UserCreate(createRequest with
            {
                Email = "test2@example.com",
                UserName = "testuser2",
                PhoneNumber = "1234567880"
            })
            .UserUpdate(updateRequest, userIndex: Numericals.UseFirst);


        //-ACT
        var context = await runner.RunAsync();


        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("UserUpdateHandlerException");

    }

    [Fact]
    public async Task Should_not_be_able_update_a_user_when_exist_already_userName()
    {
        //-ARRANGE
        var createRequest = new UserCreateRequestTest(
          Email: "test@example.com",
          UserName: "testuser",
          Password: "P@ssw0rd",
          ConfirmPassword: "P@ssw0rd",
          PhoneNumber: "1234567890",
          FirstName: "Test",
          LastName: "User");

        var updateRequest = new UserUpdateRequestTest(
         Email: "test2@example.com",
         UserName: "testuser-updated");
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserCreate(createRequest)
            .UserCreate(createRequest with
            {
                Email = "test2@example.com",
                UserName = "testuser2",
                PhoneNumber = "123123123"
            })
            .UserUpdate(updateRequest, userIndex: Numericals.UseFirst);


        //-ACT
        var context = await runner.RunAsync();


        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("UserUpdateHandlerException");

    }


    [Fact]
    public async Task Should_not_be_able_update_a_user_when_exist_already_phone()
    {
        //-ARRANGE
        var createRequest = new UserCreateRequestTest(
          Email: "test@example.com",
          UserName: "testuser",
          Password: "P@ssw0rd",
          ConfirmPassword: "P@ssw0rd",
          PhoneNumber: "1234567890",
          FirstName: "Test",
          LastName: "User");

        var updateRequest = new UserUpdateRequestTest(
          Email: "test3@example.com",
          UserName: "testuser3",
          PhoneNumber: "1234567891");
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .UserCreate(createRequest)
            .UserCreate(createRequest with
            {
                Email = "test2@example.com",
                UserName = "testuser2",
                PhoneNumber = "1234567891"
            })
            .UserUpdate(updateRequest, userIndex: Numericals.UseFirst);


        //-ACT
        var context = await runner.RunAsync();


        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("UserUpdateHandlerException");

    }

}
