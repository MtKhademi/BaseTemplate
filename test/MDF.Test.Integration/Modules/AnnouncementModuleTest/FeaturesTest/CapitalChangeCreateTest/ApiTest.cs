using MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;
using MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.AnnouncementTest.CapitalChangeCreateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("ANNOUNCEMENT", "capital-change-create")]
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
    public async Task Should_not_be_able_create_an_capitalChange_when_not_exist_announcement()
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
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(["there is not exist any AnnouncementEntity =\u003E CodalCode : 123"]);
    }


    [Fact]
    public async Task Should_be_able_create()
    {
        var codalCode = 123;
        await _factory.Repositories.AnnouncementAddAsync(codalCode);
        var dto = new CapitalChangeCreateRequestTest
        {
            CodalCode = codalCode,
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
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
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
        apiResult.LastShareCount.Should().Be(1000);
        apiResult.NewShareCount.Should().Be(2000);
        apiResult.ApprovalType.Should().BeTrue();
        apiResult.IsAccept.Should().BeFalse();
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
        apiResult.Reserves.Should().Be(300000);
        apiResult.SarfSaham.Should().Be(200000);
        apiResult.RevaluationSurplus.Should().Be(100000);
    }
}
