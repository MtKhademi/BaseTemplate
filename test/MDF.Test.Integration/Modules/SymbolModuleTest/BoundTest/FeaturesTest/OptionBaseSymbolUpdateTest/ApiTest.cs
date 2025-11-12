using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.OptionBaseSymbolUpdateTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "option-change-symbol-base")]
public partial class OptionBaseSymbolUpdateTest : BaseTest
{
    private readonly string _apiAddress = $"/api/v4/symbol/bound/option/change-symbol-base";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public OptionBaseSymbolUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "")]
    [InlineData("", null)]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    public async Task Should_cant_be_update_when_not_valid_data(string baseIsin, string optionIsin)
    {
        //-ARRANGE
        var dto = new OptionUpdateBaseSymbolRequestTest(baseIsin, optionIsin);

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("OptionBaseSymbolUpdateCommandException");
    }


    [Fact]
    public async Task Should_cant_be_update_when_not_exist_base_isin()
    {
        //-ARRANGE
        var dto = new OptionUpdateBaseSymbolRequestTest("IRB8585851", "IRB8585801");

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }


    [Fact]
    public async Task Should_cant_be_update_when_not_exist_option_isin()
    {
        //-ARRANGE
        var baseIsin = "IRB8585851";
        await _factory.Repositories.SymbolAddAsync(isin: baseIsin);
        var dto = new OptionUpdateBaseSymbolRequestTest(baseIsin, "IRB8585801");

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_cant_be_update_when_not_exist_option_data()
    {
        //-ARRANGE
        var baseIsin = "IRB8585851";
        var optionIsin = "IRB8585801";
        await _factory.Repositories.SymbolAddAsync(isin: baseIsin);
        await _factory.Repositories.SymbolAddAsync(isin: optionIsin, typeOfSymbol: TypeOfSymbolTest.PutOption);
        var dto = new OptionUpdateBaseSymbolRequestTest(baseIsin, optionIsin);

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_can_update()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 10, 19, 14, 50, 0);
        var baseIsinOld = "IRBOld85851";
        var baseIsin = "IRB8585851";
        var optionIsin = "IRB8585801";
        var symbolBaseOptionOld = await _factory.Repositories.SymbolAddAsync(isin: baseIsinOld);
        var symbolBaseOption = await _factory.Repositories.SymbolAddAsync(isin: baseIsin);
        var symbolOption = await _factory.Repositories.SymbolAddAsync(isin: optionIsin, typeOfSymbol: TypeOfSymbolTest.PutOption);

        var option = await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOptionOld.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 20000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(-20));

        var dto = new OptionUpdateBaseSymbolRequestTest(baseIsin, optionIsin);

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<OptionResponseTest>>();
        apiResult.Should().NotBeNull();
        var optionResponse = apiResult.FirstOrDefault();
        optionResponse.OptionId.Should().Be(option.PutOptionIdPk);
        optionResponse.SymbolBaseId.Should().Be(symbolBaseOption.SymbolIdPk);
        optionResponse.SymbolBaseIsin.Should().Be(symbolBaseOption.Isin);
        optionResponse.SymbolOptionId.Should().Be(symbolOption.SymbolIdPk);
        optionResponse.SymbolOptionIsin.Should().Be(symbolOption.Isin);
    }


    [Fact]
    public async Task Should_can_update_when_exist_multi_record()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 10, 19, 14, 50, 0);
        var baseIsinOld = "IRBOld85851";
        var baseIsin = "IRB8585851";
        var optionIsin = "IRB8585801";
        var symbolBaseOptionOld = await _factory.Repositories.SymbolAddAsync(isin: baseIsinOld);
        var symbolBaseOption = await _factory.Repositories.SymbolAddAsync(isin: baseIsin);
        var symbolOption = await _factory.Repositories.SymbolAddAsync(isin: optionIsin, typeOfSymbol: TypeOfSymbolTest.PutOption);

        var option = await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOptionOld.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 20000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(-20));


        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 85858);
        var dividend = await _factory.Repositories.DividendPerShareAddAsync(
            announcementId: announcement.AnnouncementIdPk, dtEntry: dt.AddDays(-2));

        await _factory.Repositories.PutOptionAddAsync(
         symbolId: symbolBaseOptionOld.SymbolIdPk,
         symbolPutOptionId: symbolOption.SymbolIdPk,
         announcementId: announcement.AnnouncementIdPk.ToString(),
         applyPrice: 50000,
         dtStart: dt.AddDays(-10),
         dtApply: dt.AddDays(-20),
         forFinance: MDF.Modules.SymbolModule.Depricate.Abstractions.ETypeOfPutOptinForFinance.OtherFinancing);

        var dto = new OptionUpdateBaseSymbolRequestTest(baseIsin, optionIsin);

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<OptionResponseTest>>();
        apiResult.Should().NotBeNull();
        var optionResponse = apiResult.FirstOrDefault();
        optionResponse.OptionId.Should().Be(option.PutOptionIdPk);
        optionResponse.SymbolBaseId.Should().Be(symbolBaseOption.SymbolIdPk);
        optionResponse.SymbolBaseIsin.Should().Be(symbolBaseOption.Isin);
        optionResponse.SymbolOptionId.Should().Be(symbolOption.SymbolIdPk);
        optionResponse.SymbolOptionIsin.Should().Be(symbolOption.Isin);
    }

}
