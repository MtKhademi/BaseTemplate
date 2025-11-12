using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.OptionHistoryListTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "option-history")]
public partial class OptionHistoryListTest : BaseTest
{
    private readonly string _apiAddress = $"/api/v4/symbol/bound/option/history";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public OptionHistoryListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_get_history_change_of_a_symbol()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 10, 12, 14, 51, 0);
        var isinBaseOption = "IRO1SDZJ0001";
        var symbolBaseOption = await _factory.Repositories.SymbolAddAsync(isin: isinBaseOption);

        var isinOption = "IRO1SDZJ0002";
        var symbolOption = await _factory.Repositories.SymbolAddAsync(isin: isinOption, typeOfSymbol: TypeOfSymbolTest.PutOption);

        await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 20000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(-20));

        await _factory.Repositories.PutOptionAddAsync(
         symbolId: symbolBaseOption.SymbolIdPk,
         symbolPutOptionId: symbolOption.SymbolIdPk,
         applyPrice: 50000,
         dtStart: dt.AddDays(-10),
         dtApply: dt.AddDays(-20),
         forFinance: MDF.Modules.SymbolModule.Depricate.Abstractions.ETypeOfPutOptinForFinance.OtherFinancing);


        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/{isinOption}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<OptionResponseTest>>();
        apiResult.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_be_get_history_with_one_Announcement()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 10, 12, 14, 51, 0);
        var isinBaseOption = "IRO1SDZJ0001";
        var symbolBaseOption = await _factory.Repositories.SymbolAddAsync(isin: isinBaseOption);

        var isinOption = "IRO1SDZJ0002";
        var symbolOption = await _factory.Repositories.SymbolAddAsync(isin: isinOption, typeOfSymbol: TypeOfSymbolTest.PutOption);

        await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 20000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(-20));

        await _factory.Repositories.PutOptionAddAsync(
         symbolId: symbolBaseOption.SymbolIdPk,
         symbolPutOptionId: symbolOption.SymbolIdPk,
         applyPrice: 50000,
         dtStart: dt.AddDays(-10),
         dtApply: dt.AddDays(-20),
         forFinance: MDF.Modules.SymbolModule.Depricate.Abstractions.ETypeOfPutOptinForFinance.OtherFinancing);

        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 85858);
        var dividend = await _factory.Repositories.DividendPerShareAddAsync(
            announcementId: announcement.AnnouncementIdPk, dtEntry: dt.AddDays(-2));

        await _factory.Repositories.PutOptionAddAsync(
         symbolId: symbolBaseOption.SymbolIdPk,
         symbolPutOptionId: symbolOption.SymbolIdPk,
         announcementId: announcement.AnnouncementIdPk.ToString(),
         applyPrice: 50000,
         dtStart: dt.AddDays(-10),
         dtApply: dt.AddDays(-20),
         forFinance: MDF.Modules.SymbolModule.Depricate.Abstractions.ETypeOfPutOptinForFinance.OtherFinancing);



        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/{isinOption}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<OptionResponseTest>>();
        apiResult.Should().NotBeNull();



        // ASSERT
        apiResult.Should().NotBeNull();
        apiResult.Should().HaveCount(3);

        var first = apiResult.FirstOrDefault();
        first.State.Should().Be("تقسیم سود");
        first.State.Should().Be("تقسیم سود");
        first.ApplyPrice.Should().Be(50000.0m);
        first.AnnouncementId.Should().NotBeNull();
        first.OptionForFinanceDescription.Should().Be("با هدف غیر از تامین مالی");

        var second = apiResult.Skip(1).First();
        second.State.Should().Be("شروع دوره");
        second.ApplyPrice.Should().Be(50000.0m);
        second.AnnouncementId.Should().BeNull();
        second.OptionForFinanceDescription.Should().Be("با هدف غیر از تامین مالی");

        var third = apiResult.Skip(2).First();
        third.State.Should().Be("شروع دوره");
        third.ApplyPrice.Should().Be(20000.0m);
        third.AnnouncementId.Should().BeNull();
        third.OptionForFinanceDescription.Should().Be("تایین نشده");

    }


    [Fact]
    public async Task Should_be_get_history_with_tow_Announcement()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 10, 12, 14, 51, 0);
        var isinBaseOption = "IRO1SDZJ0001";
        var symbolBaseOption = await _factory.Repositories.SymbolAddAsync(isin: isinBaseOption);

        var isinOption = "IRO1SDZJ0002";
        var symbolOption = await _factory.Repositories.SymbolAddAsync(isin: isinOption, typeOfSymbol: TypeOfSymbolTest.PutOption);

        var option1 = await _factory.Repositories.PutOptionAddAsync(
             symbolId: symbolBaseOption.SymbolIdPk,
             symbolPutOptionId: symbolOption.SymbolIdPk,
             applyPrice: 20000,
             dtStart: dt.AddDays(-10),
             dtApply: dt.AddDays(-20));

        var option2 = await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 50000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(-20),
            forFinance: MDF.Modules.SymbolModule.Depricate.Abstractions.ETypeOfPutOptinForFinance.OtherFinancing);

        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 85858);
        var dividend = await _factory.Repositories.DividendPerShareAddAsync(
            announcementId: announcement.AnnouncementIdPk, dtEntry: dt.AddDays(-2));

        var option3 = await _factory.Repositories.PutOptionAddAsync(
           symbolId: symbolBaseOption.SymbolIdPk,
           symbolPutOptionId: symbolOption.SymbolIdPk,
           announcementId: announcement.AnnouncementIdPk.ToString(),
           applyPrice: 50000,
           dtStart: dt.AddDays(-10),
           dtApply: dt.AddDays(-20),
           forFinance: MDF.Modules.SymbolModule.Depricate.Abstractions.ETypeOfPutOptinForFinance.OtherFinancing);


        var announcement1 = await _factory.Repositories.AnnouncementAddAsync(codalCode: 85858);
        var capitalChange = await _factory.Repositories.CapitalChangeAddAsync(
           announcementId: announcement1.AnnouncementIdPk, dtEntry: dt.AddDays(-2));

        var option4 = await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            announcementId: $"{announcement.AnnouncementIdPk};{announcement1.AnnouncementIdPk}",
            applyPrice: 50000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(-20),
            forFinance: MDF.Modules.SymbolModule.Depricate.Abstractions.ETypeOfPutOptinForFinance.OtherFinancing);



        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/{isinOption}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<OptionResponseTest>>();
        apiResult.Should().NotBeNull();



        // ASSERT
        apiResult.Should().NotBeNull();
        apiResult.Should().HaveCount(4);
        var expected = new[]
            {
        new {
            OptionId = option4.PutOptionIdPk,
            SymbolBaseId = symbolBaseOption.SymbolIdPk,
            SymbolBaseName = symbolBaseOption.SymbolName,
            SymbolBaseIsin = symbolBaseOption.Isin,
            SymbolOptionId = symbolOption.SymbolIdPk,
            SymbolOptionName = symbolOption.SymbolName,
            SymbolOptionIsin = symbolOption.Isin,
            ApplyPrice = 50000.0m,
            State = "تقسیم سود, افزایش سرمایه",
            AnnouncementId = $"{dividend.AnnouncementIdFk};{capitalChange.AnnouncementIdFk}",
            OptionForFinanceDescription = "با هدف غیر از تامین مالی"
        },
        new {
            OptionId = option3.PutOptionIdPk,
            SymbolBaseId = symbolBaseOption.SymbolIdPk,
            SymbolBaseName = symbolBaseOption.SymbolName,
            SymbolBaseIsin = symbolBaseOption.Isin,
            SymbolOptionId = symbolOption.SymbolIdPk,
            SymbolOptionName = symbolOption.SymbolName,
            SymbolOptionIsin = symbolOption.Isin,
            ApplyPrice = 50000.0m,
            State = "تقسیم سود",
            AnnouncementId = dividend.AnnouncementIdFk.ToString(),
            OptionForFinanceDescription = "با هدف غیر از تامین مالی"
        },
        new {
            OptionId = option2.PutOptionIdPk,
            SymbolBaseId = symbolBaseOption.SymbolIdPk,
            SymbolBaseName = symbolBaseOption.SymbolName,
            SymbolBaseIsin = symbolBaseOption.Isin,
            SymbolOptionId = symbolOption.SymbolIdPk,
            SymbolOptionName = symbolOption.SymbolName,
            SymbolOptionIsin = symbolOption.Isin,
            ApplyPrice = 50000.0m,
            State = "شروع دوره",
            AnnouncementId = (string)null,
            OptionForFinanceDescription = "با هدف غیر از تامین مالی"
        },
        new {
            OptionId = option1.PutOptionIdPk,
            SymbolBaseId = symbolBaseOption.SymbolIdPk,
            SymbolBaseName = symbolBaseOption.SymbolName,
            SymbolBaseIsin =symbolBaseOption.Isin,
            SymbolOptionId = symbolOption.SymbolIdPk,
            SymbolOptionName = symbolOption.SymbolName,
            SymbolOptionIsin = symbolOption.Isin,
            ApplyPrice = 20000.0m,
            State = "شروع دوره",
            AnnouncementId = (string)null,
            OptionForFinanceDescription = "تایین نشده"
        }
    };

        var apiList = apiResult.ToList();
        for (int i = 0; i < expected.Length; i++)
        {
            var actual = apiList[i];
            var exp = expected[i];
            actual.OptionId.Should().Be(exp.OptionId);
            actual.SymbolBaseId.Should().Be(exp.SymbolBaseId);
            actual.SymbolBaseName.Should().Be(exp.SymbolBaseName);
            actual.SymbolBaseIsin.Should().Be(exp.SymbolBaseIsin);
            actual.SymbolOptionId.Should().Be(exp.SymbolOptionId);
            actual.SymbolOptionName.Should().Be(exp.SymbolOptionName);
            actual.SymbolOptionIsin.Should().Be(exp.SymbolOptionIsin);
            actual.ApplyPrice.Should().Be(exp.ApplyPrice);
            actual.State.Should().Be(exp.State);
            actual.AnnouncementId.Should().Be(exp.AnnouncementId);
            actual.OptionForFinanceDescription.Should().Be(exp.OptionForFinanceDescription);
        }
    }
}
