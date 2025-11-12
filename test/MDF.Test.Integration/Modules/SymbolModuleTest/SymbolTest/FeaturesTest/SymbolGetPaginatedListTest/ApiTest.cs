using MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Responses;
using SymbolModule.Contract.Symbold.Requests;
using System.Net;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.FeaturesTest.SymbolGetPaginatedListTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "get-paginated")]
public partial class SymbolGetPaginatedListTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/paginated";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public SymbolGetPaginatedListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Theory]
    [InlineData("2025-09-08", "2025-09-09")]
    public async Task Should_be_able_get_data_with_custom_filter(
        string? fromDateChanged = default!,
        string? toDateChanged = default!)
    {
        //-ARRANGE
        await _factory.Repositories.SymbolAddAsync(isin: "IRBXXXA001",
            dtCreate: DateTime.Parse("2025-09-07"),
            dtEntry: DateTime.Parse("2025-09-07"));
        await _factory.Repositories.SymbolAddAsync(isin: "IRBXXXA011",
            dtCreate: DateTime.Parse("2025-09-08"),
            dtEntry: DateTime.Parse("2025-09-08"));
        await _factory.Repositories.SymbolAddAsync(isin: "IRBXXXA021",
            dtCreate: DateTime.Parse("2025-09-05"),
            dtEntry: DateTime.Parse("2025-09-05"),
            dtLastUpdate: DateTime.Parse("2025-09-09"));

        //-ACT
        var response = await _client.GetAsync($"{_api}?" +
            $"{new SymbolGetPaginatedListRequestTest
            {
                FromDateChanged = fromDateChanged
            }.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var paginatedResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<SymbolResponseTest>>();
        paginatedResult.Should().NotBeNull();
        paginatedResult.Data.Should().NotBeNull();
        paginatedResult.Data.Count().Should().BeGreaterThanOrEqualTo(1);

        foreach (var symbol in paginatedResult.Data)
        {
            symbol.EntryDate.Should().NotBeNull();
            symbol.LastModifiedDate.Should().NotBeNull();
            var entryDate = symbol.EntryDate.GetDateFromISOFormat();
            var lastModified = symbol.LastModifiedDate.GetDateFromISOFormat();
            if (!string.IsNullOrWhiteSpace(fromDateChanged))
            {
                var fromDateChangedDt = DateTime.Parse(fromDateChanged);
                (fromDateChangedDt.Date <= entryDate.Date || fromDateChangedDt.Date <= lastModified.Date).Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(toDateChanged))
            {
                var toDateChangeDt = DateTime.Parse(toDateChanged);
                (toDateChangeDt.Date >= entryDate.Date || toDateChangeDt.Date >= lastModified.Date).Should().BeTrue(
                    $"isin : {symbol.Isin} - toDateChange : {toDateChanged} [entryDate: {entryDate} :: modifiedDate: {lastModified}]");
            }
        }
    }

}
