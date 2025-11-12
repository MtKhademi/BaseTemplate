using MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;
using SymbolModule.Contract.Instrument.Dtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.FeaturesTest.InstrumentGetListByFilterTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "instrument-get-by-filter")]
public partial class InstrumentGetListByFilterTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/instrument/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public InstrumentGetListByFilterTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData(null)]
    [InlineData("IRBRSZ000001")]
    public async Task Should_be_get_all_instrument_base_custome_filter(string isin)
    {
        //-ARRANGE
        var dto = new InstrumentFilterDtoTest
        {
            Isin = isin
        };

        //-ACT
        var response = await _client.GetAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var instruments = await response.Content.ReadModelFromJsonAsync<IEnumerable<InstrumentGetDto>>();
        foreach (var instrument in instruments)
        {
            if (!string.IsNullOrWhiteSpace(isin))
                instrument.Isin.Should().Be(isin);
        }
    }
}
