using MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.FeaturesTest.InstrumentAddTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "instrument-add")]
public partial class InstrumentAddATest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/instrument/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public InstrumentAddATest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    //[Theory]
    //[ClassData(typeof(MarketChangeAddOrUpdateDtoNotValidData))]
    //public async Task When_call_api_with_not_valid_data_Expect_get_bad_response(MarketChangeAddOrUpdateDtoV4Test dto, IEnumerable<string> messagesException)
    //{
    //    //-ARRANGE

    //    //-ACT
    //    var response = await _client.PostAsync(_api, dto.ToContentHttpString());
    //    await response.WriteOnConsoleAsync(_outPutHelper);

    //    //-ASSERT
    //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    //    var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
    //    apiResult.Assertion(messagesException);
    //}


    //[Fact]
    //public async Task When_not_exist_isin_Expect_get_not_found_symbol_response()
    //{
    //    //-ARRANGE
    //    var dto = new MarketChangeAddOrUpdateDtoV4Test()
    //    {
    //        FromSymbolCloseDate = "2025-01-10",
    //        FromSymbolIsin = "IRB5AE800008",
    //        ToSymbolOpenDate = "2025-01-11",
    //        ToSymbolIsin = "IRB5AE800009"
    //    };
    //    //-ACT
    //    var response = await _client.PostAsync(_api, dto.ToContentHttpString());
    //    await response.WriteOnConsoleAsync(_outPutHelper);

    //    //-ASSERT
    //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    //    var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
    //    apiResult.Assertion(["there is not exist any SymbolModel => Isin : IRB5AE800008"]);
    //}


    [Fact]
    public async Task When_send_valid_data_and_not_exist_already_Expect_add_new_instrument_and_get_ok_response()
    {
        //-ARRANGE
        var dto = new InstrumentAddDtoTest()
        {
            DateOfEvent = "2025-05-04",
            EnTitle = "test",
            CompanyCode = "BSTZ",
            InstrumentTypeCode = 300,
            Isin = "IRB5AE800008",
            UnitCount = 1000
        };


        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<InstrumentGetDtoTest>();
        apiResult.Should().NotBeNull();
        apiResult.Isin.Should().Be(dto.Isin);
        apiResult.EnTitle.Should().Be(dto.EnTitle);
        apiResult.UnitCount.Should().Be(dto.UnitCount);
        apiResult.DateOfEvent.Should().Be(dto.DateOfEvent);
        apiResult.InstrumentTypeCode.Should().Be(dto.InstrumentTypeCode);
        apiResult.InstrumentTypeId.Should().NotBeNull();
        apiResult.InstrumentTypeTitle.Should().NotBeNull();

    }


    //[Fact]
    //public async Task When_send_valid_data_and_exist_already_get_bad_request()
    //{
    //    //-ARRANGE
    //    var dt = "2025-01-26".ConvertToDateFromMiladiDate(dateSeperator: "-");
    //    await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800008");
    //    await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800009");
    //    var dto = new MarketChangeAddOrUpdateDtoV4Test()
    //    {
    //        ToSymbolIsin = "IRB5AE800009",
    //        ToSymbolOpenDate = dt.GetISOStringDateTime(),
    //        FromSymbolIsin = "IRB5AE800008",
    //        FromSymbolCloseDate = dt.AddDays(-1).GetISOStringDateTime()
    //    };


    //    //-ACT
    //    var response = await _client.PostAsync(_api, dto.ToContentHttpString());
    //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

    //    // send again and add again
    //    response = await _client.PostAsync(_api, dto.ToContentHttpString());
    //    await response.WriteOnConsoleAsync(_outPutHelper);

    //    //-ASSERT
    //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    //    var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
    //    apiResult.Assertion(["Market change from symbol IRB5AE800008 to symbol IRB5AE800009 already exist"]);
    //}

}
