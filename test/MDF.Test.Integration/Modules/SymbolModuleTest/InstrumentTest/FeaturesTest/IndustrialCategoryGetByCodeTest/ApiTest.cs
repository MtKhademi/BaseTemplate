using MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;
using Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafGetByIsinApiTest;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.FeaturesTest.IndustrialCategoryGetByCodeTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "industrial-category-get-by-code")]
public partial class IndustrialCategoryGetByCodeTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/industrial-category";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public IndustrialCategoryGetByCodeTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_not_be_able_get_data_if_not_exist_data()
    {
        //-ARRANGE
        // - default in db
        //-ACT
        var response = await _client.GetAsync($"{_api}/BSTZ");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("IndustrialCategoryNotExistWithCodeException");

    }


    [Theory]
    [InlineData("01", "زراعت و خدمات وابسته")]
    public async Task Should_be_get_correct_data_base_code(string code, string title)
    {
        //-ARRANGE
        // - default in db
        //-ACT
        var response = await _client.GetAsync($"{_api}/{code}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var company = await response.Content.ReadModelFromJsonAsync<IndustrialCategoryGetDtoTest>();
        company.Should().NotBeNull();
        company.Code.Should().Be(code);
        company.Title.Should().Be(title);
    }
}