using MDF.Modules.DAL.Entities;
using MDF.Modules.Modules.AnnouncementModule.Builders.Entities;
using MDF.Modules.Modules.MutualFundModule.Abstractions.Enumerations;
using MDF.DAL.Modules.Entities.OldEntities;
using Microsoft.EntityFrameworkCore;
using MDF.Modules.SymbolModule.Depricate.Abstractions;
using MDF.Modules.SymbolModule.Depricate.PutOptionSection.Builders;
using MDF.Modules.SymbolModule.Depricate.FixedIncomeSection.Builders;
using MDF.Modules.SymbolModule.Depricate.ClosingPriceSection.Builders;
using MDF.Modules.SymbolModule.Depricate.AdjustedPriceSection.Builders;
using Modules.MutualFundModule.Contracts.Enumerations;

namespace MDF.Test.Integration.Fixtures;

internal class DatabaseRepositoryFixture
{
    private readonly DatabaseContext _context;

    public DatabaseRepositoryFixture(DatabaseContext context)
    {
        _context = context;
    }


    #region Announcement

    public async Task<Announcement> AnnouncementAddAsync(
        int codalCode,
        int firmId = 1, DateTime? dtPublish = null,
        int? announcementLinkId = null,
        string? title = null)
    {
        var dt = dtPublish ?? DateTime.Now;
        var announcement = new AnnouncementEntityBuilder()
       .WithCodalCode(codalCode)
       .WithFirmId(firmId)
       .WithPublishDate(dt)
       .WithAnnouncementTypeId(12)
       .Build();

        if (announcementLinkId.HasValue)
            announcement.AnnouncementIdPk = announcementLinkId.Value;

        announcement.Title = title ?? announcement.Title;
        await _context.Announcements.AddAsync(announcement);
        await _context.SaveChangesAsync();

        return announcement;
    }
    public async Task<List<Announcement>> AnnouncementGetsAsync()
    {
        return await _context.Announcements.ToListAsync();
    }
    public async Task<long> AnnouncementGetCountAsync()
    {
        return await _context.Announcements.LongCountAsync();
    }
    public async Task<Announcement?> AnnouncementGetByCodalCodeAsync(int codalCode)
    {
        return await _context.Announcements.Where(an => an.Code == codalCode)
            .FirstOrDefaultAsync();
    }
    public async Task<Announcement> AnnouncementGetByAnnouncementIdAsync(int announcementId)
    {
        return await _context.Announcements.FindAsync(announcementId);
    }
    public async Task<Announcement> AnnouncementAddAsync(Announcement entity)
    {

        await _context.Announcements.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    #endregion

    #region AnnouncementLink
    public async Task<AnnouncementLink1> AnnouncementLinkAddAsync(int announcementId, int isDownoaded = 0, byte linkType = 1, string linkUrl = "LINK")
    {
        var announcementLink = new AnnouncementLinkEntityBuilder()
            .WithAnnouncementIdFk(announcementId)
            .WithIsDownloaded(isDownoaded)
            .WithLinkType(linkType)
            .WithLinkUrl(linkUrl)
            .Build();
        await _context.AnnouncementLinks1.AddAsync(announcementLink);
        await _context.SaveChangesAsync();
        return announcementLink;
    }

    #endregion

    #region CapitalChange
    public async Task<CapitalChange?> CapitalChangeAddAsync(
        int announcementId, bool isAccept = true, bool approvalType = false,
        DateTime? dtModify = null, DateTime? dtEntry = null,
        string? insertBy = null, bool isConfirm = false, bool? isConfirmManual = null,
        DateTime? dtConfirmManual = null,
        double LastShareCount = 10,
        long LastShareValue = 100,
        long NewShareValue = 100,
        double NewShareCount = 10,
        byte capitalChangeType = 1,
        bool? isDeleted = null,
        short? insertionType = null,
        bool? isUseSalb = null
        )
    {
        var modify = dtModify ?? DateTime.Now;
        var entry = dtEntry ?? DateTime.Now;

        var capitalChange = new CapitalChangeEntityBuilder()
          .WithAnnouncementId(announcementId)
          .WithIsAccept(isAccept)
          .WithApprovalType(approvalType)
          .WithCapitalChangeTypeIdFk(1)
          .WithIsConfirm(isConfirm)
          .WithIsConfirmedManual(isConfirmManual)
          .WithConfirmedManualTime(dtConfirmManual)
          .WithModifyDate(modify)
          .WithEntryDate(entry)
          .WithInsertedBy(insertBy)
          .Build();


        capitalChange.LastShareValue = LastShareValue;
        capitalChange.LastShareCount = LastShareCount;
        capitalChange.NewShareValue = NewShareValue;
        capitalChange.NewShareCount = NewShareCount;
        capitalChange.IsAccept = isAccept;
        capitalChange.ApprovalType = approvalType;
        capitalChange.CapitalChangeTypeIdFk = capitalChangeType;

        capitalChange.IsDeleted = isDeleted ?? capitalChange.IsDeleted;
        capitalChange.InsertionType = insertionType ?? capitalChange.InsertionType;
        capitalChange.IsUseSalb = isUseSalb ?? capitalChange.IsUseSalb;

        await _context.CapitalChanges.AddAsync(capitalChange);
        await _context.SaveChangesAsync();

        return capitalChange;
    }
    public async Task<CapitalChange?> CapitalChangeGetByCodalCodeAsync(int codalCode)
    {

        var announcement = await _context.Announcements.Where(a => a.Code == codalCode)
          .FirstOrDefaultAsync();


        return await _context.CapitalChanges
            .Where(d => d.AnnouncementIdFk == announcement.AnnouncementIdPk)
            .Where(d => d.ApprovalType)
            .FirstOrDefaultAsync();
    }
    public async Task<CapitalChange?> CapitalChangeAddAsync(CapitalChange entity)
    {
        await _context.CapitalChanges.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<int> CapitalChangeGetCountAsync()
    {
        return await _context.CapitalChanges.CountAsync();
    }
    #endregion

    #region CapitalChangeItems

    public async Task<int> CapitalChangeItemsGetCountByCapitlaChangeIdAsync(int id)
    {
        return await _context.CapitalChangeItems.Where(x => x.CapitalChangeIdFk == id).CountAsync();
    }
    public async Task<List<CapitalChangeItem>?> CapitalChangeItemsGetsAsync()
    {
        return await _context.CapitalChangeItems.ToListAsync();
    }
    public async Task<List<CapitalChangeItem>?> CapitalChangeItemsGetByCapitalChangeIdAsync(int capitalChangeId)
    {
        return await _context.CapitalChangeItems.Where(x => x.CapitalChangeIdFk == capitalChangeId).ToListAsync();
    }
    public async Task<CapitalChangeItem?> CapitalChangeItemsAddAsync(
        int capitalChangeId, byte capitalChangeMethod, int value = 10)
    {
        var capitalChangeItem = new CapitalChangeItemEntityBuilder()
         .WithCapitalChangeIdFk(capitalChangeId)
         .WithCapitalChangeMethodIdFk(capitalChangeMethod)
         .WithValue(value)
         .Build();

        await _context.CapitalChangeItems.AddAsync(capitalChangeItem);
        await _context.SaveChangesAsync();
        return capitalChangeItem;
    }
    public async Task<CapitalChangeItem?> CapitalChangeItemsAddAsync(CapitalChangeItem entity)
    {
        await _context.CapitalChangeItems.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    #endregion

    #region CapitalChangeMethods
    public async Task<List<CapitalChangeMethod>?> CapitalChangeMethodGetsAsync()
    {
        return await _context.CapitalChangeMethods.ToListAsync();
    }
    public async Task<CapitalChangeMethod?> CapitalChangeMethodAddAsync(CapitalChangeMethod entity)
    {
        await _context.CapitalChangeMethods.AddAsync(entity);
        return entity;
    }
    #endregion

    #region DividendPerShare
    public async Task<DividendPerShare> DividendPerShareAddAsync(int announcementId, DateTime? dtEntry = null,
        DateTime? dtModify = null, DateTime? dtConfirmManual = null,
        bool isConfirm = false, string insertedBy = "Mt.khademi")
    {
        DateTime entry = dtEntry ?? DateTime.Now;
        DateTime modify = dtModify ?? DateTime.Now;

        var dividend = new DividendPerShareEntityBuilder()
         .WithAnnouncementIdFk(announcementId)
         .WithEntryDate(entry)
         .WithModifyDate(modify)
         .WithConfirmedManualTime(dtConfirmManual)
         .WithInsertedBy(insertedBy)
         .WithIsConfirmed(isConfirm)
         .Build();

        await _context.DividendPerShares.AddAsync(dividend);
        await _context.SaveChangesAsync();

        return dividend;
    }
    public async Task<int> DividendPerShareAddAsync(DividendPerShare entity)
    {
        await _context.DividendPerShares.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.DividendPerShareIdPk;
    }
    public async Task<DividendPerShare?> DividendPerShareGetByCodalCodeAsync(int codalCode)
    {

        var announcement = await _context.Announcements.Where(a => a.Code == codalCode)
          .FirstOrDefaultAsync();


        return await _context.DividendPerShares
              .FirstOrDefaultAsync(d => d.AnnouncementIdFk == announcement.AnnouncementIdPk);

    }
    public async Task<int> DividendPerShareGetCountAsync()
    {
        return await _context.DividendPerShares.CountAsync();
    }
    #endregion

    #region RightDurationDate

    public async Task<RightDurationDate?> RightDurationAddAsync(
        int announcementId, int meetingAnnouncementId,
        string insertBy = "MT.KHADEMI", DateTime? dtEnd = null, DateTime? dtStart = null,
        DateTime? dtModifyTime = null, DateTime? dtentry = null)
    {

        var end = dtEnd ?? DateTime.Now;
        var start = dtStart ?? DateTime.Now;
        var modifiTime = dtModifyTime ?? DateTime.Now;
        var entry = dtentry ?? DateTime.Now;
        var rightDuration = new RightDurationEntityBuilder()
              .WithAnnouncementId(announcementId)
              .WithMeetingAnnouncementId(meetingAnnouncementId)
              .WithInsertedBy(insertBy)
              .WithEndDate(end)
              .WithStartDate(start)
              .WithEntryTime(entry)
              .WithModifiedTime(modifiTime)
              .Build();
        await _context.RightDurationDates.AddAsync(rightDuration);
        await _context.SaveChangesAsync();
        return rightDuration;
    }
    public async Task<RightDurationDate?> RightDurationGetByCodalCodeAsync(int codalCode)
    {
        var announcement = await _context.Announcements.Where(a => a.Code == codalCode)
          .FirstOrDefaultAsync();

        return await _context.RightDurationDates
            .Where(d => d.AnnouncementId == announcement.AnnouncementIdPk)
            .FirstOrDefaultAsync();
    }
    public async Task<RightDurationDate?> RightDurationAddAsync(RightDurationDate entity)
    {
        await _context.RightDurationDates.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<int> RightDurationGetCountAsync()
    {
        return await _context.RightDurationDates.CountAsync();
    }

    #endregion

    #region DividendPaymentSchedule

    public async Task<DividendPaymentSchedule> DividendPaymentScheduleAddAsync(
        int announcementParentId, int announcementDividendPaymentId,
        ICollection<DividendPaymentScheduleDetail> details)
    {
        var dividendPaymentScheduleEntity = new DividendPaymentScheduleEntityBuilder()
             .WithAnnouncementIdFk(announcementParentId)
             .WithDividendAnnouncementIdFk(announcementDividendPaymentId)
             .WithDetails(details)
             .Build();

        await _context.DividendPaymentSchedules.AddAsync(dividendPaymentScheduleEntity);
        await _context.SaveChangesAsync();

        return dividendPaymentScheduleEntity;
    }

    #endregion

    #region Firm
    public async Task<List<Firm>> FirmGetsAsync()
    {
        return await _context.Firms.ToListAsync();
    }

    public async Task FirmAddAsync(int firmId)
    {
        await _context.Database.ExecuteSqlRawAsync(
          $" SET IDENTITY_INSERT dbo.Firm ON " +
          $" INSERT INTO dbo.Firm ([FirmId_PK], [OriginId], [FirmTypeId_FK], [FirmStateId_FK], " +
          $" [OrganizationId_FK], [ParentId], [Title], [Symbol], [ISIC], [ExecutiveManager]," +
          $" [ActivitySubject], [ManagementGroup], [Inspector], [AuditorName], [ListedCapital], " +
          $" [FinancialYear], [Address], [TelNo], [FaxNo], [OfficeAddress], [ShareOfficeAddress]," +
          $" [Website], [Email], [EntryDate], [ModifyDate], [FirmId_PK_New], [DisplaySymbol], [IsinCode], [IsDeleted])" +
          $" VALUES" +
          $" ({firmId}, 0, 0, 0, 0, NULL, N'', N'', '', NULL, NULL, N'', NULL, N'', 0, N'BULDER'," +
          $" N'', N'', N'', N'', N'', ''," +
          $" '', N'{DateTime.Now}', N'{DateTime.Now}', 0, N'FIRM3', N'IRO123123123', 0 )" +
          $" SET IDENTITY_INSERT dbo.Firm OFF");
        await _context.SaveChangesAsync();
    }
    #endregion

    #region IndustrialCategory

    public async Task<IndustrialCategory> IndustrialCategoryAddAsync(string code,
        string title = "Industrial category title",
        int? parentId = null)
    {
        var entity = new IndustrialCategory()
        {
            ParentId = parentId,
            Title = title,
            Code = code
        };
        await _context.IndustrialCategories.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<IndustrialCategory> IndustrialCategoryAddAsync(IndustrialCategory entity)
    {
        await _context.IndustrialCategories.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<IndustrialCategory?> IndustrialCategoryGetByCodeAsync(string code)
    {
        return await _context.IndustrialCategories
               .FirstOrDefaultAsync(ind => ind.Code == code);
    }

    #endregion

    #region CompanyType

    public async Task<CompanyType> CompanyTypeAddAsync(CompanyType entity)
    {
        await _context.CompanyTypes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<CompanyType?> CompanyTypeGetByCodeAsync(string code)
    {
        return await _context.CompanyTypes
               .FirstOrDefaultAsync(ind => ind.Code == code);
    }

    #endregion

    #region Company

    public async Task<Company> CompanyAddAsync(
        string code = "CODE",
        string title = "TITLE",
        int industrialCategoryId = 1)
    {
        var company = new Company
        {
            CompanyTypeIdFk = 1,
            CompanyCode = code,
            DateOfEvent = DateTime.Now,
            FirmIdFk = 1,
            Title = title,
            IndustrialCategoryIdFk = industrialCategoryId
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        return company;
    }
    public async Task<Company> CompanyAddAsync(Company entity)
    {
        await _context.Companies.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    //public async Task<Company?> CompanyGetByCodeAsync(string code)
    //{
    //    return await _context.Companies
    //           .FirstOrDefaultAsync(ind => ind.Code == code);
    //}

    #endregion

    #region Instrument


    public async Task<InstrumentType> InstrumentTypeAddAsync(InstrumentType entity)
    {
        await _context.InstrumentTypes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Instrument> InstrumentAddAsync(
        string title,
        int companyId = 1,
        byte instrumentTypeId = 1,
        string isin = "IRB5AE800046",
        int unitCount = 200000)
    {
        var entity = new Instrument
        {
            CompanyIdFk = companyId,
            InstrumentTypeIdFk = instrumentTypeId,
            EnTitle = title,
            Isin = isin,
            UnitCount = unitCount,
            DateOfEvent = DateTime.Now,
            IsCompelete = true,
            IsImport = true,
            IsImported = true,
        };
        await _context.Instruments.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Instrument> InstrumentAddAsync(Instrument entity)
    {
        await _context.Instruments.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    //public async Task<Instrument?> InstrumentGetByCodeAsync(string code)
    //{
    //    return await _context.Companies
    //           .FirstOrDefaultAsync(ind => ind.Code == code);
    //}

    #endregion

    #region Symbol

    public async Task<SymbolStateType> SymbolStateTypeAddAsync(
        string code = "A",
        string title = "مجاز",
        string enTitle = "Order entry authorized-Instrument open")
    {


        SymbolStateType? entity = await
            _context.SymbolStateTypes.SingleOrDefaultAsync(x => x.Code == code);

        if (entity != null)
            return entity;
        entity = new SymbolStateType
        {
            Code = code,
            Title = title,
            EnTitle = enTitle
        };

        await _context.SymbolStateTypes.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<SymbolTradingStateType> SymbolTradingStateTypeAddAsync(
        string code = "S",
        string title = "Suspension of trading for an instrument, or an options class suspension of calculation of a stock index")
    {
        SymbolTradingStateType? entity =
            await _context.SymbolTradingStateTypes.SingleOrDefaultAsync(x => x.Code == code);

        if (entity != null)
            return entity;

        entity = new SymbolTradingStateType
        {
            Code = code,
            Title = title,
        };

        await _context.SymbolTradingStateTypes.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<SymbolStateAction> SymbolStateActionAddAsync(
        string code = "P",
        string title = "Programming of a deferred opening time for the instrument")
    {
        SymbolStateAction? entity =
            await _context.SymbolStateActions.SingleOrDefaultAsync(x => x.Code == code);

        if (entity != null)
            return entity;

        entity = new SymbolStateAction
        {
            Code = code,
            Title = title,
        };

        await _context.SymbolStateActions.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }


    public async Task<SymbolState> SymbolStateAddAsync(
        int symbolId,
        byte symbolTradingStateTypeId,
        byte symbolStateTypeId,
        byte symbolStateActionId,
        DateTime dateOfEvent)
    {
        var entity = new SymbolState
        {
            SymbolIdFk = symbolId,
            DateOfEvent = dateOfEvent,
            SymbolTradingStateTypeIdFk = symbolTradingStateTypeId,
            SymbolStateTypeIdFk = symbolStateTypeId,
            SymbolStateActionIdFk = symbolStateActionId,
        };

        await _context.SymbolStates.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }


    public async Task<SymbolState> SymbolStateAddAsync(
        int symbolId,
        DateTime dateOfEvent,
        string stateTypeCode = "A",
        string tradingStateCode = "AD",
        string stateActionCode = "AC",
        string stateTypeTitle = "مجاز")
    {

        try
        {
            var stateType = await SymbolStateTypeAddAsync(code: stateTypeCode, title: stateTypeTitle);
            var tradingState = await SymbolTradingStateTypeAddAsync(code: tradingStateCode);
            var stateAction = await SymbolStateActionAddAsync(code: stateActionCode);

            var entity = new SymbolState
            {
                SymbolIdFk = symbolId,
                DateOfEvent = dateOfEvent,
                SymbolTradingStateTypeIdFk = tradingState.SymbolTradingStateTypeIdPk,
                SymbolStateTypeIdFk = stateType.SymbolStateTypeIdPk,
                SymbolStateActionIdFk = stateAction.SymbolStateActionIdPk,
                StartDateForSuspension = DateTime.Now,
                SymbolOriginReservation = "",
                SymbolStateTcs = "",

            };

            await _context.SymbolStates.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<SymbolGroup> SymbolGroupAddAsync(string title, string code)
    {
        var entity = new SymbolGroup
        {
            Title = title,
            Code = code
        };
        await _context.SymbolGroups.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Symbol> SymbolAddAsync(
        string isin,
        string enName = "EN-SYMBOL",
        string symbolName = "SYMBOL-NAME",
        DateTime? dtCreate = null,
        DateTime? dtEntry = null,
        DateTime? dtEvent = null,
        DateTime? dtDisable = null,
        byte exchangeBoardId = 1,
        int exchangeMarketId = 1,
        int instrumentId = 1,
        byte symbolGroupId = 1,
        int? firmId = 1,
        TypeOfSymbolTest typeOfSymbol = TypeOfSymbolTest.Undefined,
        bool? isDisable = false,
        int? securityExchangeMarket = null,
        byte? securityExchangeBoard = null,
        string? bourseCode = null,
        TypeOfSymbolTest typeOfSymbolInTseTmc = TypeOfSymbolTest.Undefined,
        string? cdsSymbolName = default!,
        DateTime? dtLastUpdate = default!,
        long? instCodeTse = default!,
        int? settlementPeriod = default!)
    {
        var dt = dtCreate ?? DateTime.Now;
        var entry = dtEntry ?? DateTime.Now;
        var @event = dtEvent ?? DateTime.Now;
        var symbol = new SymbolEntityBuilderTest()
               .WithIsin(isin)
               .WithEnSymbol(enName)
               .WithSymbolName(symbolName)
               .WithCreatedDateTime(dt)
               .WithEntryDate(entry)
               .WithDateOfEvent(@event)
               .WithExchangeBoardIdFk(exchangeBoardId)
               .WithExchangeMarketIdFk(exchangeMarketId)
               .WithInstrumentId(instrumentId)
               .WithSymbolGroupId(symbolGroupId)
               .WithFirmId(firmId)
               .WithTypeOfSymbolInTseTmc(typeOfSymbolInTseTmc)
               .WithTypeOfSymbol(typeOfSymbol)
               .WithIsDisabled(isDisable)
               .WithDisableDateTime(dtDisable)
               .WithBourseCode(bourseCode)
               .Build();
        symbol.CdsSymbolName = cdsSymbolName;
        symbol.LastModifiedDate = dtLastUpdate ?? symbol.LastModifiedDate;
        if (securityExchangeMarket.HasValue) symbol.ExchangeMarketIdFk = securityExchangeMarket.Value;
        if (securityExchangeBoard.HasValue) symbol.ExchangeBoardIdFk = securityExchangeBoard.Value;
        symbol.InstCodeTse = instCodeTse;
        symbol.SettlementPeriod = settlementPeriod;

        await _context.Symbols.AddAsync(symbol);
        await _context.SaveChangesAsync();
        return symbol;
    }
    public async Task<Symbol> SymbolAddAsync(Symbol entity)
    {
        await _context.Symbols.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<Symbol?> SymbolGetByIsinAsync(string isin)
    {
        return await _context.Symbols
            .Include(x => x.ExchangeBoardIdFkNavigation)
            .ThenInclude(x => x.BoardIdFkNavigation)
            .Include(x => x.ExchangeMarketIdFkNavigation)
            .ThenInclude(x => x.MarketIdFkNavigation)
            .Include(x => x.ExchangeMarketIdFkNavigation)
            .ThenInclude(x => x.SecuritiesExchangeIdFkNavigation)
            .FirstOrDefaultAsync(ind => ind.Isin == isin);
    }

    public async Task<int?> SymbolGetsCountAsync()
    {
        return await _context.Symbols.CountAsync();
    }

    #endregion

    #region FixedIncome

    public async Task<FixedIncome?> FixedIncomeAddAsync(
        int instrumentId,
        DateTime? dtPublish = null,
        DateTime? dtEntry = null,
        DateTime? dtModify = null,
        DateTime? dtMaturity = null,
        DateTime? dtSubscriptionEnd = null,
        DateTime? dtSubscriptionStart = null,
        DateTime? dtTradeStart = null,
        double nominalPrice = 1000,
        int interestRate = 10,
        byte interestPaymentInterval = 0,
        string description = "",
        int fixedIncomeTypeId = 1,
        int redeemRate = 10,
        int duration = 2)
    {

        var fixedIncome = new FixedIncomeEntityBuilder()
                  .WithInstrumentIdFk(instrumentId)
                  .WithPublicationDate(dtPublish)
                  .WithEntryDate(dtEntry ?? DateTime.Now)
                  .WithModifyDate(dtModify ?? DateTime.Now)
                  .WithMaturityDate(dtMaturity ?? DateTime.Now)
                  .WithSubscriptionEndDate(dtSubscriptionEnd ?? DateTime.Now)
                  .WithSubscriptionStartDate(dtSubscriptionStart ?? DateTime.Now)
                  .WithTradeStartDate(dtTradeStart ?? DateTime.Now)
                  .WithNominalValue(nominalPrice)
                  .WithInterestRate(interestRate)
                  .WithDescription(description)
                  .WithRedeemedRate(redeemRate)
                  .WithInterestPaymentInterval(interestPaymentInterval)
                  .WithDuration(duration)
                  .WithFixedIncomeTypeId(1)
                  .Build();
        await _context.FixedIncomes.AddAsync(fixedIncome);
        await _context.SaveChangesAsync();

        return fixedIncome;
    }


    public async Task<FixedIncome?> FixedIncomeGetByIsinAsync(string isin)
    {
        var sy = await _context.Symbols.SingleOrDefaultAsync(xx => xx.Isin == isin);
        return await _context.FixedIncomes
            .FirstOrDefaultAsync(xx => xx.InstrumentIdFk == sy.InstrumentIdFk);
    }

    public async Task<IEnumerable<FixedIncomeProfitDaily>?> FixedIncomeProfitDailyGetsByIsisnAsync(string isin)
    {
        var sy = await _context.Symbols.SingleOrDefaultAsync(xx => xx.Isin == isin);
        var fix = await _context.FixedIncomes
            .FirstOrDefaultAsync(xx => xx.InstrumentIdFk == sy.InstrumentIdFk);

        return await _context.FixedIncomeProfitDailies.Where(x => x.FixedIncomeIdFk == fix.FixedIncomeIdPk).ToListAsync();
    }

    public async Task FixedIncomeProfitDailyAddAsync(int fixedIncomeId,
        DateTime dtProfitDaily, bool? isHasAdjusted = null,
        DateTime? dtModified = null)
    {
        var fixedIncomeProfit = new FixedIncomeProfitDailyBuilder()
           .WithFixedIncomeIdFk(fixedIncomeId)
           .WithProfitDailyPrice(10)
           .WithSubscriptionProfit(10)
           .WithProfitDailyDate(dtProfitDaily)
           .WithModifiedDate(dtModified ?? DateTime.Now)
           .WithNominalPrice(5)
           .WithHasAdjusted(isHasAdjusted)
           .Build();
        await _context.FixedIncomeProfitDailies.AddAsync(fixedIncomeProfit);
        await _context.SaveChangesAsync();
    }

    #endregion

    #region SalafFixedIncome


    public async Task<SalafProfit> SalafProfitAddAsync(int fixedIncomeId,
        DateTime dtOfEvent, int numberOfDay = 365,
        decimal price = 10, bool isDeleted = false)
    {
        var profit = new SalafProfit()
        {
            FixedIncomeIdFk = fixedIncomeId,
            DateOfEvent = dtOfEvent,
            NumberOfDay = numberOfDay,
            Price = price,
            CreatedOnDate = DateTime.Now,
            IsDeleted = isDeleted
        };
        await _context.SalafProfits.AddAsync(profit);
        await _context.SaveChangesAsync();
        return profit;
    }
    public async Task<SalafFixedIncome> SalafFixedIncomeAddAsync(int FixedIncomeIdPk,
        int? EachContractAmount = null,
        int? Producer = null, decimal? BuyConsequentialPrice = null, decimal? SellConsequentialPrice = null,
        decimal? EachTonNominalPrice = null, decimal? EachTonIpoprice = null, DateTime? SecondaryTradeStartDate = null,
        DateTime? SecondrayTradeEndDate = null)
    {
        var salaf = new SalafFixedIncome()
        {
            EachContractAmount = EachContractAmount,
            Producer = Producer,
            BuyConsequentialPrice = BuyConsequentialPrice,
            SellConsequentialPrice = SellConsequentialPrice,
            EachTonNominalPrice = EachTonNominalPrice,
            EachTonIpoprice = EachTonIpoprice,
            SecondaryTradeStartDate = SecondaryTradeStartDate,
            SecondrayTradeEndDate = SecondrayTradeEndDate,
            FixedIncomeIdPk = FixedIncomeIdPk
        };
        await _context.SalafFixedIncomes.AddAsync(salaf);
        await _context.SaveChangesAsync();
        return salaf;
    }
    public async Task<SalafFixedIncome?> SalafFixedIncomeGetByIsinAsync(string isin)
    {
        var sy = await _context.Symbols.SingleOrDefaultAsync(xx => xx.Isin == isin);
        var fixedIncome = await _context.FixedIncomes
            .FirstOrDefaultAsync(xx => xx.InstrumentIdFk == sy.InstrumentIdFk);
        return await _context.SalafFixedIncomes.SingleOrDefaultAsync(x => x.FixedIncomeIdPk == fixedIncome.FixedIncomeIdPk);
    }

    public async Task<IEnumerable<SalafProfit>?> SalafProfitGetsByFixedIncomeIdAsync(int fixedIncomeId)
    {
        return await _context.SalafProfits.Where(x => x.FixedIncomeIdFk == fixedIncomeId).ToListAsync();
    }

    #endregion

    #region FixedIncomeConstituents

    public async Task FixedIncomeConstituentsAddAsync(int fixedIncomeId, int partyId, ETypeOfFixedIncomeConstituentTypeTest typeConst)
    {
        byte typeConstb = (byte)typeConst;
        await _context.FixedIncomeConstituents.AddAsync(new FixedIncomeConstituent
        {
            FixedIncomeIdFk = fixedIncomeId,
            PartyIdFk = partyId,
            FixedIncomeConstituentTypeIdFk = typeConstb
        });
        await _context.SaveChangesAsync();

    }

    public async Task<IEnumerable<FixedIncomeConstituent>> FixedIncomeConstituentsGetsAsync(int fixedIncomeId)
        => await _context.FixedIncomeConstituents
            .Where(x => x.FixedIncomeIdFk == fixedIncomeId).ToListAsync();
    public async Task<IEnumerable<FixedIncomeConstituent>> FixedIncomeConstituentsGetsAsync()
        => await _context.FixedIncomeConstituents.ToListAsync();


    #endregion

    #region PartyType

    public async Task<PartyType> PartyTypeAddAsync(string title, string code)
    {
        var partyType = new PartyType
        {
            Code = code,
            EnTitle = title,
            Title = title
        };
        await _context.PartyTypes.AddAsync(partyType);
        await _context.SaveChangesAsync();

        return partyType;
    }

    #endregion

    #region Party

    public async Task PartyAddAsync(int partyId, int partyTypeId)
    {
        await _context.Database.ExecuteSqlRawAsync(
            $"SET IDENTITY_INSERT dbo.Party ON " +
            $"INSERT INTO dbo.Party " +
            $"(PartyId_PK,PartyTypeId_FK) " +
            $"VALUES({partyId},{partyTypeId}) " +
            $"SET IDENTITY_INSERT dbo.Party OFF ");
    }


    #endregion

    #region AdjustedPrice

    public async Task<AdjustedPriceArchive> AdjustedPriceArchiveAddAsync(string isin,
        decimal closingPrice,
        decimal lastPrice,
        DateTime dt,
        int? capitalChangeId = null,
        int? dividendPerShare = null,
        bool isDeleted = false)
    {

        var symbol = await SymbolGetByIsinAsync(isin);
        if (symbol is null)
            symbol = await SymbolAddAsync(isin: isin);

        var closingPriceEntity = await ClosingPriceArchiveAddAsync(symbol.SymbolIdPk, dt, ((double)closingPrice), ((double)lastPrice));

        //var adjusted = new AdjustedPriceEntityBuilder()
        //   .WithClosingPriceId(closingPriceEntity.ClosingPriceIdPk)
        //   .WithDate(dt)
        //   .WithSymbolId(symbol.SymbolIdPk)
        //   .WithIsAdjusted(true)
        //   .WithAdjustedLastPrice(lastPrice)
        //   .WithAdjustedPrice(closingPrice)
        //   .WithCapitalChangeId(capitalChangeId)
        //   .WithDividendId(dividendPerShare)
        //   .Build();
        var adjustedPrice = new AdjustedPriceArchive
        {
            AdjustedLastPrice = lastPrice,
            AdjustedPrice1 = closingPrice,
            Date = dt,
            SymbolId = symbol.SymbolIdPk,
            CapitalChangeId = capitalChangeId,
            DividendId = dividendPerShare,
            ClosingPriceId = closingPriceEntity.ClosingPriceIdPk,
        };

        adjustedPrice.IsDeleted = isDeleted;

        await _context.AdjustedPriceArchives.AddAsync(adjustedPrice);
        await _context.SaveChangesAsync();

        return adjustedPrice;
    }

    public async Task<AdjustedPrice> AdjustedPriceAddAsync(string isin,
        decimal closingPrice,
        decimal lastPrice,
        DateTime dt,
        int? capitalChangeId = null,
        int? dividendPerShare = null,
        bool isDeleted = false)
    {

        var symbol = await SymbolGetByIsinAsync(isin);
        if (symbol is null)
            symbol = await SymbolAddAsync(isin: isin);

        var closingPriceEntity = await ClosingPriceAddAsync(symbol.SymbolIdPk, dt, ((double)closingPrice), ((double)lastPrice));

        var adjusted = new AdjustedPriceEntityBuilder()
            .WithClosingPriceId(closingPriceEntity.ClosingPriceIdPk)
           .WithDate(dt)
           .WithSymbolId(symbol.SymbolIdPk)
           .WithIsAdjusted(true)
           .WithAdjustedLastPrice(lastPrice)
           .WithAdjustedPrice(closingPrice)
           .WithCapitalChangeId(capitalChangeId)
           .WithDividendId(dividendPerShare)
           .Build();

        adjusted.IsDeleted = isDeleted;

        await _context.AdjustedPrices.AddAsync(adjusted);
        await _context.SaveChangesAsync();

        return adjusted;
    }

    public async Task<AdjustedPrice> AdjustedPriceAddAsync(int symbolId,
        int closingPriceId = 0, DateTime? dt = null)
    {
        var adjusted = new AdjustedPriceEntityBuilder()
            .WithClosingPriceId(closingPriceId)
           .WithDate(dt ?? DateTime.Now)
           .WithSymbolId(symbolId)
           .WithIsAdjusted(true)
           .WithAdjustedLastPrice(15)
           .WithAdjustedPrice(10)
           .Build();
        await _context.AdjustedPrices.AddAsync(adjusted);
        await _context.SaveChangesAsync();

        return adjusted;
    }
    public async Task<AdjustedPrice> AdjustedPriceAddAsync(AdjustedPrice entity)
    {
        await _context.AdjustedPrices.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<IEnumerable<AdjustedPrice>> AdjustedPriceGetsAsync()
    {
        return await _context.AdjustedPrices.ToListAsync();
    }

    #endregion

    #region Closing Price



    public async Task<ClosingPrice_archive> ClosingPriceArchiveAddAsync(int symbolId, DateTime dtOfEvent,
            double closingPrice = 15.02, double lastPrice = 16.02)
    {
        //var closingPriceEntity = new ClosingPriceEntityBuilder()
        //   .WithClosingPrice(closingPrice)
        //   .WithCloseIndicatorIdFk(2)
        //   .WithLastTradePrice(lastPrice)
        //   .WithDateOfEvent(dtOfEvent)
        //   .WithDateOfEvent2(dtOfEvent)
        //   .WithSymbolIdFk(symbolId)
        //   .Build();
        var closingPriceEntity = new ClosingPrice_archive
        {
            ClosingPrice1 = closingPrice,
            CloseIndicatorIdFk = 2,
            LastTradePrice = lastPrice,
            DateOfEvent = dtOfEvent,
            DateOfEvent2 = dtOfEvent,
            SymbolIdFk = symbolId
        };
        await _context.ClosingPrice_Archives.AddAsync(closingPriceEntity);
        await _context.SaveChangesAsync();

        return closingPriceEntity;
    }
    public async Task<ClosingPrice> ClosingPriceAddAsync(string isin, DateTime dtOfEvent,
        double closingPrice = 15.02, double lastPrice = 16.02)
    {
        var symbol = await SymbolGetByIsinAsync(isin);
        if (symbol is null)
            symbol = await SymbolAddAsync(isin: isin);

        var closingPriceEntity = new ClosingPrice
        {
            ClosingPrice1 = closingPrice,
            CloseIndicatorIdFk = 2,
            LastTradePrice = lastPrice,
            DateOfEvent = dtOfEvent,
            DateOfEvent2 = dtOfEvent,
            SymbolIdFk = symbol.SymbolIdPk,
        };
        await _context.ClosingPrices.AddAsync(closingPriceEntity);
        await _context.SaveChangesAsync();

        return closingPriceEntity;
    }
    public async Task<ClosingPrice> ClosingPriceAddAsync(int symbolId, DateTime dtOfEvent,
            double closingPrice = 15.02, double lastPrice = 16.02)
    {
        var closingPriceEntity = new ClosingPriceEntityBuilder()
           .WithClosingPrice(closingPrice)
           .WithCloseIndicatorIdFk(2)
           .WithLastTradePrice(lastPrice)
           .WithDateOfEvent(dtOfEvent)
           .WithDateOfEvent2(dtOfEvent)
           .WithSymbolIdFk(symbolId)
           .Build();
        await _context.ClosingPrices.AddAsync(closingPriceEntity);
        await _context.SaveChangesAsync();

        return closingPriceEntity;
    }
    public async Task<ClosingPrice> ClosingPriceAddAsync(ClosingPrice entity)
    {
        await _context.ClosingPrices.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<IEnumerable<ClosingPrice>> ClosingPriceGetsAsync(int symbolId, DateTime dtOfEvent)
    {
        return await _context.ClosingPrices
            .Where(x => x.SymbolIdFk == symbolId)
            .Where(x => x.DateOfEvent.Date == dtOfEvent.Date)
            .ToListAsync();
    }

    #endregion

    #region Closing Price Type

    public async Task<ClosingPriceType> ClosingPriceTypeAddAsync(ClosingPriceType entity)
    {
        await _context.ClosingPriceTypes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    #endregion

    #region Closing Price Indicator

    public async Task<CloseIndicator> ClosingPriceIndicatorAddAsync(CloseIndicator entity)
    {
        await _context.CloseIndicators.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    #endregion

    #region ChangeMarket

    public async Task<IEnumerable<SymbolMarketChange>> ChangeMarketGetssync()
    {
        return await _context.SymbolMarketChanges.ToListAsync();
    }
    public async Task<SymbolMarketChange> ChangeMarketGetsync(string fromIsin)
    {
        var symbol = await _context.Symbols.SingleOrDefaultAsync(x => x.Isin == fromIsin);
        var entity = await _context.SymbolMarketChanges.SingleOrDefaultAsync(x => x.FromSymbolId == symbol.SymbolIdPk);
        return entity;
    }
    public async Task<SymbolMarketChange> ChangeMarketAddAsync(SymbolMarketChange entity)
    {
        await _context.SymbolMarketChanges.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<SymbolMarketChange> ChangeMarketAddAsync(string fromIsin, string toIsin, DateTime dt, bool isFinal = false)
    {
        var symbol1 = await _context.Symbols.SingleOrDefaultAsync(x => x.Isin == fromIsin);
        if (symbol1 is null)
            symbol1 = await SymbolAddAsync(isin: fromIsin);
        var symbol2 = await _context.Symbols.SingleOrDefaultAsync(x => x.Isin == toIsin);
        if (symbol2 is null)
            symbol2 = await SymbolAddAsync(isin: toIsin);


        return await ChangeMarketAddAsync(new SymbolMarketChange
        {
            ChangeDate = dt,
            CreateDate = dt,
            FromSymbolId = symbol1.SymbolIdPk,
            FromSymbolIdCloseDate = dt.AddDays(1),
            IsFinal = isFinal,
            ToSymbolId = symbol2.SymbolIdPk,
            ToSymbolIdOpenDate = dt.AddDays(2)
        });
    }
    public async Task<SymbolMarketChange> ChangeMarketAddAsync(
        string fromIsin, string toIsin, DateTime dtCreate, DateTime dtClose, DateTime dtOpen,
        int? fromFirmId = null, int? toFirmId = null, bool isFinal = false)
    {
        var symbol1 = await _context.Symbols.SingleOrDefaultAsync(x => x.Isin == fromIsin);
        if (symbol1 is null)
            symbol1 = await SymbolAddAsync(isin: fromIsin, firmId: fromFirmId);
        var symbol2 = await _context.Symbols.SingleOrDefaultAsync(x => x.Isin == toIsin);
        if (symbol2 is null)
            symbol2 = await SymbolAddAsync(isin: toIsin, firmId: toFirmId);


        return await ChangeMarketAddAsync(new SymbolMarketChange
        {
            ChangeDate = dtCreate,
            CreateDate = dtCreate,
            FromSymbolId = symbol1.SymbolIdPk,
            FromSymbolIdCloseDate = dtClose,
            IsFinal = isFinal,
            ToSymbolId = symbol2.SymbolIdPk,
            ToSymbolIdOpenDate = dtOpen
        });
    }


    #endregion

    #region PutOption

    public async Task<PutOption> PutOptionAddAsync(int symbolId, int symbolPutOptionId, decimal applyPrice,
        DateTime dtStart, DateTime dtApply, string announcementId = null,
        ETypeOfPutOptinForFinance? forFinance = null,
        bool? isDeleted = false,
        DateTime? dtConfirmManualAnnouncement = null,
        DateTime? dtModify = null,
        DateTime? dtEntry = null,
        decimal? applyPricetest1 = null,
        decimal? applyPriceTest2 = null,
        short? meetingType = null)
    {
        var putOption = new PutOptionEntityBuilder()
           .WithSymbolIdFk(symbolId)
           .WithPutOptionSymbolIdFk(symbolPutOptionId)
           .WithApplyPrice(applyPrice)
           .WithStartDate(dtStart)
           .WithApplyDate(dtApply)
           .WithModifyDate(dtModify)
           .WithIsDeleted(isDeleted)
           .WithAnnouncementIdFk(announcementId)
           .WithOptionForFinance(forFinance)
           .WithConfirmedManualTimeAnnoucement(dtConfirmManualAnnouncement)
           .WithEntryDate(dtEntry)
           .WithApplyPriceTest1(applyPricetest1)
           .WithApplyPriceTest2(applyPriceTest2)
           .WithMeetingType(meetingType)
           .Build();
        await _context.PutOptions.AddAsync(putOption);
        await _context.SaveChangesAsync();

        return putOption;
    }

    public async Task<List<PutOption>> PutOptionGetsBasePutOptionIsinAync(string symbolPutOptionIsin)
    {
        var symbol = await _context.Symbols.SingleOrDefaultAsync(x => x.Isin == symbolPutOptionIsin);
        var entitys = await _context.PutOptions.Where(x => x.PutOptionSymbolIdFk == symbol.SymbolIdPk).ToListAsync();
        return entitys;
    }
    public async Task<List<PutOption>> PutOptionGetsAllAsync()
    {
        return await _context.PutOptions.ToListAsync();
    }

    #endregion

    #region Organizations

    public async Task OrganizationAddAsync(
        int organizationId,
        string nationalCode = "123",
        DateTime? registerDate = null,
        string title = "Organization Title",
        string registerNumber = "123")
    {
        //-- ADD NEW ORGANIZATION FOR [MRKETER,GARUNTOR,PUBLISHER]
        //var organization = new OrganizationEntityBuilder()
        //   .WithNationalCode(nationalCode)
        //   .WithRegisterDate(registerDate ?? DateTime.Now)
        //   .WithTitle(title)
        //   .WithRegisterNumber(registerNumber)
        //   .Build();
        //await _context.Organizations.AddAsync(organization);
        //await _context.SaveChangesAsync();

        //return organization;

        string sqlCommand =
            @$"SET IDENTITY_INSERT dbo.Organization ON
                    INSERT INTO dbo.Organization(   
                    OrganizationId_PK,
                    RegisterCityId_FK,  
                    OrganizationTypeId_FK,  
                    NationalCode,   
                    RegisterNumber,  
                    RegisterDate,   
                    Title, 
                    URLId
                    )VALUES( 
                    {organizationId},
                    {registerNumber},
                    6,
                    N'{nationalCode}',
                    N'', 
                    N'{registerDate ?? DateTime.Now}',
                    N'{title}',
                    NULL )

                    SET IDENTITY_INSERT dbo.Organization OFF";

        await _context.Database.ExecuteSqlRawAsync(sqlCommand);
        await _context.SaveChangesAsync();
    }

    public async Task<Organization?> OrganizationGetByAsync(string nationalCode)
        => await _context.Organizations
            .SingleOrDefaultAsync(x => x.NationalCode == nationalCode);

    public async Task<OrganizationType> OrganizationTypeAddAsync(
        string code = "123",
        string title = "Title",
        string enTitle = "en title")
    {
        var entity = new OrganizationType()
        {
            Code = code,
            EnTitle = enTitle,
            Title = title
        };
        await _context.OrganizationTypes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    #endregion

    #region Mutual funds

    public async Task<Mffee> FundFeeAddAsync(
        int mutualFundIdFk,
        decimal? subscriptionFixedFee = null,
        decimal? redemptionFixedFee = null,
        double? subscriptionVariableFee = null,
        decimal? subscriptionVariableMaximumFee = null,
        double? rewardPercentage = null,
        DateTime? eventDate = null,
        DateTime? createdOnDate = null)
    {
        var entity = new Mffee
        {
            MutualFundIdFk = mutualFundIdFk,
            SubscriptionFixedFee = subscriptionFixedFee ?? 0m,
            RedemptionFixedFee = redemptionFixedFee ?? 0m,
            SubscriptionVariableFee = subscriptionVariableFee ?? 0.0,
            SubscriptionVariableMaximumFee = subscriptionVariableMaximumFee ?? 0m,
            RewardPercentage = rewardPercentage ?? 0.0,
            EventDate = eventDate ?? DateTime.Now,
            CreatedOnDate = createdOnDate ?? DateTime.Now
        };
        await _context.Set<Mffee>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }


    public async Task<MutualFund> FundAddAsync(string seoRegisterNumber,
        string title = "Fund Title Default",
        FundProviderTest fundProvider = FundProviderTest.NotSet,
        FundTypeTest fundType = FundTypeTest.Unknown,
        string fundIsin = "",
        string symbolIsin = "",
        DateTime? dtStart = default!,
        FundXMLTypeTest? fundXMLType = null,
        int? organizationIdFk = default!,
        int? managerOrganizationIdFk = default!,
        DateTime? dtLastChange = default!)
    {
        var entity = new MutualFund()
        {
            SeoregisterNumber = seoRegisterNumber,
            Title = title,
            DateApproved = DateTime.UtcNow,
            DateLastChanged = dtLastChange ?? DateTime.Now,
            DateSeoregistered = DateTime.UtcNow,
            DateStart = dtStart ?? DateTime.Now,
            MutualFundTypeIdFk = (int)fundType,
            FundProvider = (ETypeOfFundProvider)((int)fundProvider),
            FundXMLType = fundXMLType.HasValue ? (FundXMLType)((int?)fundXMLType) : FundXMLType.Unknown,
            Isin = fundIsin,
            SymbolIsin = symbolIsin,
            OrganizationIdFk = organizationIdFk,
            ManagerOrganizationIdFk = managerOrganizationIdFk,
        };
        await _context.MutualFunds.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }


    public async Task<List<MfdailyFinancial>> FundNavGetWithSeoRegisterNumber(string seoRegisterNumber, DateTime dtFinancial)
    {
        var fund = await _context.MutualFunds.SingleOrDefaultAsync(x => x.SeoregisterNumber == seoRegisterNumber);
        var entitys = await _context.MfdailyFinancials.Where(x => x.MutualFundIdFk == fund.MutualFundIdPk
            && x.EventDate.Date == dtFinancial.Date).ToListAsync();
        return entitys;
    }


    public async Task<MfdailyFinancial> FundNavDailyAddAsync(
        string seoRegisterNumber, DateTime dtFinancial,
        int? mfDailyPreviousId = null, DateTime? dtLastChange = null)
    {
        var fund = await _context.MutualFunds.SingleOrDefaultAsync(x => x.SeoregisterNumber == seoRegisterNumber);
        var entity = new MfdailyFinancial
        {
            MutualFundIdFk = fund.MutualFundIdPk,
            NavRedemption = 11000,
            NavSubscription = 11069,
            NavStat = 11000,
            NetAsset = 11000,
            NetChange = 11000,
            ChangePercent = 11000,
            InvestorsUnits = 11000,
            UnitsRedemption = 11000,
            UnitsSubscription = 11000,
            EventDate = dtFinancial.Date,
            DateLastChange = dtLastChange.HasValue ? dtLastChange.Value.Date : dtFinancial.Date,
            InstitutionInvestmentNo = 11000,
            RetailInvestmentNo = 11000,
            RetailInvestmentPercent = 11000,
            InstitutionInvestmentPercent = 11000,
            PreviousDailyFinancialIdFk = mfDailyPreviousId,
        };
        await _context.MfdailyFinancials.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<MfdailyFinancial?> FundNavDailyGetsync(string seoRegisterNumber, DateTime dtEvent)
    {
        var fund = await _context.MutualFunds.SingleOrDefaultAsync(x => x.SeoregisterNumber == seoRegisterNumber);
        return await _context.MfdailyFinancials.Where(x => x.MutualFundIdFk == fund.MutualFundIdPk && x.EventDate.Date == dtEvent.Date).FirstOrDefaultAsync();
    }

    #endregion

    #region Market

    public async Task<IEnumerable<Market>> MarketGetsAsync() => await _context.Markets.ToListAsync();
    public async Task<ExchangeMarket?> MarketExchangeGetAsync(string marketCode, byte securitiesExchangeCode)
    {
        return await _context.ExchangeMarkets
            .Include(ex => ex.MarketIdFkNavigation)
            .Include(ex => ex.SecuritiesExchangeIdFkNavigation)
            .Where(ex => ex.MarketIdFkNavigation.Code == marketCode)
            .Where(ex => ex.SecuritiesExchangeIdFkNavigation.Code == securitiesExchangeCode)
            .FirstOrDefaultAsync();
    }

    public async Task<ExchangeMarket> MarketExchangeAddAsync(string marketCode, byte securitiesExchangeCode)
    {
        var mar = new Market
        {
            Code = marketCode,
            Title = "Market Title",
        };
        await _context.Markets.AddAsync(mar);

        var sec = new SecuritiesExchange
        {
            Code = securitiesExchangeCode,
            Title = "Securities Exchange Title",
        };
        await _context.SecuritiesExchanges.AddAsync(sec);

        await _context.SaveChangesAsync();

        var xx = new ExchangeMarket
        {
            MarketIdFk = mar.MarketIdPk,
            SecuritiesExchangeIdFk = sec.SecuritiesExchangeIdPk,
            MarketIdFkNavigation = mar,
            SecuritiesExchangeIdFkNavigation = sec
        };
        await _context.ExchangeMarkets.AddAsync(xx);
        await _context.SaveChangesAsync();
        return xx;
    }

    public async Task<ExchangeMarket> MarketExchangeAddAsync(Market market, SecuritiesExchange securitiesExchange)
    {

        await _context.Markets.AddAsync(market);
        await _context.SecuritiesExchanges.AddAsync(securitiesExchange);
        await _context.SaveChangesAsync();

        var xx = new ExchangeMarket
        {
            MarketIdFk = market.MarketIdPk,
            SecuritiesExchangeIdFk = securitiesExchange.SecuritiesExchangeIdPk,
            MarketIdFkNavigation = market,
            SecuritiesExchangeIdFkNavigation = securitiesExchange
        };
        await _context.ExchangeMarkets.AddAsync(xx);
        await _context.SaveChangesAsync();
        return xx;
    }

    #endregion

    #region Board


    public async Task<ExchangeBoard?> BoardExchangeGetAsync(byte boardCode, byte securitiesExchangeCode)
    {
        return await _context.ExchangeBoards
            .Include(ex => ex.BoardIdFkNavigation)
            .Include(ex => ex.SecuritiesExchangeIdFkNavigation)
            .Where(ex => ex.BoardIdFkNavigation.BoardCode == boardCode)
            .Where(ex => ex.SecuritiesExchangeIdFkNavigation.Code == securitiesExchangeCode)
            .FirstOrDefaultAsync();
    }
    public async Task<ExchangeBoard> BoardExchangeAddAsync(byte boardCode, byte securitiesExchange)
    {
        // Step 1: Add Board if not exists
        var board = await _context.Set<Board>().FirstOrDefaultAsync(b => b.BoardCode == boardCode);
        if (board == null)
        {
            board = new Board
            {
                BoardCode = boardCode,
                Title = $"Board {boardCode}",
            };
            await _context.Set<Board>().AddAsync(board);
            await _context.SaveChangesAsync();
        }

        // Step 2: Add SecuritiesExchange if not exists
        var secExchange = await _context.Set<SecuritiesExchange>().FirstOrDefaultAsync(se => se.Code == securitiesExchange);
        if (secExchange == null)
        {
            secExchange = new SecuritiesExchange
            {
                Code = securitiesExchange,
                Title = $"Securities Exchange {securitiesExchange}"
            };
            await _context.Set<SecuritiesExchange>().AddAsync(secExchange);
            await _context.SaveChangesAsync();
        }

        // Step 3: Add ExchangeBoard if not exists
        var exchangeBoard = await _context.ExchangeBoards
            .FirstOrDefaultAsync(eb => eb.BoardIdFk == board.BoardIdPk && eb.SecuritiesExchangeIdFk == secExchange.SecuritiesExchangeIdPk);

        if (exchangeBoard == null)
        {
            exchangeBoard = new ExchangeBoard
            {
                BoardIdFk = board.BoardIdPk,
                SecuritiesExchangeIdFk = secExchange.SecuritiesExchangeIdPk
            };
            await _context.ExchangeBoards.AddAsync(exchangeBoard);
            await _context.SaveChangesAsync();
        }

        return exchangeBoard;
    }

    public async Task<ExchangeBoard> BoardExchangeAddAsync(Board boardEntity, SecuritiesExchange securitiesExchangeEntity)
    {
        if (await _context.Set<Board>().FirstOrDefaultAsync(b => b.BoardCode == boardEntity.BoardCode) == null)
        {
            await _context.Set<Board>().AddAsync(boardEntity);
            await _context.SaveChangesAsync();
        }
        else
        {
            boardEntity = await _context.Set<Board>().FirstOrDefaultAsync(b => b.BoardCode == boardEntity.BoardCode);
        }

        if (await _context.Set<SecuritiesExchange>().FirstOrDefaultAsync(se => se.Code == securitiesExchangeEntity.Code) == null)
        {
            await _context.Set<SecuritiesExchange>().AddAsync(securitiesExchangeEntity);
            await _context.SaveChangesAsync();
        }
        else
        {
            securitiesExchangeEntity = await _context.Set<SecuritiesExchange>().FirstOrDefaultAsync(se => se.Code == securitiesExchangeEntity.Code);
        }

        var exchangeBoard = new ExchangeBoard
        {
            BoardIdFk = boardEntity.BoardIdPk,
            SecuritiesExchangeIdFk = securitiesExchangeEntity.SecuritiesExchangeIdPk
        };
        await _context.ExchangeBoards.AddAsync(exchangeBoard);
        await _context.SaveChangesAsync();

        return exchangeBoard;
    }


    #endregion

    #region Index Data

    public async Task<IndexDatum> IndexDataAddAsync(int symbolId, double indexData, DateTime dtOfEvent,
        double? PercentVariation = 0,
        short? SignVariation = 0)
    {
        var entity = new IndexDatum
        {
            SymbolIdFk = symbolId,
            DateOfEvent = dtOfEvent,
            AggregateDayLastIndexLevel = indexData,
            IndexLevelIdFk = 2,
            NoNonTradedSymbolIndex = 339,
            NoSuspendedSymbolIndex = 47,
            TotalNoSymbolIndex = 386,
            TimeOfDayHighestIndexLevel = new TimeSpan(10, 13, 0),
            TimeOfDayLowestIndexLevel = new TimeSpan(13, 13, 0),
            DayLastIndexLevel = 0,
            DayHighestIndexLevel = 0,
            NoTradedSymbolsInIndex = 0,
            PercentageActiveSymbolInIndex = 0,
            SignOfVariationDayIndex = 0,
            VarationForDayIndex = 0,
            SignAverageVariationSymbolIndex = 0,
            VariationDayIndexDayReference = PercentVariation ?? 0,
            SignVariationDayIndexDayReference = SignVariation ?? 0,
            VariationDayIndexLastForPreviousYear = 0,
            NetReturnIndexLevel = 0,
            GrossReturnIndexLevel = 0,
            NoOfDecliningSymbolIndex = 0,
            NoRisingSymbolIndex = 0,
            NoUnchangedSymbolIndex = 0,
            NoReservedSymbolIndex = 0,
            AverageVariationSymbolIndex = 0,
            SignVariationDayLastPreviousYear = 0,
            AverageVariationDecliningSymbolIndex = 0,
            AverageVariationRisingSymbolIndex = 0,
            FlagIndicatorsRelatedIndexLevel = false,
            AggregateDayHighestIndexLevel = 0,
            AggregateDayLowestIndexLevel = 0,

        };
        await _context.IndexData.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    #endregion

    #region Configs


    public async Task ConfigDeleteAll()
    {
        _context.Configs.RemoveRange(await _context.Configs.ToListAsync());
        await _context.SaveChangesAsync();
    }
    public async Task ConfigCreates(params ConfigEntity[] entities)
    {
        await _context.Configs.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    #endregion

    #region FixedIncome for checking trading

    public async Task<IEnumerable<FixedIncomeForCheckingTrading>> FixedIncomeForCheckingTradingsGetsAsync()
    {
        return await _context.FixedIncomeForCheckingTradings.ToListAsync();
    }

    public async Task FixedIncomeForCheckingTradingsAddRangeAsync(IEnumerable<string> isins)
    {
        _context.FixedIncomeForCheckingTradings.AddRange(
           isins.Select(isin => new FixedIncomeForCheckingTrading
           {
               Isin = isin
           }));
        await _context.SaveChangesAsync();
    }

    #endregion

    #region Trade

    public async Task TradeAddAsync(int symbolId, DateTime dtOfEvent)
    {
        await _context.Trades.AddAsync(new Trade
        {
            SymbolIdFk = symbolId,
            BuyingBrokerIdFk = 1,
            SellingBrokerIdFk = 1,
            TechnicalOriginOfbuyOrderIdFk = 1,
            TechnicalOriginOfsellOrderIdFk = 1,
            ClearingAccountTypeOfBuyerIdFk = 1,
            ClearingAccountTypeOfSellerIdFk = 1,
            TradedQuantity = 10,
            TradePrice = 10,
            LastTradePriceVariation = 0,
            CrossTradeFlag = 0,
            FlagEndOfTrades = 0,
            SignOfPriceVariation = 0,
            TradeNumber = 0,
            TradeDateTime = dtOfEvent,
            PriceVariation = 0,
            TradeType = false,
            DateOfEvent = dtOfEvent,
        });
        await _context.SaveChangesAsync();
    }

    #endregion

    #region Calendar

    public async Task CalendarAddAsync(DateTime dtOfEvent, bool isHoliday = false, string description = "Test Description")
    {
        await _context.Calendars.AddAsync(new Calendar
        {
            CalendarDate = dtOfEvent.ToShortDateString(),
            CalendarQuarter = (byte)((dtOfEvent.Month - 1) / 3 + 1),
            CalendarSemester = (byte)((dtOfEvent.Month - 1) / 6 + 1),
            CalendarTypeId = 1,
            CalendarYear = ((short)dtOfEvent.Year),
            GregorianDate = dtOfEvent,
            IsHoliday = isHoliday,
            MonthDayNumber = (byte)dtOfEvent.Day,
            MonthId = dtOfEvent.Month,
            WeekDayId = (byte)dtOfEvent.DayOfWeek,
            YearDayNumber = (short)dtOfEvent.DayOfYear,
            YearWeekNumber = 1
        });
        await _context.SaveChangesAsync();
    }

    #endregion
}
