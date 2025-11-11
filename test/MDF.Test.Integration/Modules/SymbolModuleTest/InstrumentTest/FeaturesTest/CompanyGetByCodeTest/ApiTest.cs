using Confluent.Kafka;
using MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;
using Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafGetByIsinApiTest;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.FeaturesTest.CompanyGetByCodeTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "company-get-by-code")]
public partial class CompanyGetByCodeTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/company";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public CompanyGetByCodeTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
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

        //-ACT
        var response = await _client.GetAsync($"{_api}/BSTZ120");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(["there is not exist any CompanyModel =\u003E CompanyCode : BSTZ120"]);

    }


    [Fact]
    public async Task Should_get_correct_instrument_type()
    {
        //-ARRANGE
        // - default in db
        //-ACT
        var response = await _client.GetAsync($"{_api}/BSTZ");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var company = await response.Content.ReadModelFromJsonAsync<CompanyGetDtoTest>();
        company.Should().NotBeNull();
        company.Title.Should().Be("مشاركت توسعه سه ماهه 20درصد");
        company.CompanyCode.Should().Be("BSTZ");
        company.CompanyId.Should().Be(1);
        company.DateOfEvent.Should().Be("2011-01-23");

        company.CompanyTypeId.Should().Be(1);
        company.CompanyTypeTitle.Should().Be("شرکت");
        company.CompanyTypeCode.Should().Be("A");


        company.IndustrialCategoryId.Should().Be(1);
        company.IndustrialCategoryTitle.Should().Be("زراعت و خدمات وابسته");
        company.IndustrialCategoryCode.Should().Be("01");
    
    }
}