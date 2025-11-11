using MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.RequestesTest;
using MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.ResponsesTest;
using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.FeaturesTest.ERApiFundCreateOrUpdateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND-EXTENAL-RESOUCE [API]", "fund-create-or-update")]
public class ERApiFundCreateOrUpdateTest : BaseTest
{
    private readonly string _apiAddress = "api/v4/fund/extenal-resouce/api";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public ERApiFundCreateOrUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
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
    [ClassData(typeof(ERApiFundCreateOrUpdateTestValidData))]
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
        var request = new ERFundApiCreateOrUpdateRequestTest()
        {
            FundProvider = dataForTest.fundProvider,
            SeoRegisterNumber = dataForTest.seoRegisterNumber
        };
        var response = await _client.PostAsync(_apiAddress, request.ToContentHttpString());
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        foreach (var fundExpected in responsesExpected)
        {
            response = await _client.GetAsync($"api/v4/fund/{fundExpected.RegNo}");
            _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var fundActual = await response.Content.ReadModelFromJsonAsync<FundResponseTest>();
            fundActual.Should().NotBeNull($"Fund with SeoRegisterNumber {fundExpected.RegNo} not found in response");

            fundActual.SeoregisterNumber.Should().Be(fundExpected.RegNo);
            fundActual.Title.Should().Be(fundExpected.Name);
            fundActual.EnTitle.Should().Be(fundExpected.NameEng);
            fundActual.Website.Should().Be(fundExpected.WebSite);
            fundActual.DateStart.Should().Be(fundExpected.InitiationDate
                .ToEnglishNumber().ConvertToDateFromPersianDate()
                .GetISOStringDate());

        }
    }
}
