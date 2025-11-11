using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.FundRequests;
using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.FundsTest.FeaturesTest.FundCreateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND", "create")]
public partial class FundCreateTest : BaseTest
{
    private readonly string _api = $"/api/v4/fund";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FundCreateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [ClassData(typeof(FundCreateNotValidData))]
    public async Task Should_not_be_able_create_a_fund_when_not_send_correct_data(FundCreaterRequestTest dto, IEnumerable<string> errors)
    {
        //-ARRANGE

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(errors);
    }



    [Fact]
    public async Task Should_be_able_create_a_fund()
    {
        //-ARRANGE
        FundCreaterRequestTest dto = new FundCreaterRequestTest(
                SeoregisterNumber: "1258585",
                Title: "این یک صندوق جدید است",
                EnTitle: "This is a new fund ...",
                DateStart: "2025-07-01",
                FundProvider: FundProviderTest.Mofid,
                FundType: FundTypeTest.Leverage,
                FundXML: FundXMLTypeTest.Leverage
            );

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var fundResponse = await response.Content.ReadModelFromJsonAsync<FundResponseTest>();
        fundResponse.Should().NotBeNull();
        fundResponse.Should().NotBeNull();
        fundResponse.SeoregisterNumber.Should().Be(dto.SeoregisterNumber);
        fundResponse.Title.Should().Be(dto.Title);
        fundResponse.EnTitle.Should().Be(dto.EnTitle);
        fundResponse.FundType.Should().Be(dto.FundType);
        fundResponse.FundProvider.Should().Be(dto.FundProvider);
        fundResponse.FundXMLType.Should().Be(dto.FundXML);
    }


    [Fact]
    public async Task Should_not_be_able_create_a_fund_when_repeat_seoRegisterNumber()
    {
        //-ARRANGE
        FundCreaterRequestTest dto = new FundCreaterRequestTest(
                SeoregisterNumber: "1258585",
                Title: "این یک صندوق جدید است",
                EnTitle: "This is a new fund ...",
                DateStart: "2025-07-01",
                FundProvider: FundProviderTest.Mofid,
                FundType: FundTypeTest.Leverage,
                FundXML: FundXMLTypeTest.Leverage
            );
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);


        //-ACT
        response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);


        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
        apiResult.Messages.Should().NotBeNull();
        apiResult.Messages.Should().Contain($"there is already exist a Fund =\u003E seoRegisterNumber : {dto.SeoregisterNumber}");

    }

}
