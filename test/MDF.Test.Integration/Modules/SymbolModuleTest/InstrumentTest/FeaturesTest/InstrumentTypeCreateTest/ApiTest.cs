using MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.FeaturesTest.InstrumentTypeCreateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "instrument-type-create")]
public partial class InstrumentTypeCreateTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/instrument/instrument-type";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public InstrumentTypeCreateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData("", 0, "Code is required", "Code must be greater than to 0")]
    [InlineData("", -1, "Code must be greater than to 0")]
    public async Task Should_not_be_able_create_when_not_valid_data(
        string title,
        int code,
        params string[] errorMessages)
    {
        //-ARRANGE
        var addDto = new InstrumentTypeAddDtoTest
        {
            Title = title,
            Code = code,
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", addDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(errorMessages.ToList());

    }

    [Theory]
    [InlineData("", 10)]
    [InlineData("New title", 1)]
    public async Task Should_be_able_create_when_valid_data(
        string title,
        int code)
    {
        //-ARRANGE
        var addDto = new InstrumentTypeAddDtoTest
        {
            Title = title,
            Code = code,
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", addDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var instrumentTypeCreated = await response.Content.ReadModelFromJsonAsync<InstrumentTypeGetDtoTest>();
        instrumentTypeCreated.Should().NotBeNull();
        instrumentTypeCreated.Code.Should().Be(code);
        instrumentTypeCreated.Title.Should().Be(title);
        instrumentTypeCreated.Id.Should().BeGreaterThan(0);
    }


    [Fact]
    public async Task Should_not_be_able_create_again_when_exist_already()
    {
        //-ARRANGE
        var addDto = new InstrumentTypeAddDtoTest
        {
            Title = "title",
            Code = 10,
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", addDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        //-ASSERT
        response = await _client.PostAsync($"{_api}", addDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(["there is already exist a InstrumentTypeEntity => InstrumentTypeCode : 10"]);
    }
}