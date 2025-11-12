using MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;
using Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafGetByIsinApiTest;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.FeaturesTest.InstrumentTypeGetByCodeTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "instrument-type-get-by-code")]
public partial class InstrumentTypeGetByCodeTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/instrument/instrument-type";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public InstrumentTypeGetByCodeTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_get_correct_instrument_type()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync($"{_api}/300");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var instrumentType = await response.Content.ReadModelFromJsonAsync<InstrumentTypeGetDtoTest>();
        instrumentType.Should().NotBeNull();
        instrumentType.Title.Should().Be("سهام");
        instrumentType.Code.Should().Be(300);
        instrumentType.Id.Should().Be(1);
    }
}