using MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;
using MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.FeaturesTest.AnnouncementUpdateTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("ANNOUNCEMENT", "update")]

public class AnnouncementUpdateTest : BaseTest
{
    private readonly string _apiAddress = "api/v4/announcement";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AnnouncementUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
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
        var dto = new AnnouncementUpdateRequestTest()
        {
            CodalCode = codalCode,
            PublishDateTime = publisDateTime,
            SymbolIsin = isin,
            Title = title
        };
        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("AnnouncementUpdateCommandException");
    }



    [Fact]
    public async Task When_not_exist_isin_in_db_Expect_get_Not_Found_response()
    {
        //-ARRANGE
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 123);
        var dto = new AnnouncementUpdateRequestTest()
        {
            CodalCode = 123,
            PublishDateTime = "2025-10-14",
            SymbolIsin = "IRBA858581",
            Title = "title"
        };

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
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
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 123);
        await _factory.Repositories.SymbolAddAsync(isin: isin, firmId: null);
        var dto = new AnnouncementUpdateRequestTest()
        {
            CodalCode = 123,
            PublishDateTime = "2025-10-14",
            SymbolIsin = isin,
            Title = "title"
        };

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("AnnouncementUpdateHandler");
    }


    [Fact]
    public async Task When_not_exist_announcement_Expect_get_notFound_response()
    {
        //-ARRANGE
        var isin = "IRBA858581";
        await _factory.Repositories.SymbolAddAsync(isin: isin, firmId: 1);
        var dto = new AnnouncementUpdateRequestTest()
        {
            CodalCode = 123,
            PublishDateTime = "2025-10-14",
            SymbolIsin = isin,
            Title = "title"
        };

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<AnnouncementResponseTest>();
        apiResult.Should().NotBeNull();
    }


    [Fact]
    public async Task When_already_exist_announcement_Expect_can_update()
    {
        //-ARRANGE
        var isin = "IRBA858581";
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 123);
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: isin, firmId: 1);
        var dto = new AnnouncementUpdateRequestTest()
        {
            CodalCode = 123,
            PublishDateTime = "2025-10-14T10:47:00",
            SymbolIsin = isin,
            Title = "title"
        };

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<AnnouncementResponseTest>();
        apiResult.Should().NotBeNull();
        apiResult.AnnouncementId.Should().Be(announcement.AnnouncementIdPk);
        apiResult.CodalCode.Should().Be(announcement.Code);
        apiResult.FirmId.Should().Be(symbol.FirmId);
        apiResult.SymbolIsin.Should().Be(isin);
        apiResult.Title.Should().Be(dto.Title);
        apiResult.TypeOfAnnouncement.Should().Be("Other");
        apiResult.PublishDate.Should().Be("2025-10-14T10:47:00");
    }
}
