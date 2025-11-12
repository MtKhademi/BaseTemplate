using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.FundRequests;
using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.FundsTest.FeaturesTest.FundUpdateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND", "update")]
public partial class FundUpdateTest : BaseTest
{
    private readonly string _api = $"/api/v4/fund";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FundUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [ClassData(typeof(FundCreateNotValidData))]
    public async Task Should_not_be_able_create_a_fund_when_not_send_correct_data(FundUpdateRequestTest dto, IEnumerable<string> errors)
    {
        //-ARRANGE

        //-ACT
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(errors);
    }


    [Fact]
    public async Task Should_not_be_able_update_a_fund_when_not_exist()
    {
        //-ARRANGE
        FundUpdateRequestTest dto = new FundUpdateRequestTest(
                SeoregisterNumber: "1258585",
                Title: "این یک صندوق جدید است",
                EnTitle: "This is a new fund ...",
                DateStart: "2025-07-01",
                FundProvider: FundProviderTest.Mofid,
                FundType: FundTypeTest.Leverage,
                FundXML: FundXMLTypeTest.Leverage
            );

        //-ACT
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var fundResponse = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
    }


    [Theory]
    [InlineData("123123", "این یک صندوق جدید است", "This is a new fund ...", "2025-07-01",
        FundProviderTest.Mofid,
        FundTypeTest.Leverage,
        FundXMLTypeTest.Leverage,
        null,
        null)]

    [InlineData("1238585", "این یک صندوق جدید است", "This is a new fund ...", "2025-08-18",
        FundProviderTest.Tadbir,
        FundTypeTest.Leverage,
        FundXMLTypeTest.Leverage,
        120,
        null)]
    public async Task Should_be_able_update_a_fund(
        string seoRegisterNumber,
        string title,
        string enTitle,
        string dateStart,
        FundProviderTest fundProvider,
        FundTypeTest fundType,
        FundXMLTypeTest fundXML,
        int? managerId,
        string? dateLastChange)
    {
        //-ARRANGE
        if (managerId.HasValue)
        {
            await _factory.Repositories.OrganizationAddAsync(managerId.Value);
        }
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: seoRegisterNumber);
        FundUpdateRequestTest dto = new FundUpdateRequestTest(
                SeoregisterNumber: seoRegisterNumber,
                Title: title,
                EnTitle: enTitle,
                DateStart: dateStart,
                FundProvider: fundProvider,
                FundType: fundType,
                FundXML: fundXML,
                ManagerId: managerId,
                DateLastChanged: dateLastChange
            );


        //-ACT
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<FundResponseTest>();
        apiResult.Should().NotBeNull();
    }


    [Fact]
    public async Task Should_not_be_able_update_to_new_isin_when_exist_already()
    {
        //-ARRANGE
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123124", fundIsin: "IRB1233");
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123123", fundIsin: "IRB123");
        FundUpdateRequestTest dto = new FundUpdateRequestTest(
                SeoregisterNumber: "123123",
                Title: "این یک صندوق جدید است",
                EnTitle: "This is a new fund ...",
                DateStart: "2025-07-01",
                Isin: "IRB1233", // this isin already exist
                FundProvider: FundProviderTest.Mofid,
                FundType: FundTypeTest.Leverage,
                FundXML: FundXMLTypeTest.Leverage
            );


        //-ACT
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
        apiResult.Messages.Should().Contain("there is already exist a Fund =\u003E isin : IRB1233");
    }


    [Fact]
    public async Task Should_not_be_able_update_to_new_symbolIsin_when_exist_already()
    {
        //-ARRANGE
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123124", symbolIsin: "IRB1233");
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123123", symbolIsin: "IRB123");
        FundUpdateRequestTest dto = new FundUpdateRequestTest(
                SeoregisterNumber: "123123",
                Title: "این یک صندوق جدید است",
                EnTitle: "This is a new fund ...",
                DateStart: "2025-07-01",
                SymbolIsin: "IRB1233", // this isin already exist
                FundProvider: FundProviderTest.Mofid,
                FundType: FundTypeTest.Leverage,
                FundXML: FundXMLTypeTest.Leverage
            );


        //-ACT
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
        apiResult.Messages.Should().Contain("there is already exist a Fund =\u003E isin : IRB1233");
    }

}
