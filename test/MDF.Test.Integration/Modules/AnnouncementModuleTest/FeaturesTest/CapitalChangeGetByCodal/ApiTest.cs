using MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;
using MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.AnnouncementTest.CapitalChangeGetByCodal;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("ANNOUNCEMENT", "capital-change-get-by-codal-code")]
public class FundGetByIsinOrSeoRegisterNumber : BaseTest
{
    private readonly string _apiAddress = "api/v4/announcement/capital-change";
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
    public async Task Should_be_able_get_capital_change()
    {

        var dt = new DateTime(2025, 10, 27, 14, 7, 51);
        var dtConfirm = new DateTime(2025, 10, 27, 18, 7, 51);
        var firmId = 301011;
        await _factory.Repositories.FirmAddAsync(firmId);
        var announcement = await _factory.Repositories
            .AnnouncementAddAsync(codalCode: 1426908,
            title: "خلاصه تصمیمات مجمع عمومی فوق العاده دوره 12 ماهه منتهی به",
            firmId: firmId,
            dtPublish: dt);

        var capitalChange1 = await _factory.Repositories.CapitalChangeAddAsync(
            announcementId: announcement.AnnouncementIdPk,
            LastShareValue: 1000,
            NewShareValue: 2000,
            LastShareCount: 1000,
            NewShareCount: 2000,
            approvalType: false,
            isAccept: true,
            dtEntry: dt,
            dtModify: dt,
            isConfirm: false,
            isDeleted: false,
            insertionType: 0,
            isConfirmManual: null,
            insertBy: null,
            isUseSalb: false
            );

        var capitalChange2 = await _factory.Repositories.CapitalChangeAddAsync(
            announcementId: announcement.AnnouncementIdPk,
            LastShareValue: 1000,
            NewShareValue: 2000,
            LastShareCount: 1000,
            NewShareCount: 2000,
            approvalType: true,
            isAccept: true,
            dtEntry: dt,
            dtModify: dt,
            isConfirm: true,
            isDeleted: false,
            insertionType: 0,
            isConfirmManual: true,
            insertBy: "شهاب",
            isUseSalb: false,
            dtConfirmManual: dtConfirm
            );


        var item1 = await _factory.Repositories.CapitalChangeItemsAddAsync(
            capitalChangeId: capitalChange2.CapitalChangeIdPk,
            capitalChangeMethod: 1,
            value: 0
            );

        var item2 = await _factory.Repositories.CapitalChangeItemsAddAsync(
           capitalChangeId: capitalChange2.CapitalChangeIdPk,
           capitalChangeMethod: 2,
           value: 2600000
           );

        var item3 = await _factory.Repositories.CapitalChangeItemsAddAsync(
           capitalChangeId: capitalChange2.CapitalChangeIdPk,
           capitalChangeMethod: 3,
           value: 0
           );

        var item4 = await _factory.Repositories.CapitalChangeItemsAddAsync(
           capitalChangeId: capitalChange2.CapitalChangeIdPk,
           capitalChangeMethod: 4,
           value: 0
           );

        var item5 = await _factory.Repositories.CapitalChangeItemsAddAsync(
           capitalChangeId: capitalChange2.CapitalChangeIdPk,
           capitalChangeMethod: 4,
           value: 0
           );


        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/{announcement.Code}");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<CapitalChangeResponseTest>();
        apiResult.Should().NotBeNull();

        apiResult.CapitalChangeIdPk.Should().Be(capitalChange2.CapitalChangeIdPk);
        apiResult.AnnouncementIdFk.Should().Be(announcement.AnnouncementIdPk);
        apiResult.CapitalChangeTypeIdFk.Should().Be(1);
        apiResult.LastShareValue.Should().Be(1000);
        apiResult.NewShareValue.Should().Be(2000);
        apiResult.LastShareCount.Should().Be(1000);
        apiResult.NewShareCount.Should().Be(2000);
        apiResult.ApprovalType.Should().BeTrue();
        apiResult.IsAccept.Should().BeTrue();
        apiResult.EntryDate.Should().Be("2025-10-27");
        apiResult.ModifyDate.Should().Be("2025-10-27");
        apiResult.IsConfirmed.Should().BeTrue();
        apiResult.IsDeleted.Should().BeFalse();
        apiResult.InsertionType.Should().Be(0);
        apiResult.InsertedBy.Should().Be("شهاب");
        apiResult.IsConfirmedManual.Should().BeTrue();
        apiResult.ConfirmedManualTime.Should().BeNull();
        apiResult.IsUseSalb.Should().BeFalse();

        apiResult.CashIncoming.Should().Be(0);
        apiResult.RetaindedEarning.Should().Be(2600000);
        apiResult.Reserves.Should().Be(0);
        apiResult.SarfSaham.Should().Be(0);
        apiResult.RevaluationSurplus.Should().Be(0);
        apiResult.CashIncomingPercent.Should().Be(0);
        apiResult.CapitalChangePercent.Should().Be(260000000);
        apiResult.RetainedEarningPercent.Should().Be(260000000);
        apiResult.ReservesPercent.Should().Be(0);
        apiResult.SarfSahamPercent.Should().Be(0);
        apiResult.RevaluationSurplusPercent.Should().Be(0);
    }

}
