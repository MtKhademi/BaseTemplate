using MDF.DAL.Modules.Entities.OldEntities;
using MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;
using MDF.Test.Integration.Modules.AnnouncementModuleTest.Responses;

namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.AnnouncementTest.AnnouncementGetPaginatedListTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("ANNOUNCEMENT", "paginated-list")]
public partial class AnnouncementGetPaginatedListTest : BaseTest
{
    private readonly string _api = $"/api/v4/announcement/paginated-list";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AnnouncementGetPaginatedListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_paginatedList_data()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var paginatedList = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AnnouncementResponseTest>>();
        paginatedList.AssertionPaginatedListEmpty();

    }

    [Theory]
    [ClassData(typeof(AnnouncementGetPaginatedListRequestTestNotValidData))]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(AnnouncementGetPaginatedListRequestTest dto, List<string> errors)
    {
        //-ARRANGE
        var api = $"{_api}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(errors);
    }

    [Fact]
    public async Task Should_be_able_data_without_any_filter()
    {
        //-ARRANGE
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 123456);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 1234567);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 12345678);

        var dtoFilter = new AnnouncementGetPaginatedListRequestTest();
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AnnouncementResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Count().Should().Be(3);
    }


    [Fact]
    public async Task Should_be_able_get_announcement_with_CapitalChange_data()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 08, 03, 09, 06, 00);
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 123456, dtPublish: dt);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 1234567, dtPublish: dt);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 12345678, dtPublish: dt);
        var capitalChange = await _factory.Repositories.CapitalChangeAddAsync(
                announcementId: announcement.AnnouncementIdPk,
                dtEntry: dt,
              isAccept: true, approvalType: false, dtModify: dt);

        var dtoFilter = new AnnouncementGetPaginatedListRequestTest()
        {
            CodalCode = 123456,
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        // ...existing code...

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AnnouncementResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().HaveCount(1);

        var item = apiResult.Data.First();
        item.AnnouncementId.Should().Be(announcement.AnnouncementIdPk);
        item.CodalCode.Should().Be(123456);
        item.FirmId.Should().Be(1);
        item.SymbolIsin.Should().BeNull();
        item.SymbolName.Should().BeNull();
        item.OrganizationName.Should().BeNull();
        item.IsConfirm.Should().BeFalse();
        item.Title.Should().BeNull();
        item.TypeOfAnnouncement.Should().Be("CapitalChange");
        item.PublishDate.Should().Be("2025-08-03T09:06:00");
        item.ConfirmedName.Should().BeNull();
        item.ConfirmDateTime.Should().BeNull();
        item.HtmlLink.Should().BeNull();

        item.CapitalChange.Should().NotBeNull();
        item.CapitalChange.CapitalChangeIdPk.Should().Be(capitalChange.CapitalChangeIdPk);
        item.CapitalChange.AnnouncementIdFk.Should().Be(announcement.AnnouncementIdPk);
        item.CapitalChange.CapitalChangeTypeIdFk.Should().Be(1);
        item.CapitalChange.LastShareValue.Should().Be(100);
        item.CapitalChange.NewShareValue.Should().Be(100);
        item.CapitalChange.LastShareCount.Should().Be(10);
        item.CapitalChange.NewShareCount.Should().Be(10);
        item.CapitalChange.ApprovalType.Should().BeFalse();
        item.CapitalChange.IsAccept.Should().BeTrue();
        item.CapitalChange.EntryDate.Should().Be("2025-08-03");
        item.CapitalChange.ModifyDate.Should().Be("2025-08-03");
        item.CapitalChange.IsConfirmed.Should().BeFalse();
        item.CapitalChange.IsDeleted.Should().BeFalse();
        item.CapitalChange.InsertionType.Should().Be(0);
        item.CapitalChange.InsertedBy.Should().BeNull();
        item.CapitalChange.IsConfirmedManual.Should().BeNull();
        item.CapitalChange.ConfirmedManualTime.Should().BeNull();
        item.CapitalChange.IsUseSalb.Should().BeFalse();
        item.CapitalChange.CashIncoming.Should().Be(0);
        item.CapitalChange.RetaindedEarning.Should().Be(0);
        item.CapitalChange.Reserves.Should().Be(0);
        item.CapitalChange.SarfSaham.Should().Be(0);
        item.CapitalChange.RevaluationSurplus.Should().Be(0);

    }

    [Fact]
    public async Task Should_be_able_get_announcement_with_DividendPerShare_data()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 08, 03, 09, 06, 00);
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 123456, dtPublish: dt);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 1234567, dtPublish: dt);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 12345678, dtPublish: dt);
        var dividendPerShare = await _factory.Repositories.DividendPerShareAddAsync(
            dtEntry: dt,
            announcementId: announcement.AnnouncementIdPk, dtModify: dt);

        var dtoFilter = new AnnouncementGetPaginatedListRequestTest()
        {
            CodalCode = 123456,
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        // ...existing code...

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AnnouncementResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().HaveCount(1);


        var item = apiResult.Data.First();
        item.AnnouncementId.Should().Be(announcement.AnnouncementIdPk);
        item.CodalCode.Should().Be(123456);
        item.FirmId.Should().Be(1);
        item.SymbolIsin.Should().BeNull();
        item.SymbolName.Should().BeNull();
        item.OrganizationName.Should().BeNull();
        item.IsConfirm.Should().BeFalse();
        item.Title.Should().BeNull();
        item.TypeOfAnnouncement.Should().Be("DividenPerShare");
        item.PublishDate.Should().Be("2025-08-03T09:06:00");
        item.ConfirmedName.Should().BeNull();
        item.ConfirmDateTime.Should().BeNull();
        item.HtmlLink.Should().BeNull();
        item.CapitalChange.Should().BeNull();

        item.DividendPerShare.Should().NotBeNull();
        item.DividendPerShare.DividendPerShareIdPk.Should().Be(dividendPerShare.DividendPerShareIdPk);
        item.DividendPerShare.AnnouncementIdFk.Should().Be(announcement.AnnouncementIdPk);
        item.DividendPerShare.ProportionableRetainedEarnings.Should().Be(0);
        item.DividendPerShare.DividedRetainedEarning.Should().Be(0);
        item.DividendPerShare.LegalReserve.Should().Be(0);
        item.DividendPerShare.ExtenseReserve.Should().Be(0);
        item.DividendPerShare.Other.Should().Be(0);
        item.DividendPerShare.NetIncomeLoss.Should().Be(0);
        item.DividendPerShare.AnnualAdjustment.Should().Be(0);
        item.DividendPerShare.BeginingRetainedEarnings.Should().Be(0);
        item.DividendPerShare.PreYearDevidedRetainedEarning.Should().Be(0);
        item.DividendPerShare.PreYearOtherReserves.Should().Be(0);
        item.DividendPerShare.TransferToCapital.Should().Be(0);
        item.DividendPerShare.PreYearOtherReservesTransferToRetainedEarning.Should().Be(0);
        item.DividendPerShare.PreYearBoardMemberGift.Should().Be(0);
        item.DividendPerShare.EndingRetainedEarning.Should().Be(0);
        item.DividendPerShare.DividendPerShare1.Should().Be(0);
        item.DividendPerShare.EntryDate.Should().Be("2025-08-03");
        item.DividendPerShare.ModifyDate.Should().Be("2025-08-03");
        item.DividendPerShare.IsConfirmed.Should().BeFalse();
        item.DividendPerShare.IsDeleted.Should().BeFalse();
        item.DividendPerShare.InsertionType.Should().Be(0);
        item.DividendPerShare.InsertedBy.Should().Be("Mt.khademi");
        item.DividendPerShare.IsConfirmedManual.Should().BeFalse();
        item.DividendPerShare.ConfirmedManualTime.Should().BeNull();
        item.DividendPerShare.OnlyForPrevCap.Should().BeFalse();
        item.DividendPerShare.ListedCapital.Should().BeNull();

    }

    [Fact]
    public async Task Should_be_able_get_announcement_with_RightDurateDate_data()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 08, 03, 09, 06, 00);
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 123456, dtPublish: dt);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 1234567, dtPublish: dt);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 12345678, dtPublish: dt);
        var rightDurateDate = await _factory.Repositories.RightDurationAddAsync(
            announcementId: announcement.AnnouncementIdPk, 1,
            dtStart: dt, dtEnd: dt.AddDays(5),
            dtModifyTime: dt, dtentry: dt);

        var dtoFilter = new AnnouncementGetPaginatedListRequestTest()
        {
            CodalCode = 123456,
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        // ...existing code...

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AnnouncementResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().HaveCount(1);

        var item = apiResult.Data.First();
        item.AnnouncementId.Should().Be(announcement.AnnouncementIdPk);
        item.CodalCode.Should().Be(123456);
        item.FirmId.Should().Be(1);
        item.SymbolIsin.Should().BeNull();
        item.SymbolName.Should().BeNull();
        item.OrganizationName.Should().BeNull();
        item.IsConfirm.Should().BeTrue();
        item.Title.Should().BeNull();
        item.TypeOfAnnouncement.Should().Be("RightDuration");
        item.PublishDate.Should().Be("2025-08-03T09:06:00");
        item.ConfirmedName.Should().Be("MT.KHADEMI");
        item.ConfirmDateTime.Should().Be(dt.GetISOStringDateTime());
        item.HtmlLink.Should().BeNull();
        item.CapitalChange.Should().BeNull();
        item.DividendPerShare.Should().BeNull();

        item.RightDurationDate.Should().NotBeNull();
        item.RightDurationDate.RightDurationDateId.Should().Be(rightDurateDate.RightDurationDateId);
        item.RightDurationDate.StartDate.Should().Be("2025-08-03");
        item.RightDurationDate.EndDate.Should().Be("2025-08-08");
        item.RightDurationDate.Code.Should().Be(1);
        item.RightDurationDate.InsertedBy.Should().Be("MT.KHADEMI");
        item.RightDurationDate.EntryTime.Should().Be("2025-08-03");
        item.RightDurationDate.ModifiedTime.Should().Be("2025-08-03");
        item.RightDurationDate.AnnouncementId.Should().Be(announcement.AnnouncementIdPk);
        item.RightDurationDate.MeetingAnnouncementId.Should().Be(1);
        item.RightDurationDate.IsTraded.Should().BeNull();
        item.RightDurationDate.CalculationMethod.Should().BeNull();
        item.RightDurationDate.FirmId.Should().BeNull();
        item.RightDurationDate.Period.Should().BeNull();
        item.RightDurationDate.Price.Should().Be(0.0m);
        item.RightDurationDate.Description.Should().BeNull();

        item.DividendPaymentSchedules.Should().NotBeNull();
        item.DividendPaymentSchedules.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_be_able_get_announcement_with_DividendPaymentSchedule_data()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 08, 03, 09, 06, 00);
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 123456, dtPublish: dt);
        var announcementDividendPayment = await _factory.Repositories.AnnouncementAddAsync(codalCode: 1234567);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 12345678);
        await _factory.Repositories.DividendPaymentScheduleAddAsync(announcement.AnnouncementIdPk, announcementDividendPayment.AnnouncementIdPk,
            [
                new DividendPaymentScheduleDetail(){
                    FromChar = "A",
                    ToChar = "B",
                    FromCount = 1000,
                    ToCount = 2000,
                    FromDate = dt,
                    ToDate =dt.AddDays(5),
                    ShareHolderType = 1
                }
            ]);


        var dtoFilter = new AnnouncementGetPaginatedListRequestTest()
        {
            CodalCode = 123456,
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        // ...existing code...

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AnnouncementResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().HaveCount(1);

        var item = apiResult.Data.First();
        item.AnnouncementId.Should().Be(announcement.AnnouncementIdPk);
        item.CodalCode.Should().Be(123456);
        item.FirmId.Should().Be(1);
        item.SymbolIsin.Should().BeNull();
        item.SymbolName.Should().BeNull();
        item.OrganizationName.Should().BeNull();
        item.IsConfirm.Should().BeFalse();
        item.Title.Should().BeNull();
        item.TypeOfAnnouncement.Should().Be("Other");
        item.PublishDate.Should().Be("2025-08-03T09:06:00");
        item.ConfirmedName.Should().BeNull();
        item.ConfirmDateTime.Should().BeNull();
        item.HtmlLink.Should().BeNull();
        item.CapitalChange.Should().BeNull();
        item.DividendPerShare.Should().BeNull();
        item.RightDurationDate.Should().BeNull();

        item.DividendPaymentSchedules.Should().NotBeNull();
        item.DividendPaymentSchedules.Should().HaveCount(1);

        var schedule = item.DividendPaymentSchedules.First();
        schedule.Id.Should().Be(1);
        schedule.DividendAnnouncementIdFk.Should().Be(announcementDividendPayment.AnnouncementIdPk);
        schedule.AnnouncementIdFk.Should().Be(announcement.AnnouncementIdPk);
        schedule.Description.Should().Be("");
        schedule.DividendAnnouncementType.Should().Be(0);
        schedule.DividendPaymentScheduleDetails.Should().NotBeNull();
        schedule.DividendPaymentScheduleDetails.Should().HaveCount(1);

        var detail = schedule.DividendPaymentScheduleDetails.First();
        detail.DividendPaymentScheduleIdFk.Should().Be(1);
        detail.ShareHolderType.Should().Be(1);
        detail.FromDate.Should().Be("2025-08-03");
        detail.ToDate.Should().Be("2025-08-08");
        detail.FromCount.Should().Be(1000);
        detail.ToCount.Should().Be(2000);
        detail.FromChar.Should().Be("A");
        detail.ToChar.Should().Be("B");
        detail.Id.Should().Be(1);


    }

    [Theory]
    [InlineData(true, null, null)]
    [InlineData(true, "2025-08-31", "2025-08-31")]
    public async Task Should_be_get_announcement_base_filter(
        bool? isConfirm,
        string? startDate,
        string? endDate)
    {
        //-ARRANGE
        var dt = new DateTime(2025, 08, 31, 10, 48, 00);
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 123456, dtPublish: dt);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 1234567, dtPublish: dt);
        await _factory.Repositories.AnnouncementAddAsync(codalCode: 12345678, dtPublish: dt);
        var capitalChange = await _factory.Repositories.CapitalChangeAddAsync(
                announcementId: announcement.AnnouncementIdPk,
                dtEntry: dt,
              isAccept: true, approvalType: false, dtModify: dt);

        announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 123456, dtPublish: dt);
        capitalChange = await _factory.Repositories.CapitalChangeAddAsync(
            announcementId: announcement.AnnouncementIdPk,
            dtEntry: dt, isConfirm: true, isConfirmManual: true, dtConfirmManual: dt, insertBy: "Mt.Khademi",
            isAccept: true, approvalType: false, dtModify: dt);


        dt = new DateTime(2025, 08, 31, 02, 34, 00);
        announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 123456, dtPublish: dt);
        capitalChange = await _factory.Repositories.CapitalChangeAddAsync(
            announcementId: announcement.AnnouncementIdPk,
            dtEntry: dt,
            isConfirm: true,
            isConfirmManual: true,
            dtConfirmManual: dt.AddHours(3),
            dtModify: dt.AddHours(3),
            insertBy: "Mt.Khademi",
            isAccept: true, approvalType: false);


        var dtoFilter = new AnnouncementGetPaginatedListRequestTest()
        {
            IsConfirm = isConfirm,
            StartDateTime = startDate,
            EndDateTime = endDate,
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        // ...existing code...

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AnnouncementResponseTest>>();
        apiResult.Should().NotBeNull();

        foreach (var item in apiResult.Data)
        {
            if (isConfirm.HasValue)
            {
                item.IsConfirm.Should().Be(isConfirm);
                item.ConfirmedName.Should().NotBeNull();
                item.ConfirmDateTime.Should().NotBeNull();
            }
            else
            {
                item.ConfirmedName.Should().BeNull();
                item.ConfirmDateTime.Should().BeNull();
                item.IsConfirm.Should().BeFalse();
            }

            if (!string.IsNullOrWhiteSpace(startDate))
            {
                var startDt = startDate.GetDateFromISOFormat();
                item.PublishDate.Should().NotBeNull();
                item.PublishDate.GetDateFromISOFormat().Should().BeOnOrAfter(startDt);
            }

            //item.Title.Should().BeNull();
            //item.TypeOfAnnouncement.Should().Be("CapitalChange");
            //item.PublishDate.Should().Be("2025-08-03");
            //item.HtmlLink.Should().BeNull();

            //item.CapitalChange.Should().NotBeNull();
            //item.CapitalChange.CapitalChangeIdPk.Should().Be(capitalChange.CapitalChangeIdPk);
            //item.CapitalChange.AnnouncementIdFk.Should().Be(announcement.AnnouncementIdPk);
            //item.CapitalChange.CapitalChangeTypeIdFk.Should().Be(1);
            //item.CapitalChange.LastShareValue.Should().Be(100);
            //item.CapitalChange.NewShareValue.Should().Be(100);
            //item.CapitalChange.LastShareCount.Should().Be(10);
            //item.CapitalChange.NewShareCount.Should().Be(10);
            //item.CapitalChange.ApprovalType.Should().BeFalse();
            //item.CapitalChange.IsAccept.Should().BeTrue();
            //item.CapitalChange.EntryDate.Should().Be("2025-08-03");
            //item.CapitalChange.ModifyDate.Should().Be("2025-08-03");
            //item.CapitalChange.IsConfirmed.Should().BeFalse();
            //item.CapitalChange.IsDeleted.Should().BeFalse();
            //item.CapitalChange.InsertionType.Should().Be(0);
            //item.CapitalChange.InsertedBy.Should().BeNull();
            //item.CapitalChange.IsConfirmedManual.Should().BeNull();
            //item.CapitalChange.ConfirmedManualTime.Should().BeNull();
            //item.CapitalChange.IsUseSalb.Should().BeFalse();
            //item.CapitalChange.CashIncoming.Should().Be(0);
            //item.CapitalChange.RetaindedEarning.Should().Be(0);
            //item.CapitalChange.Reserves.Should().Be(0);
            //item.CapitalChange.SarfSaham.Should().Be(0);
            //item.CapitalChange.RevaluationSurplus.Should().Be(0);
        }
    }
}
