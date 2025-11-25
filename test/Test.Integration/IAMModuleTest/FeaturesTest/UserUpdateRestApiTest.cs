using Test.Integration.Fixtures.IAMModuleFixtures;

namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "user-update[REST]")]
public partial class UserUpdateRestApiTest : BaseTest
{
    private readonly string _api = $"/api/iam/v1/user";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserUpdateRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        //_client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_update_a_user()
    {
        //-ARRANGE
        await _client.IAMLoginAdmin();
        var createRequest = new UserCreateRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        var user = await _client.IAMCreateAUser(createRequest);

        //-ACT
        var updateRequest = new UserCreateRequestTest(
           Email: "test-updated@example.com",
           UserName: "testuser-updated",
           PhoneNumber: "123U567890",
           FirstName: "Test-updated",
           LastName: "User - updated"
       );
        var response = await _client.PutAsync($"{_api}/{user.UserId}", updateRequest.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<ApplicationUserResponseTest>>();
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
        await _client.IAMLoginAdmin();
        var createRequest = new UserCreateRequestTest(
           Email: "test@example.com",
           UserName: "testuser",
           Password: "P@ssw0rd",
           ConfirmPassword: "P@ssw0rd",
           PhoneNumber: "1234567890",
           FirstName: "Test",
           LastName: "User"
       );
        var user = await _client.IAMCreateAUser(createRequest);
        var user2 = await _client.IAMCreateAUser(createRequest with
        {
            Email = "test2@example.com",
            UserName = "testuser2",
            PhoneNumber = "1234567880"
        });


        //-ACT
        var updateRequest = new UserCreateRequestTest(
          Email: "test2@example.com",
          UserName: "testuser-updated");
        var response = await _client.PutAsync($"{_api}/{user.UserId}", updateRequest.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
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
        await _client.IAMLoginAdmin();
        var createRequest = new UserCreateRequestTest(
           Email: "test@example.com",
           UserName: "testuser",
           Password: "P@ssw0rd",
           ConfirmPassword: "P@ssw0rd",
           PhoneNumber: "1234567890",
           FirstName: "Test",
           LastName: "User"
       );
        var user = await _client.IAMCreateAUser(createRequest);
        var user2 = await _client.IAMCreateAUser(createRequest with
        {
            Email = "test2@example.com",
            UserName = "testuser2",
            PhoneNumber = "123123123"
        });


        //-ACT
        var updateRequest = new UserCreateRequestTest(
          Email: "test3@example.com",
          UserName: "testuser2");
        var response = await _client.PutAsync($"{_api}/{user.UserId}", updateRequest.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
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
        await _client.IAMLoginAdmin();
        var createRequest = new UserCreateRequestTest(
           Email: "test@example.com",
           UserName: "testuser",
           Password: "P@ssw0rd",
           ConfirmPassword: "P@ssw0rd",
           PhoneNumber: "1234567890",
           FirstName: "Test",
           LastName: "User"
       );
        var user = await _client.IAMCreateAUser(createRequest);
        var user2 = await _client.IAMCreateAUser(createRequest with
        {
            Email = "test2@example.com",
            UserName = "testuser2",
            PhoneNumber = "1234567891"
        });


        //-ACT
        var updateRequest = new UserCreateRequestTest(
          Email: "test3@example.com",
          UserName: "testuser3",
          PhoneNumber: "1234567891");
        var response = await _client.PutAsync($"{_api}/{user.UserId}", updateRequest.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("UserUpdateHandlerException");

    }

}
