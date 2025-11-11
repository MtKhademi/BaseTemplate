using MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.FeaturesTest.InstrumentUpdateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "instrument-update")]
public partial class InstrumentUpdateTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/instrument";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public InstrumentUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_not_be_able_update_when_not_exist_this_instrument()
    {
        //-ARRANGE
        var updateDto = new InstrumentUpdateDtoTest
        {
            InstrumentTypeId = 1,
            EnTitle = "title",
            InstrumentId = 100,
            UnitCount = 1
        };

        //-ACT
        var response = await _client.PutAsync($"{_api}", updateDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(["there is not exist any InstrumentEntity =\u003E InstrumentIdPk : 100"]);

    }


    [Fact]
    public async Task Should_not_be_able_update_when_not_exist_this_InstrumentType()
    {
        //-ARRANGE
        var updateDto = new InstrumentUpdateDtoTest
        {
            InstrumentTypeId = 100,
            EnTitle = "title",
            InstrumentId = 1,
            UnitCount = 1
        };

        //-ACT
        var response = await _client.PutAsync($"{_api}", updateDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(["there is not exist any InstrumentTypeEntity => InstrumentTypeIdPk : 100"]);

    }


    [Fact]
    public async Task Should_be_able_update_when_exist_this_instrument()
    {
        //-ARRANGE
        var instrument = await _factory.Repositories.InstrumentAddAsync(
              title: "عنوان قدیمی",
              unitCount: 10);

        var updateDto = new InstrumentUpdateDtoTest
        {
            InstrumentId = instrument.InstrumentIdPk,
            InstrumentTypeId = 1,
            EnTitle = "title - این یک تایتل جدید است",
            UnitCount = 10000
        };

        //-ACT
        var response = await _client.PutAsync($"{_api}", updateDto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<InstrumentGetDtoTest>();
        apiResult.Should().NotBeNull();
        apiResult.InstrumentId.Should().Be(updateDto.InstrumentId);
        apiResult.EnTitle.Should().Be(updateDto.EnTitle);
        apiResult.UnitCount.Should().Be(updateDto.UnitCount);
    }
}