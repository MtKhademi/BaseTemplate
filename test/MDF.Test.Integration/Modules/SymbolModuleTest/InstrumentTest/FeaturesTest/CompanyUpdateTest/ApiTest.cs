using MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.FeaturesTest.CompanyUpdateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "company-update")]
public partial class CompanyUpdateTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/company";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public CompanyUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData(1, "", "CODE-1400", null, "if company code exists, it must have a max length of 4")]
    public async Task Should_not_be_able_update_when_not_valid_data_for_update_company(int companyId,
        string title, string code, int? industrialCategoryId, params string[] errorMessages)
    {
        //-ARRANGE
        var updateDto = new CompanyUpdateDtoTest
        {
            CompanyId = companyId,
            Title = title,
            CompanyCode = code,
            IndustrialCategoryId = industrialCategoryId
        };

        //-ACT
        var response = await _client.PutAsync($"{_api}", updateDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(errorMessages.ToList());

    }


    [Fact]
    public async Task Should_not_be_able_update_when_not_exist_this_company()
    {
        //-ARRANGE
        var updateDto = new CompanyUpdateDtoTest
        {
            CompanyId = 100,
            Title = "title",
            CompanyCode = "code",
            IndustrialCategoryId = 1
        };

        //-ACT
        var response = await _client.PutAsync($"{_api}", updateDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(["there is not exist any CompanyModel =\u003E CompanyIdPk : 100"]);

    }


    [Fact]
    public async Task Should_not_be_able_update_when_not_exist_this_Industrial_category()
    {
        //-ARRANGE
        var updateDto = new CompanyUpdateDtoTest
        {
            CompanyId = 1,
            Title = "title",
            CompanyCode = "code",
            IndustrialCategoryId = 100
        };

        //-ACT
        var response = await _client.PutAsync($"{_api}", updateDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Be("IndustrialCategoryNotExistWithIdException");

    }


    [Fact]
    public async Task Should_be_able_update_when_exist_this_company()
    {
        //-ARRANGE
        var company = await _factory.Repositories.CompanyAddAsync();
        var updateDto = new CompanyUpdateDtoTest
        {
            CompanyId = company.CompanyIdPk,
            Title = "title- جدید برای تغییر",
            CompanyCode = "CD12"
        };

        //-ACT
        var response = await _client.PutAsync($"{_api}", updateDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<CompanyGetDtoTest>();
        apiResult.Should().NotBeNull();
        apiResult.CompanyId.Should().Be(company.CompanyIdPk);
        apiResult.Title.Should().Be(updateDto.Title);
        apiResult.CompanyCode.Should().Be(updateDto.CompanyCode);

    }


}