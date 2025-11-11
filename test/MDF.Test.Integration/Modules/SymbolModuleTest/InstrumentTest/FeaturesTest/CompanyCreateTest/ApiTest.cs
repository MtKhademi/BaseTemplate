using MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.FeaturesTest.CompanyCreateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "company-create")]
public partial class CompanyCreateTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/company";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public CompanyCreateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData("", "CODE-1400", null, null, "if Company Code exists, it must have a max length of 4", "Company Type Code is required", "Industrial Category Code is required")]
    [InlineData("", "", null, null, "Company Code is required", "Company Type Code is required", "Industrial Category Code is required")]
    [InlineData("XX-TITLE", "", null, null, "Company Code is required", "Company Type Code is required", "Industrial Category Code is required")]
    public async Task Should_not_be_able_create_when_not_valid_data_for_create_company(
        string title,
        string code,
        string companyTypeCode,
        string industrialCategoryCode,
        params string[] errorMessages)
    {
        //-ARRANGE
        var updateDto = new CompanyCreateDtoTest
        {
            Title = title,
            CompanyCode = code,
            CompanyTypeCode = companyTypeCode,
            IndustrialCategoryCode = industrialCategoryCode
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", updateDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(errorMessages.ToList());

    }


    [Theory]
    [InlineData("", "CODE", "A", "01")]
    public async Task Should_be_able_create_when_valid_data_for_create_company(
    string title,
    string code,
    string companyTypeCode,
    string industrialCategoryCode)
    {
        //-ARRANGE
        var updateDto = new CompanyCreateDtoTest
        {
            Title = title,
            CompanyCode = code,
            CompanyTypeCode = companyTypeCode,
            IndustrialCategoryCode = industrialCategoryCode
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", updateDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var companyCreated = await response.Content.ReadModelFromJsonAsync<CompanyGetDtoTest>();
        companyCreated.Should().NotBeNull();
        companyCreated.CompanyCode.Should().Be(code);
        companyCreated.CompanyTypeCode.Should().Be(companyTypeCode);
        companyCreated.IndustrialCategoryCode.Should().Be(industrialCategoryCode);
        companyCreated.CompanyId.Should().NotBeNull();
        companyCreated.Title.Should().Be(title);
    }


    [Fact]
    public async Task Should_not_be_able_create_again_a_company()
    {
        //-ARRANGE
        var createDto = new CompanyCreateDtoTest
        {
            Title = "",
            CompanyCode = "CODE",
            CompanyTypeCode = "A",
            IndustrialCategoryCode = "01"
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", createDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        //-ASSERT
        response = await _client.PostAsync($"{_api}", createDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(new List<string> { "there is already exist a CompanyModel =\u003E CompanyCode : CODE" });

    }
}