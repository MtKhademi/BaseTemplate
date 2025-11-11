using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.OptionGetUITableTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "option-ui-table")]
public partial class FixIncomeGetTableApiTest : BaseTest
{
    private readonly string _apiGetTable = $"/api/v4/symbol/bound/option/ui-table";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FixIncomeGetTableApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task When_get_table_Expect_get_information_data_and_OK_response()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync(_apiGetTable);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var tableDto = await response.Content.ReadModelFromJsonAsync<BaseTableDto<OptionUITableRowResponseTest>>();
        //- check basic------------------------------------------
        tableDto.Metadata.Basic.PersianTitle.Should().NotBeNullOrWhiteSpace();
        tableDto.Metadata.Basic.HasRowNumber.Should().BeTrue();
        tableDto.Data.Should().NotBeNull();
        tableDto.Data.Pageable.Should().NotBeNull();
        tableDto.Data.Content.Should().NotBeNull();
        tableDto.Metadata.Details.Should().NotBeNull();

        //-check columns---------------------------------------
        tableDto.Metadata.Details.Count.Should().Be(10);

        //-- check columns 
        tableDto.Metadata.Details
            .ColumnAssertion(nameof(OptionUITableRowResponseTest.SymbolBaseIsin), ColumnDataType.STRING)
            .ColumnAssertion(nameof(OptionUITableRowResponseTest.SymbolBaseName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(OptionUITableRowResponseTest.SymbolOptionIsin), ColumnDataType.STRING)
            .ColumnAssertion(nameof(OptionUITableRowResponseTest.SymbolOptionName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(OptionUITableRowResponseTest.ApplyPrice), ColumnDataType.INT)
            .ColumnAssertion(nameof(OptionUITableRowResponseTest.State), ColumnDataType.STRING)
            .ColumnAssertion(nameof(OptionUITableRowResponseTest.ApplyDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(OptionUITableRowResponseTest.StartDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(OptionUITableRowResponseTest.OptionForFinanceDescription), ColumnDataType.STRING)
            .ColumnAssertion(nameof(OptionUITableRowResponseTest.Actions), ColumnDataType.LIST);

        tableDto.Data.Pageable.PageNumber.Should().Be(0);
        tableDto.Data.Pageable.PageSize.Should().Be(100);
    }

    [Theory]
    [ClassData(typeof(OptionPaginatedListRequestTestNotValidData))]
    public async Task When_not_valid_filter_Expect_get_bad_response(OptionPaginatedListRequestTest dto, List<string> errors)
    {

        //-ARRANGE
        var api = $"{_apiGetTable}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.Messages.Should().BeEquivalentTo(errors);
    }


    [Fact]
    public async Task When_call_api_without_filter_Expect_get_ok_response_and_currect_data()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 09, 06, 15, 18, 0);
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

        var dtoFilter = new OptionPaginatedListRequestTest();
        var api = $"{_apiGetTable}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<OptionUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        //apiResult.Data.Content.Should().HaveCount(count);
        foreach (var item in apiResult.Data.Content)
        {
            item.SymbolBaseIsin.Should().Be(isinBaseOption);
            item.SymbolOptionIsin.Should().Be(isinOption);
            item.ApplyPrice.Should().Be(20000);
            item.StartDate.Should().Be(dt.AddDays(-10).GetISOStringDate());
            item.ApplyDate.Should().Be(dt.AddDays(-20).GetISOStringDate());
            item.Actions.Should().Contain("DETAILS");
            item.SymbolBaseIsin.Should().NotBeNullOrWhiteSpace();
            item.SymbolOptionIsin.Should().NotBeNullOrWhiteSpace();
            item.SymbolBaseName.Should().NotBeNullOrWhiteSpace();
            item.SymbolOptionName.Should().NotBeNullOrWhiteSpace();
            item.Actions.Should().Contain(["DETAILS"]);
        }
    }

    [Fact]
    public async Task When_a_symbol_have_some_record_get_last_record_in_PaginatedTable()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 09, 14, 10, 22, 0);
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

        var dtoFilter = new OptionPaginatedListRequestTest();
        var api = $"{_apiGetTable}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<OptionUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().HaveCount(1);
        foreach (var item in apiResult.Data.Content)
        {
            item.SymbolBaseIsin.Should().Be(isinBaseOption);
            item.SymbolOptionIsin.Should().Be(isinOption);
            item.ApplyPrice.Should().Be(50000);
            item.StartDate.Should().Be(dt.AddDays(-10).GetISOStringDate());
            item.ApplyDate.Should().Be(dt.AddDays(-20).GetISOStringDate());
            item.Actions.Should().Contain("DETAILS");
            item.SymbolBaseIsin.Should().NotBeNullOrWhiteSpace();
            item.SymbolOptionIsin.Should().NotBeNullOrWhiteSpace();
            item.SymbolBaseName.Should().NotBeNullOrWhiteSpace();
            item.SymbolOptionName.Should().NotBeNullOrWhiteSpace();
            item.Actions.Should().Contain(["DETAILS"]);
            item.State.Should().Be("شروع دوره");
            item.OptionForFinanceDescription.Should().Be("با هدف غیر از تامین مالی");
        }
    }


    [Fact]
    public async Task Should_have_to_get_order_correct()
    {
        //-ARRANGE
        var dtFirst = new DateTime(2025, 09, 14, 07, 22, 0);
        var isinBaseOption = "IRO1SDZJ0001";
        var symbolBaseOption = await _factory.Repositories.SymbolAddAsync(isin: isinBaseOption);

        var isinOption = "IRO1SDZJ0011";
        var symbolOption = await _factory.Repositories.SymbolAddAsync(isin: isinOption,
            typeOfSymbol: TypeOfSymbolTest.PutOption,
            dtEvent: dtFirst);

        await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 20000,
            dtStart: dtFirst.AddDays(-10),
            dtApply: dtFirst.AddDays(-20),
            dtEntry: dtFirst);


        var dtSecond = new DateTime(2025, 09, 15, 07, 22, 0);
        isinBaseOption = "IRO1SDZX0001";
        symbolBaseOption = await _factory.Repositories.SymbolAddAsync(isin: isinBaseOption);

        isinOption = "IRO1SDZX0011";
        symbolOption = await _factory.Repositories.SymbolAddAsync(isin: isinOption,
            typeOfSymbol: TypeOfSymbolTest.PutOption,
            dtEvent: dtSecond);

        await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 20000,
            dtStart: dtSecond.AddDays(-10),
            dtApply: dtSecond.AddDays(-20),
            dtEntry: dtSecond);



        var dtoFilter = new OptionPaginatedListRequestTest();
        var api = $"{_apiGetTable}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<OptionUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().HaveCount(2);

        var firstItem = apiResult.Data.Content.First();
        firstItem.SymbolBaseIsin.Should().Be("IRO1SDZX0001");
        firstItem.SymbolOptionIsin.Should().Be("IRO1SDZX0011");
        firstItem.ApplyPrice.Should().Be(20000);
        firstItem.StartDate.Should().Be(dtSecond.AddDays(-10).GetISOStringDate());
        firstItem.ApplyDate.Should().Be(dtSecond.AddDays(-20).GetISOStringDate());

        var secondItem = apiResult.Data.Content.Last();
        secondItem.SymbolBaseIsin.Should().Be("IRO1SDZJ0001");
        secondItem.SymbolOptionIsin.Should().Be("IRO1SDZJ0011");
        secondItem.ApplyPrice.Should().Be(20000);
        secondItem.StartDate.Should().Be(dtFirst.AddDays(-10).GetISOStringDate());
        secondItem.ApplyDate.Should().Be(dtFirst.AddDays(-20).GetISOStringDate());
    }

    [Fact]
    public async Task Should_be_able_to_get_all_data_in_details_for_an_option()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 09, 16, 12, 17, 0);
        var isinBaseOption = "IRO1SDZJ0001";
        var firmid = 120;

        await _factory.Repositories.FirmAddAsync(firmId: firmid);

        var symbolBaseOption = await _factory.Repositories.SymbolAddAsync(isin: isinBaseOption,
            firmId: firmid);

        var isinOption = "IRO1SDZJ0011";
        var symbolOption = await _factory.Repositories.SymbolAddAsync(isin: isinOption,
            typeOfSymbol: TypeOfSymbolTest.PutOption,
            dtEvent: dt,
            firmId: firmid);

        await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 20000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(50),
            dtEntry: dt);

        dt = new DateTime(2025, 09, 17, 12, 17, 0);
        var announdement = await _factory.Repositories.AnnouncementAddAsync(
            codalCode: 123,
            firmId: firmid);
        var dividendPerShareAnnouncement =
            await _factory.Repositories.DividendPerShareAddAsync(
                announcementId: announdement.AnnouncementIdPk,
                dtEntry: dt);

        await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 50000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(50),
            dtEntry: dt,
            announcementId: announdement.AnnouncementIdPk.ToString());


        var dtoFilter = new OptionPaginatedListRequestTest();
        var api = $"{_apiGetTable}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<OptionUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().HaveCount(1);

        var main = apiResult.Data.Content.First();
        main.SymbolBaseName.Should().Be("SYMBOL-NAME");
        main.SymbolBaseIsin.Should().Be("IRO1SDZJ0001");
        main.SymbolOptionName.Should().Be("SYMBOL-NAME");
        main.SymbolOptionIsin.Should().Be("IRO1SDZJ0011");
        main.ApplyPrice.Should().Be(50000.0m);
        main.State.Should().Be("تقسیم سود");
        main.ApplyDate.Should().Be("2025-11-06");
        main.StartDate.Should().Be("2025-09-07");
        main.OptionForFinance.Should().Be("0");
        main.OptionForFinanceDescription.Should().Be("تایین نشده");
        main.AnnouncementId.Should().Be(announdement.AnnouncementIdPk.ToString());
        main.EntryDate.Should().Be("2025-09-17");
        main.ModifyDate.Should().BeNull();
        main.IsDeleted.Should().BeFalse();
        main.OpFiUpdateDate.Should().BeNull();
        main.OpFiUpdateMode.Should().BeNull();
        main.ConfirmedManualTimeAnnoucement.Should().BeNull();
        main.Actions.Should().Contain("DETAILS");
        main.Actions.Should().Contain("EDIT");

        // Assert details
        main.Details.Should().NotBeNull();
        main.Details.Should().HaveCount(2);

        var detail1 = main.Details.ElementAt(0);
        detail1.SymbolBaseName.Should().Be("SYMBOL-NAME");
        detail1.SymbolBaseIsin.Should().Be("IRO1SDZJ0001");
        detail1.SymbolOptionName.Should().Be("SYMBOL-NAME");
        detail1.SymbolOptionIsin.Should().Be("IRO1SDZJ0011");
        detail1.ApplyPrice.Should().Be(50000.0m);
        detail1.State.Should().Be("تقسیم سود");
        detail1.ApplyDate.Should().Be("2025-11-06");
        detail1.StartDate.Should().Be("2025-09-07");
        detail1.OptionForFinance.Should().Be("0");
        detail1.OptionForFinanceDescription.Should().Be("تایین نشده");
        detail1.Details.Should().BeNull();
        detail1.AnnouncementId.Should().Be(announdement.AnnouncementIdPk.ToString());
        detail1.EntryDate.Should().Be("2025-09-17");
        detail1.ModifyDate.Should().BeNull();
        detail1.IsDeleted.Should().BeFalse();
        detail1.OpFiUpdateDate.Should().BeNull();
        detail1.OpFiUpdateMode.Should().BeNull();
        detail1.ConfirmedManualTimeAnnoucement.Should().BeNull();

        var detail2 = main.Details.ElementAt(1);
        detail2.SymbolBaseName.Should().Be("SYMBOL-NAME");
        detail2.SymbolBaseIsin.Should().Be("IRO1SDZJ0001");
        detail2.SymbolOptionName.Should().Be("SYMBOL-NAME");
        detail2.SymbolOptionIsin.Should().Be("IRO1SDZJ0011");
        detail2.ApplyPrice.Should().Be(20000.0m);
        detail2.State.Should().Be("شروع دوره");
        detail2.ApplyDate.Should().Be("2025-11-05");
        detail2.StartDate.Should().Be("2025-09-06");
        detail2.OptionForFinance.Should().Be("0");
        detail2.OptionForFinanceDescription.Should().Be("تایین نشده");
        detail2.Details.Should().BeNull();
        detail2.AnnouncementId.Should().BeNull();
        detail2.EntryDate.Should().Be("2025-09-16");
        detail2.ModifyDate.Should().BeNull();
        detail2.IsDeleted.Should().BeFalse();
        detail2.OpFiUpdateDate.Should().BeNull();
        detail2.OpFiUpdateMode.Should().BeNull();
        detail2.ConfirmedManualTimeAnnoucement.Should().BeNull();
    }


    [Fact]
    public async Task Should_be_able_to_get_option_with_CapitalChangeAnnouncement()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 09, 16, 12, 17, 0);
        var isinBaseOption = "IRO1SDZJ0001";
        var firmid = 120;

        await _factory.Repositories.FirmAddAsync(firmId: firmid);

        var symbolBaseOption = await _factory.Repositories.SymbolAddAsync(isin: isinBaseOption,
            firmId: firmid);

        var isinOption = "IRO1SDZJ0011";
        var symbolOption = await _factory.Repositories.SymbolAddAsync(isin: isinOption,
            typeOfSymbol: TypeOfSymbolTest.PutOption,
            dtEvent: dt,
            firmId: firmid);

        await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 20000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(50),
            dtEntry: dt);

        dt = new DateTime(2025, 09, 17, 12, 17, 0);
        var announdement = await _factory.Repositories.AnnouncementAddAsync(
            codalCode: 123,
            firmId: firmid,
            dtPublish: dt);
        var capitalChange = await _factory.Repositories.CapitalChangeAddAsync(
            announcementId: announdement.AnnouncementIdPk,
            dtEntry: dt);

        await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 50000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(50),
            dtEntry: dt,
            announcementId: announdement.AnnouncementIdPk.ToString());


        var dtoFilter = new OptionPaginatedListRequestTest();
        var api = $"{_apiGetTable}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<OptionUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().HaveCount(1);

        var main = apiResult.Data.Content.First();
        main.SymbolBaseName.Should().Be("SYMBOL-NAME");
        main.SymbolBaseIsin.Should().Be("IRO1SDZJ0001");
        main.SymbolOptionName.Should().Be("SYMBOL-NAME");
        main.SymbolOptionIsin.Should().Be("IRO1SDZJ0011");
        main.ApplyPrice.Should().Be(50000.0m);
        main.State.Should().Be("افزایش سرمایه");
        main.ApplyDate.Should().Be("2025-11-06");
        main.StartDate.Should().Be("2025-09-07");
        main.OptionForFinance.Should().Be("0");
        main.OptionForFinanceDescription.Should().Be("تایین نشده");
        main.AnnouncementId.Should().Be(announdement.AnnouncementIdPk.ToString());
        main.EntryDate.Should().Be("2025-09-17");
        main.ModifyDate.Should().BeNull();
        main.IsDeleted.Should().BeFalse();
        main.OpFiUpdateDate.Should().BeNull();
        main.OpFiUpdateMode.Should().BeNull();
        main.ConfirmedManualTimeAnnoucement.Should().BeNull();
        main.Actions.Should().Contain("DETAILS");
        main.Actions.Should().Contain("EDIT");

        // Assert details
        main.Details.Should().NotBeNull();
        main.Details.Should().HaveCount(2);

        var detail1 = main.Details.ElementAt(0);
        detail1.SymbolBaseName.Should().Be("SYMBOL-NAME");
        detail1.SymbolBaseIsin.Should().Be("IRO1SDZJ0001");
        detail1.SymbolOptionName.Should().Be("SYMBOL-NAME");
        detail1.SymbolOptionIsin.Should().Be("IRO1SDZJ0011");
        detail1.ApplyPrice.Should().Be(50000.0m);
        detail1.State.Should().Be("افزایش سرمایه");
        detail1.ApplyDate.Should().Be("2025-11-06");
        detail1.StartDate.Should().Be("2025-09-07");
        detail1.OptionForFinance.Should().Be("0");
        detail1.OptionForFinanceDescription.Should().Be("تایین نشده");
        detail1.Details.Should().BeNull();
        detail1.AnnouncementId.Should().Be(announdement.AnnouncementIdPk.ToString());
        detail1.EntryDate.Should().Be("2025-09-17");
        detail1.ModifyDate.Should().BeNull();
        detail1.IsDeleted.Should().BeFalse();
        detail1.OpFiUpdateDate.Should().BeNull();
        detail1.OpFiUpdateMode.Should().BeNull();
        detail1.ConfirmedManualTimeAnnoucement.Should().BeNull();

        var detail2 = main.Details.ElementAt(1);
        detail2.SymbolBaseName.Should().Be("SYMBOL-NAME");
        detail2.SymbolBaseIsin.Should().Be("IRO1SDZJ0001");
        detail2.SymbolOptionName.Should().Be("SYMBOL-NAME");
        detail2.SymbolOptionIsin.Should().Be("IRO1SDZJ0011");
        detail2.ApplyPrice.Should().Be(20000.0m);
        detail2.State.Should().Be("شروع دوره");
        detail2.ApplyDate.Should().Be("2025-11-05");
        detail2.StartDate.Should().Be("2025-09-06");
        detail2.OptionForFinance.Should().Be("0");
        detail2.OptionForFinanceDescription.Should().Be("تایین نشده");
        detail2.Details.Should().BeNull();
        detail2.AnnouncementId.Should().BeNull();
        detail2.EntryDate.Should().Be("2025-09-16");
        detail2.ModifyDate.Should().BeNull();
        detail2.IsDeleted.Should().BeFalse();
        detail2.OpFiUpdateDate.Should().BeNull();
        detail2.OpFiUpdateMode.Should().BeNull();
        detail2.ConfirmedManualTimeAnnoucement.Should().BeNull();
    }

    [Fact]
    public async Task Should_be_get_table_row_with_correct_actions()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 10, 21, 13, 55, 0);
        var isinBaseOption = "IRO1SDZJ0001";
        var firmid = 120;

        await _factory.Repositories.FirmAddAsync(firmId: firmid);

        var symbolBaseOption = await _factory.Repositories.SymbolAddAsync(isin: isinBaseOption,
            firmId: firmid);

        var isinOption = "IRO1SDZJ0011";
        var symbolOption = await _factory.Repositories.SymbolAddAsync(isin: isinOption,
            typeOfSymbol: TypeOfSymbolTest.PutOption,
            dtEvent: dt,
            firmId: firmid);

        await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 20000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(50),
            dtEntry: dt);

        dt = new DateTime(2025, 09, 17, 12, 17, 0);
        var announdement = await _factory.Repositories.AnnouncementAddAsync(
            codalCode: 123,
            firmId: firmid,
            dtPublish: dt);
        var capitalChange = await _factory.Repositories.CapitalChangeAddAsync(
            announcementId: announdement.AnnouncementIdPk,
            dtEntry: dt);

        await _factory.Repositories.PutOptionAddAsync(
            symbolId: symbolBaseOption.SymbolIdPk,
            symbolPutOptionId: symbolOption.SymbolIdPk,
            applyPrice: 50000,
            dtStart: dt.AddDays(-10),
            dtApply: dt.AddDays(50),
            dtEntry: dt,
            announcementId: announdement.AnnouncementIdPk.ToString());



        //--- case : not addred yet

        isinOption = "IRO1SDXD0011";
        var option2 = await _factory.Repositories.SymbolAddAsync(isin: isinOption,
             typeOfSymbol: TypeOfSymbolTest.PutOption,
             dtEvent: dt,
             firmId: firmid);


        var dtoFilter = new OptionPaginatedListRequestTest();
        var api = $"{_apiGetTable}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<OptionUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().HaveCount(2);


        var first = apiResult.Data.Content.ToList()[0];
        first.Actions.Should().Contain("CREATE");
        first.Actions.Should().NotContain("EDIT");
        first.Actions.Should().NotContain("DETAILS");
        first.OptionId.Should().BeNull();
        first.ApplyDate.Should().BeNull();
        first.SymbolBaseId.Should().BeNull();
        first.SymbolOptionId.Should().Be(option2.SymbolIdPk);


        var main = apiResult.Data.Content.ToList()[1];
        main.SymbolBaseName.Should().Be("SYMBOL-NAME");
        main.SymbolBaseIsin.Should().Be("IRO1SDZJ0001");
        main.SymbolOptionName.Should().Be("SYMBOL-NAME");
        main.SymbolOptionIsin.Should().Be("IRO1SDZJ0011");
        main.ApplyPrice.Should().Be(50000.0m);
        main.State.Should().Be("افزایش سرمایه");
        main.ApplyDate.Should().Be("2025-11-06");
        main.StartDate.Should().Be("2025-09-07");
        main.OptionForFinance.Should().Be("0");
        main.OptionForFinanceDescription.Should().Be("تایین نشده");
        main.AnnouncementId.Should().Be(announdement.AnnouncementIdPk.ToString());
        main.EntryDate.Should().Be("2025-09-17");
        main.ModifyDate.Should().BeNull();
        main.IsDeleted.Should().BeFalse();
        main.OpFiUpdateDate.Should().BeNull();
        main.OpFiUpdateMode.Should().BeNull();
        main.ConfirmedManualTimeAnnoucement.Should().BeNull();
        main.Actions.Should().Contain("DETAILS");
        main.Actions.Should().Contain("EDIT");
    }
}
