using MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.RequestesTest;
using MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.ResponsesTest;
using System;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.FeaturesTest.ERApiFundGetListTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND-EXTENAL-RESOUCE [API]", "fund-get-list")]
public class ERApiFundGetListTest : BaseTest
{
    private readonly string _apiAddress = "api/v4/fund/extenal-resouce/api";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public ERApiFundGetListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        _outPutHelper = outPutHelper;
        var scope = factory.Services.CreateScope();
    }



    [Theory]
    [InlineData(null)]
    [InlineData(FundProviderTest.NotSet)]
    public async Task Should_get_error_when_not_send_FundProvider(FundProviderTest? fundProvider)
    {
        //-ARRANGE
        var request = new ERFundApiGetListRequestTest() { FundProvider = fundProvider };

        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}?{request.ToQueryString()}");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
    }


    [Theory]
    [InlineData(FundProviderTest.Rayan)]
    public async Task Should_get_error_when_not_found_any_service_for_this_provider(FundProviderTest fundProvider)
    {
        //-ARRANGE
        var request = new ERFundApiGetListRequestTest() { FundProvider = fundProvider };

        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}?{request.ToQueryString()}");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
    }


    [Theory]
    [ClassData(typeof(ERApiFundGetListTestValidData))]
    public async Task Should_be_able_to_get_fundResponse(
        (FundProviderTest fundProvider, string seoRegisterNumber, string directoryName, string xmlFile) dataForTest,
        IEnumerable<ERApiFundResponseTest> responsesExpected)
    {
        //-ARRANGE
        var dt = new DateTime(2025, 08, 01, 11, 54, 0);
        string xmlPath = Path.Combine(Directory.GetCurrentDirectory(),
            nameof(Modules),
            nameof(MutualFundModuleTest),
            nameof(ExternalResourcesTest),
            nameof(ApiTest), dataForTest.directoryName, dataForTest.xmlFile);

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
        var request = new ERFundApiGetListRequestTest()
        {
            FundProvider = dataForTest.fundProvider,
            SeoRegisterNumber = dataForTest.seoRegisterNumber
        };
        var response = await _client.GetAsync($"{_apiAddress}?{request.ToQueryString()}");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<ERApiFundResponseTest>>();

        foreach (var fundExpected in responsesExpected)
        {
            var fundActual = apiResult.FirstOrDefault(f => f.RegNo == fundExpected.RegNo);
            fundActual.Should().NotBeNull($"Fund with RegNo {fundExpected.RegNo} not found in response");
            fundActual.Should().NotBeNull($"Fund with RegNo {fundExpected.RegNo} not found in response");

            fundActual.RegNo.Should().Be(fundExpected.RegNo);
            fundActual.FundType.Should().Be(fundExpected.FundType);
            fundActual.FundSize.Should().Be(fundExpected.FundSize);
            fundActual.Date.Should().Be(fundExpected.Date);
            fundActual.NAVRed.Should().Be(fundExpected.NAVRed);
            fundActual.FundProfits.Should().NotBeNull();
            fundActual.FundProfits.Should().HaveCount(fundExpected.FundProfits.Count);
            for (int i = 0; i < fundExpected.FundProfits.Count; i++)
            {
                fundActual.FundProfits[i].Date.Should().Be(fundExpected.FundProfits[i].Date);
                fundActual.FundProfits[i].Value.Should().Be(fundExpected.FundProfits[i].Value);
            }
            fundActual.NAVSub.Should().Be(fundExpected.NAVSub);
            fundActual.NAVStat.Should().Be(fundExpected.NAVStat);
            fundActual.InitiationDate.Should().Be(fundExpected.InitiationDate);
            fundActual.NetAsset.Should().Be(fundExpected.NetAsset);
            fundActual.Units.Should().Be(fundExpected.Units);
            fundActual.UnitsSubDAY.Should().Be(fundExpected.UnitsSubDAY);
            fundActual.UnitsSubFromFirst.Should().Be(fundExpected.UnitsSubFromFirst);
            fundActual.UnitsRedDAY.Should().Be(fundExpected.UnitsRedDAY);
            fundActual.UnitsRedFromFirst.Should().Be(fundExpected.UnitsRedFromFirst);

            fundActual.Portfolio.Should().NotBeNull();
            fundActual.Portfolio.Cash.Should().BeApproximately(fundExpected.Portfolio.Cash, 0.0001);
            fundActual.Portfolio.Deposit.Should().BeApproximately(fundExpected.Portfolio.Deposit, 0.0001);
            fundActual.Portfolio.Bond.Should().BeApproximately(fundExpected.Portfolio.Bond, 0.0001);
            fundActual.Portfolio.FiveBest.Should().BeApproximately(fundExpected.Portfolio.FiveBest, 0.0001);
            fundActual.Portfolio.Stock.Should().BeApproximately(fundExpected.Portfolio.Stock, 0.0001);
            fundActual.Portfolio.Other.Should().BeApproximately(fundExpected.Portfolio.Other, 0.0001);

            fundActual.Custodian.Should().Be(fundExpected.Custodian);
            fundActual.CustodianEng.Should().Be(fundExpected.CustodianEng);
            fundActual.Guarantor.Should().Be(fundExpected.Guarantor);
            fundActual.GuarantorEng.Should().Be(fundExpected.GuarantorEng);
            fundActual.ProfitGuarantor.Should().Be(fundExpected.ProfitGuarantor);
            fundActual.Manager.Should().Be(fundExpected.Manager);
            fundActual.ManagerEng.Should().Be(fundExpected.ManagerEng);
            fundActual.InvestmentManager.Should().Be(fundExpected.InvestmentManager);
            fundActual.InvestmentManagerEng.Should().Be(fundExpected.InvestmentManagerEng);
            fundActual.RegistrationManager.Should().Be(fundExpected.RegistrationManager);
            fundActual.ExecutiveManager.Should().Be(fundExpected.ExecutiveManager);
            fundActual.MarketMaker.Should().Be(fundExpected.MarketMaker);
            fundActual.Auditor.Should().Be(fundExpected.Auditor);
            fundActual.AuditorEN.Should().Be(fundExpected.AuditorEN);
            fundActual.Name.Should().Be(fundExpected.Name);
            fundActual.NameEng.Should().Be(fundExpected.NameEng);
            fundActual.WebSite.Should().Be(fundExpected.WebSite);
            fundActual.RetInvNo.Should().Be(fundExpected.RetInvNo);
            fundActual.InsInvNo.Should().Be(fundExpected.InsInvNo);
            fundActual.RetInvPercent.Should().BeApproximately(fundExpected.RetInvPercent, 0.01);
            fundActual.InsInvPercent.Should().BeApproximately(fundExpected.InsInvPercent, 0.01);
            fundActual.NaturalPercent.Should().BeApproximately(fundExpected.NaturalPercent, 0.01);
            fundActual.LegalPercent.Should().BeApproximately(fundExpected.LegalPercent, 0.01);
            fundActual.GuaranteedEarningRate.Should().Be(fundExpected.GuaranteedEarningRate);
            fundActual.EstimatedEarningRate.Should().Be(fundExpected.EstimatedEarningRate);
            fundActual.DividentIntervalPeriod.Should().Be(fundExpected.DividentIntervalPeriod);
            fundActual.Day1Return.Should().BeApproximately(fundExpected.Day1Return, 0.001);
            fundActual.Day7Return.Should().BeApproximately(fundExpected.Day7Return, 0.001);
            fundActual.Day30Return.Should().BeApproximately(fundExpected.Day30Return, 0.001);
            fundActual.Day90Return.Should().BeApproximately(fundExpected.Day90Return, 0.001);
            fundActual.Day180Return.Should().BeApproximately(fundExpected.Day180Return, 0.001);
            fundActual.Day365Return.Should().BeApproximately(fundExpected.Day365Return, 0.001);
            fundActual.DayFirstReturn.Should().BeApproximately(fundExpected.DayFirstReturn, 0.001);
            fundActual.UnitsSub.Should().Be(fundExpected.UnitsSub);
            fundActual.UnitsRed.Should().Be(fundExpected.UnitsRed);
            fundActual.ManagerNationalCode.Should().Be(fundExpected.ManagerNationalCode);
            fundActual.FixedRedemptionFee.Should().Be(fundExpected.FixedRedemptionFee);
            fundActual.FixedSubscriptionFee.Should().Be(fundExpected.FixedSubscriptionFee);
            fundActual.VariableSubscriptionFee.Should().Be(fundExpected.VariableSubscriptionFee);
            fundActual.VariableSubscriptionFeeUpperLimit.Should().Be(fundExpected.VariableSubscriptionFeeUpperLimit);
            fundActual.SplitRate.Should().Be(fundExpected.SplitRate);

        }
    }
}
