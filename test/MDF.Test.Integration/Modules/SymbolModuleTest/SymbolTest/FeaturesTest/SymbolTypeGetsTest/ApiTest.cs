namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.FeaturesTest.SymbolTypeGetsTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "type-get-list")]
public partial class SymbolTypeGetsTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/types";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public SymbolTypeGetsTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Theory]
    [ClassData(typeof(SymbolTypeGetsData))]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(BaseTypeOfSymbolTest? baseType, List<string> symbolTypes)
    {
        //-ARRANGE
        var api = $"{_api}?{(baseType == null ? "" : $"baseTypeOfSymbol={baseType}")}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<Dictionary<TypeOfSymbolTest, string>>();
        apiResult.Should().HaveCount(symbolTypes.Count);

        foreach (var symbolType in symbolTypes)
        {
            apiResult.Should().ContainKey((TypeOfSymbolTest)Enum.Parse(typeof(TypeOfSymbolTest), symbolType));
        }
    }
}
