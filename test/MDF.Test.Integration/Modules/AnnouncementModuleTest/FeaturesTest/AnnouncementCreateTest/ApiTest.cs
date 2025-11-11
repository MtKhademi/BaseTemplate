using MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;
using MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.FeaturesTest.AnnouncementCreateTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("ANNOUNCEMENT", "create")]

public class AnnouncementCreateTest : BaseTest
{
    private readonly string _apiAddress = "api/v4/announcement";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AnnouncementCreateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        _outPutHelper = outPutHelper;
        var scope = factory.Services.CreateScope();
    }

    [Theory]
    [InlineData(44, null, null, null)]
    public async Task When_not_valid_data_Expect_get_bad_response(int? codalCode, string? isin, string? title, string? publisDateTime)
    {
        //-ARRANGE
        AnnouncementCreateRequestTest dto = new AnnouncementCreateRequestTest()
        {
            CodalCode = codalCode,
            PublishDateTime = publisDateTime,
            SymbolIsin = isin,
            Title = title
        };
        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("AnnouncementCreateCommandException");
    }



    [Fact]
    public async Task When_not_exist_isin_in_db_Expect_get_Not_Found_response()
    {
        //-ARRANGE
        AnnouncementCreateRequestTest dto = new AnnouncementCreateRequestTest()
        {
            CodalCode = 123,
            PublishDateTime = "2025-10-14",
            SymbolIsin = "IRBA858581",
            Title = "title"
        };

        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("SymbolNotExistIsinException");
    }

    [Fact]
    public async Task When_symbol_dosent_have_firm_in_db_Expect_get_bad_request_response()
    {
        //-ARRANGE
        var isin = "IRBA858581";
        await _factory.Repositories.SymbolAddAsync(isin: isin, firmId: null);
        AnnouncementCreateRequestTest dto = new AnnouncementCreateRequestTest()
        {
            CodalCode = 123,
            PublishDateTime = "2025-10-14",
            SymbolIsin = isin,
            Title = "title"
        };

        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("AnnouncementCreateHandler");
    }


    [Fact]
    public async Task When_not_exist_announcement_Expect_get_Ok_response()
    {
        //-ARRANGE
        var isin = "IRBA858581";
        await _factory.Repositories.SymbolAddAsync(isin: isin, firmId: 1);
        AnnouncementCreateRequestTest dto = new AnnouncementCreateRequestTest()
        {
            CodalCode = 123,
            PublishDateTime = "2025-10-14",
            SymbolIsin = isin,
            Title = "title"
        };

        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<AnnouncementResponseTest>();
        apiResult.Should().NotBeNull();
    }


    [Fact]
    public async Task When_already_exist_announcement_Expect_get_bad_request_in_response()
    {
        //-ARRANGE
        var isin = "IRBA858581";
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 123);
        await _factory.Repositories.SymbolAddAsync(isin: isin, firmId: 1);
        AnnouncementCreateRequestTest dto = new AnnouncementCreateRequestTest()
        {
            CodalCode = 123,
            PublishDateTime = "2025-10-14",
            SymbolIsin = isin,
            Title = "title"
        };

        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("AnnouncementAlreadyExistWithCodalCodeException");
    }
}
