using MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;
using MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.AnnouncementTest.CapitalChangeUpdateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("ANNOUNCEMENT", "capital-change-update")]
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
    public async Task Should_not_be_able_update_an_capitalChange_when_not_exist_announcement()
    {
        //-ARRANGE
        var dto = new CapitalChangeCreateRequestTest
        {
            CodalCode = 123,
            LastShareValue = 1000,
            NewShareValue = 2000,
            LastShareCount = 1000,
            NewShareCount = 2000,
            CashIncoming = 1000000,
            RetaindedEarning = 500000,
            Reserves = 300000,
            SarfSaham = 200000,
            RevaluationSurplus = 100000
        };

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.ErrorKey.Should().Be("AnnouncementNotExistWithCodalCodeException");
    }


    [Fact]
    public async Task Should_not_be_able_update_an_capitalChange_when_not_exist_already_that()
    {
        var codalCode = 123;
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode);
        var dto = new CapitalChangeCreateRequestTest
        {
            CodalCode = codalCode,
            LastShareValue = 1000,
            NewShareValue = 2000,
            LastShareCount = 1000,
            NewShareCount = 2000,
            CashIncoming = 1000000,
            RetaindedEarning = 500000,
            Reserves = 100,
            SarfSaham = 0,
            RevaluationSurplus = 100000
        };

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<CapitalChangeResponseTest>();

    }

    [Fact]
    public async Task Should_be_able_update_an_capitalChange_and_not_change_others()
    {
        //-- Arrange
        var dt = new DateTime(2025, 07, 29, 11, 25, 0);
        var codalCode = 123;
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode, dtPublish: dt);
        var capitalChange = await _factory.Repositories.CapitalChangeAddAsync(announcement.AnnouncementIdPk,
            approvalType: true, dtEntry: dt, dtModify: dt);
        var capitalChangeId = capitalChange.CapitalChangeIdPk; // for test down
        var announcementId = announcement.AnnouncementIdPk;  // for test down
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 1);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 2);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 3);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 4);

        codalCode = 8585;
        announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode);
        capitalChange = await _factory.Repositories.CapitalChangeAddAsync(announcement.AnnouncementIdPk,
            approvalType: true, dtEntry: dt, dtModify: dt);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 1, 4);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 2, 4);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 3, 4);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 4, 4);
        var dto = new CapitalChangeCreateRequestTest
        {
            CodalCode = codalCode,
            LastShareValue = 1000,
            NewShareValue = 2000,
            LastShareCount = 1,
            NewShareCount = 2,
            CashIncoming = 1000000,
            RetaindedEarning = 500000,
            Reserves = 100,
            SarfSaham = 0,
            RevaluationSurplus = 100000
        };

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<CapitalChangeResponseTest>();
        apiResult.Should().NotBeNull();

        apiResult.CapitalChangeIdPk.Should().BeGreaterThan(0);
        apiResult.AnnouncementIdFk.Should().BeGreaterThan(0);
        apiResult.CapitalChangeTypeIdFk.Should().Be(1);
        apiResult.LastShareValue.Should().Be(1000);
        apiResult.NewShareValue.Should().Be(2000);
        apiResult.LastShareCount.Should().Be(1);
        apiResult.NewShareCount.Should().Be(2);
        apiResult.ApprovalType.Should().BeTrue();
        apiResult.IsAccept.Should().BeTrue();
        apiResult.EntryDate.Should().NotBeNullOrEmpty();
        apiResult.ModifyDate.Should().NotBeNullOrEmpty();
        apiResult.IsConfirmed.Should().BeFalse();
        apiResult.IsDeleted.Should().BeFalse();
        apiResult.InsertionType.Should().Be(0);
        apiResult.InsertedBy.Should().Be("TEST-USER");
        apiResult.IsConfirmedManual.Should().BeNull();
        apiResult.ConfirmedManualTime.Should().BeNull();
        apiResult.CashIncoming.Should().Be(1000000);
        apiResult.RetaindedEarning.Should().Be(500000);
        apiResult.Reserves.Should().Be(100);
        apiResult.SarfSaham.Should().Be(0);
        apiResult.RevaluationSurplus.Should().Be(100000);


        //-ASSERT
        string capitalChangeApi = "api/v4/announcement/capital-change/123";
        response = await _client.GetAsync(capitalChangeApi);
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        apiResult = await response.Content.ReadModelFromJsonAsync<CapitalChangeResponseTest>();
        apiResult.Should().NotBeNull();
        apiResult.CapitalChangeIdPk.Should().Be(capitalChangeId);
        apiResult.AnnouncementIdFk.Should().Be(announcementId);
        apiResult.CapitalChangeTypeIdFk.Should().Be(1);
        apiResult.LastShareValue.Should().Be(100);
        apiResult.NewShareValue.Should().Be(100);
        apiResult.LastShareCount.Should().Be(10);
        apiResult.NewShareCount.Should().Be(10);
        apiResult.ApprovalType.Should().BeTrue();
        apiResult.IsAccept.Should().BeTrue();
        apiResult.EntryDate.Should().Be("2025-07-29");
        apiResult.ModifyDate.Should().Be("2025-07-29");
        apiResult.IsConfirmed.Should().BeFalse();
        apiResult.IsDeleted.Should().BeFalse();
        apiResult.InsertionType.Should().Be(0);
        apiResult.InsertedBy.Should().BeNull();
        apiResult.IsConfirmedManual.Should().BeNull();
        apiResult.ConfirmedManualTime.Should().BeNull();
        apiResult.CashIncoming.Should().Be(10);
        apiResult.RetaindedEarning.Should().Be(10);
        apiResult.Reserves.Should().Be(10);
        apiResult.SarfSaham.Should().Be(10);
        apiResult.RevaluationSurplus.Should().Be(0);

    }
}
