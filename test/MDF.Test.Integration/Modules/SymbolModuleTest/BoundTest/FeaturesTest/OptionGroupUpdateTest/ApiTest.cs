using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.OptionGroupUpdateTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "option-update-group")]
public partial class OptionGroupUpdateTest : BaseTest
{
    private readonly string _apiAddress = $"/api/v4/symbol/bound/option/group";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public OptionGroupUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [InlineData(null, null, null)]
    public async Task Should_cant_be_able_update_when_not_send_correct_data(string? applyNewDate, string? startNewDate, params int[]? optionIds)
    {
        //-ARRANGE
        var dto = new OptionGroupUpdateRequestTest(optionIds, applyNewDate, startNewDate);

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("OptionGroupUpdateRequestException");
    }


    [Fact]
    public async Task Should_cant_be_able_update_when_not_exist_options()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 10, 25, 11, 21, 0);
        var dto = new OptionGroupUpdateRequestTest(new[] { 1 },
            dt.AddYears(2).GetISOStringDate(),
            dt.GetISOStringDate());

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("OptionNotExistException");
    }

    [Fact]
    public async Task Should_be_able_update()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 10, 25, 11, 21, 0);
        var isin = "IRBXX8585";
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: isin);

        var optionIsin = "IRBOption8581";
        var optionSymbol = await _factory.Repositories.SymbolAddAsync(isin: optionIsin, typeOfSymbol: TypeOfSymbolTest.PutOption);

        var option = await _factory.Repositories.PutOptionAddAsync(symbolId: symbol.SymbolIdPk,
            symbolPutOptionId: optionSymbol.SymbolIdPk,
            applyPrice: 100,
            dtStart: dt,
            dtApply: dt.AddYears(10));

        var dto = new OptionGroupUpdateRequestTest(new[] { option.PutOptionIdPk },
            dt.AddYears(2).GetISOStringDate(),
            dt.GetISOStringDate());

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<OptionResponseTest>>();
        apiResult.Should().NotBeNull();
        var optionResult = apiResult.First();
        optionResult.Should().NotBeNull();
        optionResult.OptionId.Should().Be(option.PutOptionIdPk);
        optionResult.ApplyDate.Should().Be(dt.AddYears(2).GetISOStringDate());
        optionResult.StartDate.Should().Be(dt.GetISOStringDate());
        optionResult.SymbolBaseId.Should().Be(symbol.SymbolIdPk);
        optionResult.SymbolBaseIsin.Should().Be(isin);
        optionResult.SymbolOptionId.Should().Be(optionSymbol.SymbolIdPk);
        optionResult.SymbolOptionIsin.Should().Be(optionSymbol.Isin);

    }


    [Fact]
    public async Task Should_be_able_update_multi_option()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 10, 25, 11, 21, 0);
        var isin = "IRBXX8585";
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: isin);

        var optionIsin1 = "IRBOption8581";
        var optionSymbol1 = await _factory.Repositories.SymbolAddAsync(isin: optionIsin1, typeOfSymbol: TypeOfSymbolTest.PutOption);

        var option1 = await _factory.Repositories.PutOptionAddAsync(symbolId: symbol.SymbolIdPk,
            symbolPutOptionId: optionSymbol1.SymbolIdPk,
            applyPrice: 100,
            dtStart: dt.AddDays(3),
            dtApply: dt.AddYears(10));

        var optionIsin2 = "IRBOption8582";
        var optionSymbol2 = await _factory.Repositories.SymbolAddAsync(isin: optionIsin2, typeOfSymbol: TypeOfSymbolTest.PutOption);

        var option2 = await _factory.Repositories.PutOptionAddAsync(symbolId: symbol.SymbolIdPk,
            symbolPutOptionId: optionSymbol2.SymbolIdPk,
            applyPrice: 100,
            dtStart: dt.AddDays(100),
            dtApply: dt.AddYears(10));

        var optionIsin3 = "IRBOption8583";
        var optionSymbol3 = await _factory.Repositories.SymbolAddAsync(isin: optionIsin3, typeOfSymbol: TypeOfSymbolTest.PutOption);

        var option3 = await _factory.Repositories.PutOptionAddAsync(symbolId: symbol.SymbolIdPk,
            symbolPutOptionId: optionSymbol3.SymbolIdPk,
            applyPrice: 100,
            dtStart: dt.AddMonths(10),
            dtApply: dt.AddYears(10));

        var dto = new OptionGroupUpdateRequestTest(new[] { option1.PutOptionIdPk, option3.PutOptionIdPk, option2.PutOptionIdPk },
            dt.AddYears(2).GetISOStringDate(),
            dt.GetISOStringDate());

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<OptionResponseTest>>();
        apiResult.Should().NotBeNull();

        var optionResult1 = apiResult.FirstOrDefault(o => o.OptionId == option1.PutOptionIdPk);
        optionResult1.Should().NotBeNull();
        optionResult1!.ApplyDate.Should().Be(dt.AddYears(2).GetISOStringDate());
        optionResult1.StartDate.Should().Be(dt.GetISOStringDate());
        var optionResult2 = apiResult.FirstOrDefault(o => o.OptionId == option2.PutOptionIdPk);
        optionResult2.Should().NotBeNull();
        optionResult2!.ApplyDate.Should().Be(dt.AddYears(2).GetISOStringDate());
        optionResult2.StartDate.Should().Be(dt.GetISOStringDate());
        var optionResult3 = apiResult.FirstOrDefault(o => o.OptionId == option3.PutOptionIdPk);
        optionResult3.Should().NotBeNull();
        optionResult3!.ApplyDate.Should().Be(dt.AddYears(2).GetISOStringDate());
        optionResult3.StartDate.Should().Be(dt.GetISOStringDate());

    }


    [Fact]
    public async Task When_update_options_not_change_another_data()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 10, 25, 11, 21, 0);
        var isin = "IRBXX8585";
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: isin);

        var optionIsin1 = "IRBOption8581";
        var optionSymbol1 = await _factory.Repositories.SymbolAddAsync(isin: optionIsin1, typeOfSymbol: TypeOfSymbolTest.PutOption);

        var option1 = await _factory.Repositories.PutOptionAddAsync(symbolId: symbol.SymbolIdPk,
            symbolPutOptionId: optionSymbol1.SymbolIdPk,
            applyPrice: 100,
            dtStart: dt.AddDays(3),
            dtApply: dt.AddYears(10));

        var optionIsin2 = "IRBOption8582";
        var optionSymbol2 = await _factory.Repositories.SymbolAddAsync(isin: optionIsin2, typeOfSymbol: TypeOfSymbolTest.PutOption);

        var option2 = await _factory.Repositories.PutOptionAddAsync(symbolId: symbol.SymbolIdPk,
            symbolPutOptionId: optionSymbol2.SymbolIdPk,
            applyPrice: 100,
            dtStart: dt.AddDays(100),
            dtApply: dt.AddYears(10));


        isin = "IRBNew8585";
        symbol = await _factory.Repositories.SymbolAddAsync(isin: isin);
        var optionIsin3 = "IRNOption8583";
        var optionSymbol3 = await _factory.Repositories.SymbolAddAsync(isin: optionIsin3, typeOfSymbol: TypeOfSymbolTest.PutOption);

        var option3 = await _factory.Repositories.PutOptionAddAsync(symbolId: symbol.SymbolIdPk,
            symbolPutOptionId: optionSymbol3.SymbolIdPk,
            applyPrice: 100,
            dtStart: dt.AddMonths(10),
            dtApply: dt.AddYears(10));

        var dto = new OptionGroupUpdateRequestTest(new[] { option1.PutOptionIdPk, option2.PutOptionIdPk },
            dt.AddYears(2).GetISOStringDate(),
            dt.GetISOStringDate());

        //-ACT
        var response = await _client.PutAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<OptionResponseTest>>();
        apiResult.Should().NotBeNull();
        var optionResult1 = apiResult.FirstOrDefault(o => o.OptionId == option1.PutOptionIdPk);
        optionResult1.Should().NotBeNull();
        optionResult1.ApplyDate.Should().Be(dt.AddYears(2).GetISOStringDate());
        optionResult1.StartDate.Should().Be(dt.GetISOStringDate());

        var optionResult2 = apiResult.FirstOrDefault(o => o.OptionId == option2.PutOptionIdPk);
        optionResult2.Should().NotBeNull();
        optionResult2.ApplyDate.Should().Be(dt.AddYears(2).GetISOStringDate());
        optionResult2.StartDate.Should().Be(dt.GetISOStringDate());

    }
}
