using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Requests;
using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;
using Microsoft.VisualBasic;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.FundsTest.FeaturesTest.FundNavGetUITableTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND-NAV", "ui-table")]
public partial class FundNavGetUITableTest : BaseTest
{
    private readonly string _api = $"/api/v4/fund/nav/ui-table";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FundNavGetUITableTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_not_be_able_get_data_without_any_filter()
    {
        //-ARRANGE
        //var dt = new DateTime(2023, 07, 23, 04, 18, 0);
        //await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123");
        //await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber: "123", dtFinancial: dt);

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.Messages.Should().Contain("SeoRegisterNumber is required.");


    }

    [Fact]
    public async Task Should_be_able_get_data()
    {
        //-ARRANGE
        var dt = new DateTime(2023, 07, 23, 04, 18, 0);
        var seoRegisterNumber = "1238585";
        var fund = await _factory.Repositories.FundAddAsync(seoRegisterNumber: seoRegisterNumber);
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber: seoRegisterNumber, dtFinancial: dt);
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber: seoRegisterNumber, dtFinancial: dt.AddDays(-1));
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber: seoRegisterNumber, dtFinancial: dt.AddDays(-2));
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber: seoRegisterNumber, dtFinancial: dt.AddDays(-3));

        var dtoFilter = new FundNavFilterGetPaginatedListRequestTest(SeoRegisterNumber: seoRegisterNumber,
            StartDateTime: dt.AddDays(-10).GetISOStringDate(),
            EndDateTime: dt.GetISOStringDate());
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FundNavUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().NotBeEmpty();

        //-- check columns 
        apiResult.Metadata.Details
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.NavRedemption), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.NavSubscription), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.NavStat), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.NetAsset), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.NetChange), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.NetAsset), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.InvestorsUnits), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.UnitsSubscription), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.UnitsRedemption), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.EventDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.DateLastChange), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.InstitutionInvestmentPercent), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.RetailInvestmentPercent), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.InstitutionInvestmentNo), ColumnDataType.LONG)
            .ColumnAssertion(nameof(FundNavUITableRowResponseTest.RetailInvestmentNo), ColumnDataType.LONG);


        apiResult.Data.Content.Should().HaveCount(4);   
        foreach (var fundNavResponse in apiResult.Data.Content)
        {
            fundNavResponse.FundSeoRegisterNumber.Should().Be(seoRegisterNumber);
            fundNavResponse.FundId.Should().Be(fund.MutualFundIdPk);
            fundNavResponse.NavRedemption.Should().BePositive();
            fundNavResponse.NavSubscription.Should().BePositive();
            fundNavResponse.NavStat.Should().BePositive();
            fundNavResponse.NetAsset.Should().BePositive();
            fundNavResponse.NetChange.Should().BePositive();
            fundNavResponse.InvestorsUnits.Should().BePositive();
            fundNavResponse.UnitsSubscription.Should().BePositive();
            fundNavResponse.UnitsRedemption.Should().BePositive();
            fundNavResponse.EventDate.Should().NotBeNullOrEmpty();
            fundNavResponse.DateLastChange.Should().NotBeNullOrEmpty();
            fundNavResponse.InstitutionInvestmentPercent.Should().BePositive();
            fundNavResponse.RetailInvestmentPercent.Should().BePositive();
            fundNavResponse.InstitutionInvestmentNo.Should().BePositive();
            fundNavResponse.RetailInvestmentNo.Should().BePositive();
        }

    }

    [Fact]
    public async Task Should_be_able_get_data_withou_filter_date_for_10DaysLater()
    {
        //-ARRANGE
        var dt = new DateTime(2023, 07, 23, 04, 18, 0);
        var seoRegisterNumber = "1238585";
        var fund = await _factory.Repositories.FundAddAsync(seoRegisterNumber: seoRegisterNumber);
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber: seoRegisterNumber, dtFinancial: dt);
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber: seoRegisterNumber, dtFinancial: dt.AddDays(-1));
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber: seoRegisterNumber, dtFinancial: dt.AddDays(-2));
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber: seoRegisterNumber, dtFinancial: dt.AddDays(-3));

        var dtoFilter = new FundNavFilterGetPaginatedListRequestTest(SeoRegisterNumber: seoRegisterNumber);
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FundNavUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().NotBeEmpty();
        apiResult.Data.Content.Should().HaveCount(4);
        foreach (var fundNavResponse in apiResult.Data.Content)
        {
            fundNavResponse.FundSeoRegisterNumber.Should().Be(seoRegisterNumber);
            fundNavResponse.FundId.Should().Be(fund.MutualFundIdPk);
            fundNavResponse.NavRedemption.Should().BePositive();
            fundNavResponse.NavSubscription.Should().BePositive();
            fundNavResponse.NavStat.Should().BePositive();
            fundNavResponse.NetAsset.Should().BePositive();
            fundNavResponse.NetChange.Should().BePositive();
            fundNavResponse.InvestorsUnits.Should().BePositive();
            fundNavResponse.UnitsSubscription.Should().BePositive();
            fundNavResponse.UnitsRedemption.Should().BePositive();
            fundNavResponse.EventDate.Should().NotBeNullOrEmpty();
            fundNavResponse.DateLastChange.Should().NotBeNullOrEmpty();
            fundNavResponse.InstitutionInvestmentPercent.Should().BePositive();
            fundNavResponse.RetailInvestmentPercent.Should().BePositive();
            fundNavResponse.InstitutionInvestmentNo.Should().BePositive();
            fundNavResponse.RetailInvestmentNo.Should().BePositive();
        }

    }

}
