using Test.Integration.Fixtures.IAMModuleFixtures;

namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection("Collection tests v1")]
[Trait("IAM", "user-create[REST]")]
public partial class UserCreateRestApiTest : BaseTest
{
    private readonly string _api = $"/api/iam/v1/user";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public UserCreateRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        //_client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData(null, null, null)]
    [InlineData("testuser", null, null)]
    [InlineData("testuser", null, "null")]
    public async Task Should_not_be_able_register_when_not_send_correct_data(
        string? userName = default!,
        string? password = default!,
        string? confirmPassword = default!)
    {
        //-ARRANGE
        var dto = new UserCreateRequestTest(
            UserName: userName,
            Password: password,
            ConfirmPassword: confirmPassword
        );
        await _client.IAMLoginAdmin();

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("UserCreateCommandException");
    }

    [Fact]
    public async Task Should_be_able_create_an_new_user()
    {
        //-ARRANGE
        var dto = new UserCreateRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        await _client.IAMLoginAdmin();

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
    public async Task Should_not_be_able_create_an_user_when_exist_already_userEmail()
    {
        //-ARRANGE
        var dto = new UserCreateRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        await _client.IAMLoginAdmin();


        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        dto = new UserCreateRequestTest(
            Email: "test@example.com",
            UserName: "testuser2",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567892",
            FirstName: "Test",
            LastName: "User"
        );
        response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
        apiResult.ErrorKey.Should().Be("UserAlreadyExistWithEmailException");

    }

    [Fact]
    public async Task Should_not_be_able_create_an_user_when_exist_already_userName()
    {
        //-ARRANGE
        var dto = new UserCreateRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        await _client.IAMLoginAdmin();


        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        dto = new UserCreateRequestTest(
            Email: "test2@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567892",
            FirstName: "Test",
            LastName: "User"
        );
        response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
        apiResult.ErrorKey.Should().Be("UserAlreadyExistWithUserNameException");

    }

    // در این حالت بار اول باید ثتب بشه ولی بار دومی چون همون شماهر تلفن و میفرستیم باید خطا بده
    [Fact]
    public async Task Should_not_be_able_create_an_user_when_exist_already_phoneNumber()
    {
        //-ARRANGE
        var dto = new UserCreateRequestTest(
            Email: "test@example.com",
            UserName: "testuser",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        await _client.IAMLoginAdmin();


        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        dto = new UserCreateRequestTest(
            Email: "test2@example.com",
            UserName: "testuser2",
            Password: "P@ssw0rd",
            ConfirmPassword: "P@ssw0rd",
            PhoneNumber: "1234567890",
            FirstName: "Test",
            LastName: "User"
        );
        response = await _client.PostAsync(_api, dto.ToContentHttp());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
        apiResult.ErrorKey.Should().Be("UserAlreadyExistWithPhoneException");

    }

}
