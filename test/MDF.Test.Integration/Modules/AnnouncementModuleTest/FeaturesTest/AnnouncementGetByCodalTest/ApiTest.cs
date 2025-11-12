using MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.AnnouncementTest.AnnouncementGetByCodalTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("ANNOUNCEMENT", "get-by-codal-code")]
public class FundGetByIsinOrSeoRegisterNumber : BaseTest
{
    private readonly string _apiAddress = "api/v4/announcement";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FundGetByIsinOrSeoRegisterNumber(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        _outPutHelper = outPutHelper;
        var scope = factory.Services.CreateScope();
    }



    [Fact]
    public async Task Should_not_be_able_get_announcement_when_not_exist_codalCode()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/123");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(["there is not exist any AnnouncementEntity =\u003E CodalCode : 123"]);
    }


    [Fact]
    public async Task Should_be_able_get_announcement_when_exist_isin()
    {
        //-ARRANGE
        var dtStart = new DateTime(2025, 07, 28, 09, 51, 0);
        var codalCode = 123;
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: codalCode,
             dtPublish: dtStart);

        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/123");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var announcementResponse = await response.Content.ReadModelFromJsonAsync<AnnouncementResponseTest>();
        announcementResponse.Should().NotBeNull();

        announcementResponse.AnnouncementId.Should().Be(announcement.AnnouncementIdPk);
        announcementResponse.CodalCode.Should().Be(codalCode);
        announcementResponse.FirmId.Should().Be(1);
        announcementResponse.SymbolIsin.Should().BeNull();
        announcementResponse.SymbolName.Should().BeNull();
        announcementResponse.OrganizationName.Should().BeNull();
        announcementResponse.IsConfirm.Should().BeFalse();
        announcementResponse.Title.Should().BeNull();
        announcementResponse.TypeOfAnnouncement.Should().Be("Other");
        announcementResponse.PublishDate.Should().Be("2025-07-28T09:51:00");
        announcementResponse.ConfirmedName.Should().BeNull();
        announcementResponse.ConfirmDateTime.Should().BeNull();
        announcementResponse.HtmlLink.Should().BeNull();
    }


    [Fact]
    public async Task Should_be_able_get_announcement_isConfirm_capitalChange_WithCashIncomingData()
    {
        //-ARRANGE
        var dtStart = new DateTime(2025, 07, 30, 09, 51, 0);
        var codalCode = 123;
        var announcement = await _factory.Repositories.AnnouncementAddAsync(
            codalCode: codalCode, dtPublish: dtStart);
        var capitalChange = await _factory.Repositories.CapitalChangeAddAsync(
            announcement.AnnouncementIdPk,
            capitalChangeType: 1,
            isAccept: true,
            approvalType: true,
            dtModify: dtStart,
            dtEntry: dtStart,
            LastShareValue: 1000,
            NewShareValue: 1000,
            NewShareCount: 0,
            LastShareCount: 6500000000,
            isConfirm: true,
            isConfirmManual: true,
            dtConfirmManual: dtStart,
            insertBy: "USER");

        await _factory.Repositories.CapitalChangeAddAsync(
             announcement.AnnouncementIdPk,
             capitalChangeType: 1,
             dtModify: dtStart,
             dtEntry: dtStart,
             LastShareValue: 1000,
             NewShareValue: 1000,
             NewShareCount: 0,
             LastShareCount: 6500000000,
             isAccept: true,
             approvalType: false,
             isConfirm: false,
             isConfirmManual: null,
             dtConfirmManual: null);


        await _factory.Repositories.CapitalChangeItemsAddAsync(
            capitalChangeId: capitalChange.CapitalChangeIdPk,
            capitalChangeMethod: 1,
            value: 2600000);
        await _factory.Repositories.CapitalChangeItemsAddAsync(
            capitalChangeId: capitalChange.CapitalChangeIdPk,
            capitalChangeMethod: 2,
            value: 0);
        await _factory.Repositories.CapitalChangeItemsAddAsync(
            capitalChangeId: capitalChange.CapitalChangeIdPk,
            capitalChangeMethod: 3,
            value: 0);
        await _factory.Repositories.CapitalChangeItemsAddAsync(
            capitalChangeId: capitalChange.CapitalChangeIdPk,
            capitalChangeMethod: 4,
            value: 0);
        await _factory.Repositories.CapitalChangeItemsAddAsync(
            capitalChangeId: capitalChange.CapitalChangeIdPk,
            capitalChangeMethod: 5,
            value: 0);


        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/123");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var announcementResponse = await response.Content.ReadModelFromJsonAsync<AnnouncementResponseTest>();
        announcementResponse.Should().NotBeNull();

        announcementResponse.AnnouncementId.Should().Be(announcement.AnnouncementIdPk);
        announcementResponse.CodalCode.Should().Be(codalCode);
        announcementResponse.FirmId.Should().Be(1);
        announcementResponse.SymbolIsin.Should().BeNull();
        announcementResponse.SymbolName.Should().BeNull();
        announcementResponse.OrganizationName.Should().BeNull();
        announcementResponse.IsConfirm.Should().BeTrue();
        announcementResponse.Title.Should().BeNull();
        announcementResponse.TypeOfAnnouncement.Should().Be("CapitalChange");
        announcementResponse.PublishDate.Should().Be("2025-07-30T09:51:00");
        announcementResponse.ConfirmedName.Should().Be("USER");
        announcementResponse.ConfirmDateTime.Should().Be("2025-07-30T09:51:00");
        announcementResponse.HtmlLink.Should().BeNull();

        announcementResponse.CapitalChange.Should().NotBeNull();
        announcementResponse.CapitalChange.CapitalChangeIdPk.Should().Be(capitalChange.CapitalChangeIdPk);
        announcementResponse.CapitalChange.AnnouncementIdFk.Should().Be(announcement.AnnouncementIdPk);
        announcementResponse.CapitalChange.CashIncoming.Should().Be(2600000);
        announcementResponse.CapitalChange.CashIncomingPercent.Should().NotBeNull();
    }
}
