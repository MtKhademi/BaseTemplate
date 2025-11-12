using MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.FeaturesTest.IndustrialCategoryCreateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "industrial-category-create")]
public partial class IndustrialCategoryCreateTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/industrial-category";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public IndustrialCategoryCreateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [InlineData("", "", "Code is required")]
    [InlineData("", null, "Code is required")]
    public async Task Should_not_be_able_create_when_not_valid_data(
        string title,
        string code,
        params string[] errorMessages)
    {
        //-ARRANGE
        var addDto = new IndustrialCategoryCreateDtoTest
        {
            Title = title,
            Code = code,
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", addDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(errorMessages.ToList());

    }

    [Theory]
    [InlineData("", "10")]
    [InlineData("New title", "1")]
    public async Task Should_be_able_create_when_valid_data(
        string title,
        string code)
    {
        //-ARRANGE
        var addDto = new IndustrialCategoryCreateDtoTest
        {
            Title = title,
            Code = code,
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", addDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var industrailCategoryCreated = await response.Content.ReadModelFromJsonAsync<IndustrialCategoryGetDtoTest>();
        industrailCategoryCreated.Should().NotBeNull();
        industrailCategoryCreated.Code.Should().Be(code);
        industrailCategoryCreated.Title.Should().Be(title);
        industrailCategoryCreated.Id.Should().BeGreaterThan(0);
    }


    [Fact]
    public async Task Should_not_be_able_create_again_when_exist_already()
    {
        //-ARRANGE
        var addDto = new IndustrialCategoryCreateDtoTest
        {
            Title = "title",
            Code = "10",
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", addDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        //-ASSERT
        response = await _client.PostAsync($"{_api}", addDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(["there is already exist a IndustrialCategoryEntity => Code : 10"]);
    }

    [Fact]
    public async Task Should_not_be_able_create_when_not_exist_IndustrurialCategoryParent()
    {
        //-ARRANGE
        var addDto = new IndustrialCategoryCreateDtoTest
        {
            Title = "title",
            Code = "1",
            ParentIndustrialCategoryId = 100
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", addDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(["there is not exist any IndustrialCategoryEntity =\u003E IndustrialCategoryIdPk : 100"]);
    }

    [Fact]
    public async Task Should_be_able_create_with_parent()
    {
        //-ARRANGE
        var addDto = new IndustrialCategoryCreateDtoTest
        {
            Title = "title",
            Code = "10",
            ParentIndustrialCategoryId = 1
        };

        //-ACT
        var response = await _client.PostAsync($"{_api}", addDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IndustrialCategoryGetDtoTest>();
        apiResult.Should().NotBeNull();
    }
}