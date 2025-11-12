using MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.RequestesTest;
using MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.ResponsesTest;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.FeaturesTest.ERApiFundNavCreateOrUpdate;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND-EXTENAL-RESOUCE [API]", "nav-create-or-update")]
public class ERApiFundNavGetTest : BaseTest
{
    private readonly string _apiAddress = "api/v4/fund/extenal-resouce/api/nav";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public ERApiFundNavGetTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        _outPutHelper = outPutHelper;
        var scope = factory.Services.CreateScope();
    }


    [Theory]
    [ClassData(typeof(FundNavFromApiDataValid))]
    public async Task Should_be_able_get_data_when_fund_nav_is_exist(
       (string seoRegisterNumber,
       FundProviderTest fundProvider,
       FundXMLTypeTest? fundXMLType,
       string date,
       string directoryName,
       string xmlNavFile) requierData,
       ERApiFundNavResponseTest expectedResult)
    {
        //-ARRANGE
        var request = new ERApiFundNavCreateOrUpdateRequestTest
        {
            SeoRegisterNumber = requierData.seoRegisterNumber,
            FromDate = requierData.date,
            ToDate = requierData.date,
            FundProvider = requierData.fundProvider,
            FundXmlType = requierData.fundXMLType
        };
        await _factory.Repositories.FundAddAsync(
            seoRegisterNumber: requierData.seoRegisterNumber,
            fundProvider: requierData.fundProvider,
            fundXMLType: requierData.fundXMLType);

        
        var dt = new DateTime(2025, 8, 5);
        string xmlPath = Path.Combine(Directory.GetCurrentDirectory(),
             nameof(Modules),
             nameof(MutualFundModuleTest),
             nameof(ExternalResourcesTest),
             nameof(ApiTest), requierData.directoryName, requierData.xmlNavFile);

        File.Exists(xmlPath).Should().BeTrue();

        var data = new StreamReader(xmlPath).ReadToEnd();
        var clientFactoryMock = MoqExtentions
            .CreateHttpClientFactoryMoq("ERApiHttpClient",
            HttpMethod.Get, data, It.IsAny<string>());

        _client = _factory.WithWebHostBuilder(hos =>
        {
            hos.ConfigureServices(cfg =>
            {
                var httpClients = cfg.SingleOrDefault(d => d.ServiceType == typeof(HttpClient));
                if (httpClients != null)
                    cfg.Remove(httpClients);
                cfg.AddSingleton(clientFactoryMock.Object);

                cfg.SetIDateTimeProvider_Now_Moq(dt);
            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);

        //-ACT
        var response = await _client.PostAsync(_apiAddress, request.ToContentHttpString());
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var navReslts = await response.Content.ReadModelFromJsonAsync<List<ERFundNavApiCreateOrUpdateResponseTest>>();
        navReslts.Count.Should().Be(1);

        var nav = navReslts.SingleOrDefault(x => x.Provider == requierData.fundProvider)?
            .NavResults.SingleOrDefault(x => x.SeoRegisterNumber == requierData.seoRegisterNumber)?
            .Nav;

        nav.Should().NotBeNull();
        // Compare expected and actual results
        nav.SeoRegisterNumber.Should().Be(requierData.seoRegisterNumber);
        nav.NavSubscription.Should().Be(expectedResult.NavSubscription);
        nav.NavRedemption.Should().Be(expectedResult.NavRedemption);
        nav.NavStat.Should().Be(expectedResult.NavStat);
        nav.NetAsset.Should().Be(expectedResult.NetAsset);
        nav.InvestorsUnits.Should().Be(expectedResult.InvestorsUnits);
        nav.UnitsSubscription.Should().Be(expectedResult.UnitsSubscription);
        nav.UnitsRedemption.Should().Be(expectedResult.UnitsRedemption);
        nav.EventDate.Should().Be(expectedResult.EventDate);
        nav.InstitutionInvestmentPercent.Should().Be(expectedResult.InstitutionInvestmentPercent);
        nav.RetailInvestmentPercent.Should().Be(expectedResult.RetailInvestmentPercent);
        nav.InstitutionInvestmentNo.Should().Be(expectedResult.InstitutionInvestmentNo);
        nav.RetailInvestmentNo.Should().Be(expectedResult.RetailInvestmentNo);

    }
}
