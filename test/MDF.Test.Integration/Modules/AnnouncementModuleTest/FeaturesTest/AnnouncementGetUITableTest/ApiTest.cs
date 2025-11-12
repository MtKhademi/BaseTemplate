using MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;
using MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.AnnouncementTest.AnnouncementGetUITableTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("ANNOUNCEMENT", "ui-table")]
public partial class AnnouncementGetUITableTest : BaseTest
{
    private readonly string _api = $"/api/v4/announcement/ui-table";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AnnouncementGetUITableTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_data_for_table_ui_empty()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var tableDto = await response.Content.ReadModelFromJsonAsync<BaseTableDto<AnnouncementUITableRowResponseTest>>();
        tableDto.AssertionUITableEmpty(14);

        //-- check columns 
        tableDto.Metadata.Details
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.AnnouncementId), ColumnDataType.INT)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.CodalCode), ColumnDataType.INT)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.FirmId), ColumnDataType.INT)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.SymbolIsin), ColumnDataType.STRING)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.SymbolName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.OrganizationName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.IsConfirm), ColumnDataType.BOOLEAN)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.Title), ColumnDataType.STRING)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.TypeOfAnnouncement), ColumnDataType.STRING)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.PublishDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.ConfirmedName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.ConfirmDateTime), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.HtmlLink), ColumnDataType.STRING)
            .ColumnAssertion(nameof(AnnouncementUITableRowResponseTest.Actions), ColumnDataType.LIST);
    }

    [Theory]
    [ClassData(typeof(AnnouncementGetPaginatedListRequestTestNotValidData))]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(AnnouncementGetPaginatedListRequestTest dto, List<string> errors)
    {
        //-ARRANGE
        var api = $"{_api}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(errors);
    }

    [Fact]
    public async Task Should_be_able_data_without_any_filter()
    {
        //-ARRANGE
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 8586);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 8587);

        var dtoFilter = new AnnouncementGetPaginatedListRequestTest();
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<AnnouncementUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().HaveCount(3);
        //apiResult.Data.Content.Should().HaveCount(count);
        foreach (var item in apiResult.Data.Content)
        {
        }
    }

    [Fact]
    public async Task Should_be_able_get_data_for_special_row_and_check_data_have_to_correct()
    {
        //-ARRANGE
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585);
        var link = await _factory.Repositories.AnnouncementLinkAddAsync(announcement.AnnouncementIdPk, linkType: 1);

        await _factory.Repositories.AnnouncementAddAsync(codalCode: 8586);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 8587);

        var dtoFilter = new AnnouncementGetPaginatedListRequestTest { CodalCode = 8585 };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<AnnouncementUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().NotBeEmpty();
        var announcementDto = apiResult.Data.Content.FirstOrDefault();
        announcementDto.Should().NotBeNull();
        announcementDto.CodalCode.Should().Be(8585);
        announcementDto.HtmlLink.Should().Be(link.LinkUrl);
    }


    [Fact]
    public async Task Should_be_able_can_get_data_when_exist_multi_row_for_AnnouncementLinks()
    {
        //-ARRANGE
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585);
        var link = await _factory.Repositories.AnnouncementLinkAddAsync(announcement.AnnouncementIdPk, linkType: 1);
        await _factory.Repositories.AnnouncementLinkAddAsync(announcement.AnnouncementIdPk, linkType: 2);
        await _factory.Repositories.AnnouncementLinkAddAsync(announcement.AnnouncementIdPk, linkType: 3);

        await _factory.Repositories.AnnouncementAddAsync(codalCode: 8586);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 8587);

        var dtoFilter = new AnnouncementGetPaginatedListRequestTest { CodalCode = 8585 };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<AnnouncementUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().NotBeEmpty();
        var announcementDto = apiResult.Data.Content.FirstOrDefault();
        announcementDto.Should().NotBeNull();
        announcementDto.CodalCode.Should().Be(8585);
        announcementDto.HtmlLink.Should().Be(link.LinkUrl);
    }

    [Fact]
    public async Task Should_be_able_can_get_data_with_isin_filter()
    {
        //-ARRANGE
        await _factory.Repositories.FirmAddAsync(firmId: 10);
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: "IRN123456781", firmId: 10,
            typeOfSymbol: TypeOfSymbolTest.Stock);
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585, firmId: 10);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 8586);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 8587);

        var dtoFilter = new AnnouncementGetPaginatedListRequestTest { SymbolIsin = "IRN123456781" };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<AnnouncementUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().HaveCount(1);
        var announcementDto = apiResult.Data.Content.FirstOrDefault();
        announcementDto.Should().NotBeNull();
        announcementDto.CodalCode.Should().Be(8585);
        announcementDto.SymbolIsin.Should().Be(symbol.Isin);
        announcementDto.SymbolName.Should().Be(symbol.SymbolName);

    }
}
