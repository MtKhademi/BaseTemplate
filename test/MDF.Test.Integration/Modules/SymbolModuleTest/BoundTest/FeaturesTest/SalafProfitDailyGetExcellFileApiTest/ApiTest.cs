namespace Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafProfitDailyGetExcellFileApiTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "salaf-get-excell-file")]
public partial class FixedIncomeProfitDailyGetExcellFileApiTest : BaseTest
{
    private readonly string _apiAddress = $"/api/v4/symbol/bound/salaf/profit-daily/[ISIN]/file";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FixedIncomeProfitDailyGetExcellFileApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper)
        : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        _client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("multipart/form-data"));
        _outPutHelper = outPutHelper;
    }



    [Fact]
    public async Task When_call_api_with_not_exist_symbol_Expect_get_not_found_response()
    {
        //-ARRANGE
        await AddRequierAsync();
        var pathFileDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            nameof(ModulesTest),
            nameof(SymbolModuleTest),
            nameof(BoundTest),
            nameof(FeaturesTest),
            nameof(SalafProfitDailyGetExcellFileApiTest));

        if (!Directory.Exists(pathFileDirectory))
            Directory.CreateDirectory(pathFileDirectory);

        var pathFile = Path.Combine(pathFileDirectory, "profitDaily.xlsx");

        if (File.Exists(pathFile))
            File.Delete(pathFile);

        var symbolIsin = "IRB5AE800018";

        //ACT
        var response = await _client.GetAsync($"{_apiAddress.Replace("[ISIN]", symbolIsin)}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //ASSERTION
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using (var ms = await response.Content.ReadAsStreamAsync())
        using (var fs = File.Create(pathFile))
        {
            await ms.CopyToAsync(fs);
            fs.Flush();
        }

    }
}
