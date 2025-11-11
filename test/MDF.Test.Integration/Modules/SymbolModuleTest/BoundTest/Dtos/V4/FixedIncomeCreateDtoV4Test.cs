namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.V4;


public record FixedIncomeCreateDtoV4Test(
    /// <summary>
    /// ایزین نماد
    /// </summary>
    string? SymbolIsin = default!,

    /// <summary>
    /// نوع فرمول
    /// </summary>
    FormulaTypeTest? FormulaType = default!,

    /// <summary>
    /// ناشر
    /// </summary>
    int? PublisherId = default!,


    /// <summary>
    /// بازارگردان
    /// </summary>
    int? MarketMakerId = default!,


    /// <summary>
    /// ضامن
    /// </summary>

    int? GurantorId = default!,

    /// <summary>
    /// نرخ سود سالانه
    /// </summary>
    float? InterestRate = default!,

    /// <summary>
    /// نرخ بازخرید
    /// </summary>
    float? RedeemedRate = default!,

    /// <summary>
    /// مدت زمان خرید و فروش اوراق 
    /// </summary>
    double? Duration = default!,

    /// <summary>
    /// مبلغ اسمی
    /// </summary>
    float? NominalValue = default!,

    /// <summary>
    /// دوره پرداخت سود
    /// </summary>
    int? InterestPaymentInterval = default!,

    /// <summary>
    /// تاریخ شروع معامله
    /// </summary>
    string? TradeStartDate = default!,

    /// <summary>
    /// تاریخ سررسید
    /// </summary>
    string? MaturityDate = default!,

    /// <summary>
    /// تاریخ شروع پذیره نویسی
    /// </summary>
    string? SubscriptionStartDate = default!,

    /// <summary>
    /// تاریخ پایان پذیره نویسی
    /// </summary>
    string? SubscriptionEndDate = default!,

    string? PublicationDate = default!,

    /// <summary>
    /// توضیحات
    /// </summary>
    string? Description = default!,
    int? CountOfPublished = null

);
