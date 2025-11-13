namespace Test.Integration.IAMModuleTest.FeaturesTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("IAMModule", "Register[REST]")]
public partial class RegisterRestApiTest : BaseTest
{
    private readonly string _api = $"/api/basetemplate/iam/v1/register";
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
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var fundResponse = await response.Content.ReadModelFromJsonAsync<ApiResultTest<TokenResponseTest>>();
        fundResponse.Should().NotBeNull();
        fundResponse.Should().NotBeNull();
    }
}
