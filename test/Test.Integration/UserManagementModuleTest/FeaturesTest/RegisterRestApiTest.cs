using Test.Integration.Fixtures.UserManagementModuleFixtures;

namespace Test.Integration.UserManagementModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("UserManagement", "Register[REST]")]
public partial class RegisterRestApiTest : BaseTest
{
    private readonly string _api = $"/api/iam/v1/register";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public RegisterRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        //_client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData("test@example.com", null, null, null)]
    [InlineData("test@example.com", "testuser", null, null)]
    [InlineData(null, "testuser", null, null)]
    [InlineData("test@example.com", "testuser", "P@ssw0rd", null)]
    [InlineData("test@example.com", "testuser", "P@ssw0rd", "123")]
    public async Task Should_not_be_able_register_when_not_send_correct_data(
        string? email = default!,
        string? userName = default!,
        string? password = default!,
        string? confirmPassword = default!)
    {
        //-ARRANGE
        var dto = new UserRegistrationRequestTest(
            Email: email,
            UserName: userName,
            Password: password,
            ConfirmPassword: confirmPassword,
            PhoneNumber: "0939917",
            FirstName: "Test",
            LastName: "User"
        );

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("UserRegistrationRequestException");
    }

    [Fact]
    public async Task Should_be_able_register_new_user()
    {
        //-ARRANGE
        var dto = new UserRegistrationRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<ApplicationUserResponseTest>>();
        apiResult.Should().NotBeNull();
        var user = apiResult!.Result;
        user.Should().NotBeNull();
        user.Email.Should().Be(dto.Email);
        user.FirstName.Should().Be(dto.FirstName);
        user.LastName.Should().Be(dto.LastName);
        user.PhoneNumber.Should().Be(dto.PhoneNumber);
        user.UserId.Should().NotBeEmpty();
        user.IsActive.Should().BeTrue();
        user.EmailConfirmed.Should().BeFalse();
    }

    [Fact]
    public async Task Should_not_be_able_register_when_exist_already_userEmail()
    {
        //-ARRANGE
        var dto = new UserRegistrationRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        await _client.IAMRegister(email: dto.Email);

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
        apiResult.ErrorKey.Should().Be("UserAlreadyExistWithEmailException");

    }

    [Fact]
    public async Task Should_not_be_able_register_when_exist_already_userName()
    {
        //-ARRANGE
        var dto = new UserRegistrationRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        await _client.IAMRegister(email: "test2@example.com",
            phoneNumber: "1234567898", userName: dto.UserName);

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
        apiResult.ErrorKey.Should().Be("UserAlreadyExistWithUserNameException");

    }

    [Fact]
    public async Task Should_not_be_able_register_when_exist_already_phoneNumber()
    {
        //-ARRANGE
        var dto = new UserRegistrationRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        await _client.IAMRegister(email: "test2@example.com", userName: "testuser2", phoneNumber: "1234567890");

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
        apiResult.ErrorKey.Should().Be("UserAlreadyExistWithPhoneException");

    }

}
