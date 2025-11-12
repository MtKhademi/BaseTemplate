using MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Responses;
using TseTmcWcfService;

namespace MDF.Test.Integration.Modules.TseTmcModuleTest.FeaturesTest.TseTmcSymbolCreateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"TSE-TMC", "symbol-create")]
public partial class TseTmcSymbolUpdateTest : BaseTest, IClassFixture<TseTmcMockForCheckUpdate>
{
    private readonly string _apiAddress = $"/api/v4/tse-tmc/symbol";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    TseTmcMockForCheckUpdate _tseTmcMockForUpdate;
    public TseTmcSymbolUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper, TseTmcMockForCheckUpdate tseTmcMockForUpdate) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
        _tseTmcMockForUpdate = tseTmcMockForUpdate;
    }


    [Theory]
    [ClassData(typeof(DataForCreateSymbolFromTseTmcValid))]
    public async Task Shoud_be_able_create_symbol_from_TseTmc(string file, string isin, SymbolResponseTest dto)
    {
        //-ARRANGE
        _client = _factory.WithWebHostBuilder(conf =>
        {
            conf.ConfigureServices(ser =>
            {
                ser.RemoveAll<TsePublicV2Soap>();
                ser.AddScoped(_ => _tseTmcMockForUpdate.GetTseTmcMo(file, nameof(TseTmcSymbolCreateTest)).Object);
            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);


        //-ACT
        var response = await _client.PostAsync(_apiAddress, (new List<string> { isin }).ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);


        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response = await _client.GetAsync($"/api/v4/symbol/{isin}");
        await response.WriteOnConsoleAsync(_outPutHelper);
        var symbolCreatedData = await response.Content.ReadModelFromJsonAsync<SymbolResponseTest>();
        symbolCreatedData.Should().NotBeNull();
    }
}
